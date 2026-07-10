using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RunningTextEffect : MonoBehaviour
{
    [SerializeField]
    private UnifiedText unifiedText;
    [SerializeField]
    private WordDictionary wordDictionary;
    [SerializeField]
    private int maxLength;
    [SerializeField]
    private float wordPerSecond = 24;
    [SerializeField]
    private bool spaceBetweenWords = false;
    [SerializeField]
    private bool alwaysTrimOutput = false;
    [SerializeField]
    private string initialString = "";
    [SerializeField]
    private TimeScaleMode timeScaleMode = TimeScaleMode.GameplayUnscaledTime;

    private StringBuilder stringBuilder = new StringBuilder();
    private float timeAccount;

    protected void OnEnable()
    {
        ///
        timeAccount = 0;

        ///
        stringBuilder.Clear();
        stringBuilder.Append(initialString);
    }

    protected void Update()
    {
        ///
        timeAccount += Entry.Instance.timeScaleManager.GetDeltaTime(timeScaleMode);

        ///
        int removingWordCount = Mathf.FloorToInt(timeAccount * wordPerSecond);

        ///
        if (removingWordCount <= 0)
        {
            return;
        }

        ///
        timeAccount -= removingWordCount * 1.0f / wordPerSecond;

        ///
        RemoveLastCharacters(removingWordCount);
        FillWords();

        ///
        if (!alwaysTrimOutput)
        {
            unifiedText.Text = stringBuilder.ToString(); 
        }
        else
        {
            unifiedText.Text = stringBuilder.ToString().Trim();
        }
    }

    private void RemoveLastCharacters(int count)
    {
        ///
        if (count >= stringBuilder.Length)
        {
            stringBuilder.Clear();
        }
        else
        {
            stringBuilder.Remove(stringBuilder.Length - count, count);
        }
    }

    private void FillWords()
    {
        while (true)
        {
            ///
            var randomWord = wordDictionary.GetRandomWord();

            ///
            bool shouldInsertSpace = spaceBetweenWords && stringBuilder.Length > 0;

            ///
            int insertingLength = randomWord.Length;
            if (shouldInsertSpace)
            {
                insertingLength++;
            }

            ///
            if ((insertingLength + stringBuilder.Length) > maxLength)
            {
                break;
            }

            ///
            if (shouldInsertSpace)
            {
                stringBuilder.Insert(0, " ");
            }

            ///
            stringBuilder.Insert(0, randomWord);
        }
    }
}
