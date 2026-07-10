using FHC.Core.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class CompleteInputBlocker : ExtendedMonoBehaviour
{
    [SerializeField]
    private GameObject inputBlockingCanvas;

    private BalancerWithObjects unblockedBalancer = new BalancerWithObjects();

    public bool IsBlocking => !unblockedBalancer.IsBalanced;

    protected override bool Init()
    {
        ///
        unblockedBalancer.OnOffBalanced += UnblockedBalancer_OnOffBalanced;
        unblockedBalancer.OnBalanced += UnblockedBalancer_OnBalanced;

        ///
        return base.Init();
    }

    private void UnblockedBalancer_OnBalanced()
    {
        Unblock();
    }

    private void UnblockedBalancer_OnOffBalanced()
    {
        Block();
    }

    private void Block()
    {
        UIInputActionBase.AddDisabledLock(this);
        inputBlockingCanvas.SetActive(true);
        entry.inputManager.InputActionAsset.Disable();

        ///
        (EventSystem.current.currentInputModule as InputSystemUIInputModule)?.actionsAsset.Disable();
    }

    private void Unblock()
    {
        UIInputActionBase.RemoveDisabledLock(this);
        inputBlockingCanvas.SetActive(false);
        entry.inputManager.InputActionAsset.Enable();

        ///
        (EventSystem.current.currentInputModule as InputSystemUIInputModule)?.actionsAsset.Enable();
    }

    [ContextMenu("AddBlockLock")]
    public void AddBlockLock(object @object)
    {
        ///
        TryInit();

        ///
        unblockedBalancer.AddObject(@object);
    }

    [ContextMenu("RemoveBlockLock")]
    public void RemoveBlockLock(object @object)
    {
        ///
        TryInit();

        ///
        unblockedBalancer.RemoveObject(@object);
    }
}
