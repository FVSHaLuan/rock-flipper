using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalBatch : MonoBehaviour
{
    [SerializeField]
    private List<string> commands;

    public void RunDelay(float delay)
    {
        this.InvokeDelay(Run, delay);
    }

    [ContextMenu("Run")]
    public void Run()
    {
        ///
        if (commands == null || commands.Count == 0)
        {
            return;
        }

        ///
        if (!Entry.Instance.gameSettingObject.Data.enabledTerminal && !Application.isEditor)
        {
            return;
        }

        ///
        Debug.LogWarning("Start executing a batch of terminal commands...");

        ///
        for (int i = 0; i < commands.Count; i++)
        {
            CommandTerminal.Terminal.Shell.RunCommand(commands[i].Trim());
        }
    }
}
