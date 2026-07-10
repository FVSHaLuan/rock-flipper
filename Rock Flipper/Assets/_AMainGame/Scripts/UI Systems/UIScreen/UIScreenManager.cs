using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenManager : ExtendedMonoBehaviour
{
    public event System.Action<UIScreen> OnUIScreenPoppedFromStack;

    private List<UIScreen> screenStack = new List<UIScreen>();
    private List<QueuedScreen> queuedScreens = new List<QueuedScreen>();

    public UIScreen WaitingToBeActive { get; private set; }

    private bool isInPutOperation;
    private bool isInPopOperation;

    private enum Operation
    {
        Put,
        Pop
    }

    [System.Serializable]
    private struct QueuedScreen
    {
        public UIScreen screen;
        public Operation operation;
    }

    public void PutToStack(UIScreen uiScreen)
    {
        ///
        if (isInPutOperation || isInPopOperation)
        {
            ///
            queuedScreens.Add(new QueuedScreen()
            {
                operation = Operation.Put,
                screen = uiScreen
            });

            ///
            return;
        }

        ///
        DoPutOperation(uiScreen);

        ///
        ProcessQueue();
    }

    private void DoPutOperation(UIScreen uiScreen)
    {
        ///
        if (uiScreen.IsScreenActive)
        {
            return;
        }

        ///
        isInPutOperation = true;

        ///
        WaitingToBeActive = uiScreen;

        ///
        screenStack.Remove(uiScreen);

        ///
        if (screenStack.Count > 0)
        {
            screenStack[screenStack.Count - 1].HandleBecomeInactive(uiScreen.HidePreviousPopupContent);
        }

        ///        
        screenStack.Add(uiScreen);

        ///
        if (!uiScreen.IsScreenActive)
        {
            uiScreen.HandleBecomeActive();
        }

        ///
        WaitingToBeActive = null;

        ///
        isInPutOperation = false;
    }

    public void PopFromStack(UIScreen uiScreen)
    {
        ///
        if (isInPutOperation || isInPopOperation)
        {
            ///
            queuedScreens.Add(new QueuedScreen()
            {
                operation = Operation.Pop,
                screen = uiScreen
            });

            ///
            return;
        }

        ///
        DoPopOperation(uiScreen);

        ///
        ProcessQueue();
    }

    private void DoPopOperation(UIScreen uiScreen)
    {
        ///
        isInPopOperation = true;

        ///
        bool wasLastOnStack = screenStack[screenStack.Count - 1] == uiScreen;

        ///
        if (screenStack.Remove(uiScreen))
        {
            if (uiScreen.IsScreenActive)
            {
                uiScreen.HandleBecomeInactive(true);
            }

            ///
            if (wasLastOnStack && screenStack.Count > 0)
            {
                screenStack[screenStack.Count - 1].HandleBecomeActive();
            }
        }

        ///
        isInPopOperation = false;

        ///
        OnUIScreenPoppedFromStack?.Invoke(uiScreen);
    }

    private void ProcessQueue()
    {
        ///
        if (queuedScreens.Count == 0)
        {
            return;
        }

        ///
        var queuedScreen = queuedScreens[0];
        queuedScreens.RemoveAt(0);

        ///
        ProcessQueuedScreen(queuedScreen);
    }

    private void ProcessQueuedScreen(QueuedScreen queuedScreen)
    {
        if (queuedScreen.operation == Operation.Put)
        {
            PutToStack(queuedScreen.screen);
        }
        else
        {
            PopFromStack(queuedScreen.screen);
        }
    }
}
