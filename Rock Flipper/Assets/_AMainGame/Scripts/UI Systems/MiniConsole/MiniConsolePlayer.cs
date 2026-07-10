using CommandTerminal;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace BSB.UISystems
{
    public class MiniConsolePlayer : ExtendedMonoBehaviour
    {
        protected delegate float Command(CommandArg[] args, MiniConsolePlayer miniConsolePlayer);

        [SerializeField]
        protected MiniConsole miniConsole;

        [Space]
        [SerializeField]
        private TextAsset defaultTextAsset;
        [SerializeField]
        private MiniConsolePlayerContext defaultContext;
        [SerializeField]
        private bool playOnEnabled;

        [Space]
        [SerializeField]
        private bool disableTime;
        [SerializeField]
        private float defaultIntervalMin = 1 / 60.0f;
        [SerializeField]
        private float defaultIntervalMax = 1 / 12.0f;

        [Space]
        [SerializeField]
        private bool displayTimeStamp;

        [Space]
        [SerializeField]
        private TimeScaleMode timeScaleMode = TimeScaleMode.GameplayUnscaledTimeAbsolute;
        [SerializeField]
        private TimeScaleMode timeStampScaleMode = TimeScaleMode.GameplayUnscaledTimeAbsolute;

        [Space]
        [SerializeField]
        private UnityEvent onFinished;

        private bool editor_FinishNowFlag;

        private List<CommandArg> arguments = new List<CommandArg>();
        private List<string> importedLines = new List<string>();
        private List<string> importedLinesTmp = new List<string>();

        private System.Action finishCallback;
        private TextAsset textAsset;
        private MiniConsolePlayerContext context;
        private float waitScale = 1;
        private bool waitForAnyKeyFlag;

        protected Dictionary<string, Command> commandDictionary = new Dictionary<string, Command>()
        {
            { "_SETWAITS", CmdSetWaitScale },
            { "_WAIT", CmdWait },
            {"_WAITS", CmdWaitScale },
            {"_WAITFORANYKEY", CmdWaitForAnyKey },
            {"_RANDOMINT", CmdDisplayStringWithRandomInteger },
            {"_IMPORT", CmdImport }
        };

        protected float RandomInterval => waitScale * Random.Range(defaultIntervalMin, defaultIntervalMax);

        protected void OnEnable()
        {
            ///
            if (playOnEnabled)
            {
                ClearAndPlay();
            }

            ///
            CommonCheatLib.OnCheatSignalEmitted += CommonCheatLib_OnCheatSignalEmitted;
        }

        protected void OnDisable()
        {
            StopAllCoroutines();

            ///
            CommonCheatLib.OnCheatSignalEmitted -= CommonCheatLib_OnCheatSignalEmitted;
        }

        private void CommonCheatLib_OnCheatSignalEmitted(string obj)
        {
            editor_FinishNowFlag = true;
        }

        [ContextMenu("ClearAndPlay")]
        public void ClearAndPlay()
        {
            ClearAndPlay(defaultTextAsset, defaultContext, null);
        }

        [ContextMenu("Play")]
        public void Play()
        {
            ///
            Play(defaultTextAsset, defaultContext, null);
        }

        public void ClearAndPlay(TextAsset textAsset, MiniConsolePlayerContext context, System.Action finishCallback)
        {
            ///
            miniConsole.Clear();

            ///
            Play(textAsset, context, finishCallback);
        }

        public void Play(TextAsset textAsset, MiniConsolePlayerContext context, System.Action finishCallback)
        {
            ///
            TryInit();

            ///
            this.textAsset = textAsset;
            this.finishCallback = finishCallback;
            this.context = context;

            ///
            StopAllCoroutines();
            StartCoroutine(PlayRoutine());
        }

        private IEnumerator PlayRoutine()
        {
            ///
            editor_FinishNowFlag = false;
            waitScale = 1;
            waitForAnyKeyFlag = false;

            ///
            using (StringReader stringReader = new StringReader(textAsset.text))
            {
                ///
                string line;
                float deltaTime = 0;
                importedLines.Clear();

                ///
                bool finished = false;

                ///
                while (true)
                {
                    ///
                    deltaTime += GetDeltaTime(timeScaleMode);

                    ///
                    if (finished || editor_FinishNowFlag)
                    {
                        break;
                    }

                    ///
                    while (deltaTime > 0)
                    {
                        // Get new line
                        if (importedLines.Count > 0)
                        {
                            line = importedLines[0];
                            importedLines.RemoveAt(0);
                        }
                        else
                        {
                            line = stringReader.ReadLine();
                        }

                        ///
                        if (line == null)
                        {
                            finished = true;
                            break;
                        }

                        ///
                        deltaTime -= ProcessLine(line);

                        ///
                        if (waitForAnyKeyFlag)
                        {
                            break;
                        }
                    }

                    ///
                    if (waitForAnyKeyFlag)
                    {
                        ///
                        deltaTime = 0;

                        ///
                        waitForAnyKeyFlag = false;

                        ///
                        yield return new WaitForAnyKeyPressed();
                    }

                    ///
                    yield return null;
                }
            }
            ;

            ///
            onFinished?.Invoke();
            finishCallback?.Invoke();
        }

        private float ProcessLine(string line)
        {
            ///
            if (string.IsNullOrWhiteSpace(line) || line[0] != '_')
            {
                return ProcessLineAsLiteralString(line);
            }

            ///
            return ProcessLineAsCommandLine(line);
        }

        private float ProcessLineAsCommandLine(string line)
        {
            ///
            CommandShell.GetArguments(line, arguments);

            ///
            if (arguments.Count == 0)
            {
                ///
                Debug.LogErrorFormat("Invalid console command: {0}", line);

                ///
                return 0;
            }

            ///
            string command_name = arguments[0].String.ToUpper();
            arguments.RemoveAt(0); // Remove command name from arguments

            ///
            return RunCommand(command_name, arguments.ToArray());
        }

        private float RunCommand(string upperCommandName, CommandArg[] arguments)
        {
            ///
            var command = FindCommand(upperCommandName);

            ///
            if (command == null)
            {
                ///
                Debug.LogErrorFormat("Invalid console command name: {0}", upperCommandName);

                ///
                return 0;
            }

            ///
            return command(arguments, this);
        }

        private Command FindCommand(string upperCommandName)
        {
            ///
            Command command = null;

            ///
            if (commandDictionary.TryGetValue(upperCommandName, out command))
            {
                return command;
            }

            ///
            return null;
        }

        private float ProcessLineAsLiteralString(string line)
        {
            return ProcessLineAsLiteralString(line, RandomInterval);
        }

        private float ProcessLineAsLiteralString(string line, float interval)
        {
            ///
            var str = displayTimeStamp ? AddTimeStampPrefix(line) : line;

            ///
            miniConsole.PushItem(str);

            ///
            return interval;
        }

        private string AddTimeStampPrefix(string line)
        {
            ///
            var time = System.TimeSpan.FromSeconds(GetTime(timeStampScaleMode));

            ///
            return string.Format(@"[{0:hh\:mm\:ss\.ff}] - {1}", time, line);
        }

        #region Commands
        private static float CmdWait(CommandArg[] args, MiniConsolePlayer miniConsolePlayer)
        {
            ///
            if (miniConsolePlayer.disableTime)
            {
                return 0;
            }

            ///
            float minSeconds = args[0].Float;
            float maxSeconds = args.Length >= 2 ? args[1].Float : minSeconds;

            ///
            return Random.Range(minSeconds, maxSeconds);
        }

        private static float CmdWaitScale(CommandArg[] args, MiniConsolePlayer miniConsolePlayer)
        {
            ///
            if (miniConsolePlayer.disableTime)
            {
                return 0;
            }

            ///
            float scale = args[0].Float;

            ///
            return miniConsolePlayer.RandomInterval * scale;
        }

        private static float CmdDisplayStringWithRandomInteger(CommandArg[] args, MiniConsolePlayer miniConsolePlayer)
        {
            ///
            string format = args[0].String.Replace("_", " ");
            var min = args[1].Int;
            var max = args[2].Int;
            var largeNumberString = args[3].Bool;

            ///
            int value = Random.Range(min, max);
            string outputStr;
            if (largeNumberString)
            {

                outputStr = string.Format(format, value.ToLargeNumberString());
            }
            else
            {
                outputStr = string.Format(format, value);
            }

            ///
            return miniConsolePlayer.ProcessLineAsLiteralString(outputStr);
        }

        private static float CmdSetWaitScale(CommandArg[] args, MiniConsolePlayer miniConsolePlayer)
        {
            ///
            if (miniConsolePlayer.disableTime)
            {
                return 0;
            }

            ///
            float scale = args[0].Float;

            ///
            miniConsolePlayer.waitScale = scale;

            ///
            return 0;
        }

        private static float CmdWaitForAnyKey(CommandArg[] args, MiniConsolePlayer miniConsolePlayer)
        {
            ///
            if (miniConsolePlayer.disableTime)
            {
                return 0;
            }

            ///            
            miniConsolePlayer.waitForAnyKeyFlag = true;

            ///
            return 0;
        }

        private static float CmdImport(CommandArg[] args, MiniConsolePlayer miniConsolePlayer)
        {
            ///
            if (args.Length < 1)
            {
                Debug.LogError("Import command requires at least one argument.");
                return 0;
            }

            ///
            string key = args[0].String.Trim();

            ///
            miniConsolePlayer.context.GetImportingLines(key, miniConsolePlayer.importedLinesTmp);
            miniConsolePlayer.importedLines.InsertRange(0, miniConsolePlayer.importedLinesTmp);

            ///
            return 0;
        }
        #endregion

#if UNITY_EDITOR
        [ContextMenu("Editor_FinishNow")]
        protected void Editor_FinishNow()
        {
            editor_FinishNowFlag = true;
        }
#endif
    }

}