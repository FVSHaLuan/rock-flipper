using FHC.Core.Architecture;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class UIInputActionBase : Selectable
{
    public static event System.Action OnDisabled;
    public static event System.Action OnEnabled;

    public event System.Action<InputAction.CallbackContext> OnActionStarted;
    public event System.Action<InputAction.CallbackContext> OnActionPerformed;
    public event System.Action<InputAction.CallbackContext> OnActionPerformedIgnoredStartedTime;
    public event System.Action<InputAction.CallbackContext> OnActionCanceled;
    public event ActionEventHandler OnActionDisrupted;
    public event System.Action OnPerformedUpdate;

    public delegate void ActionEventHandler(bool performed, float durationFromStarted, float durationFromPerformed);

    [Header("UIInputActionBase")]
    [SerializeField]
    private InputActionReference inputActionReference;

    [Space]
    [SerializeField]
    private bool ignoreStartOnEnable;
    [SerializeField]
    private bool ignoreUIScreenInteractability;
    [SerializeField]
    private bool hasNoStartEvent = false;

    [Space]
    [SerializeField]
    private bool useActionTimeOut = false;

    [Space]
    [SerializeField]
    private UnityEvent onActionStarted;
    [SerializeField]
    private UnityEvent onActionPerformed;
    [SerializeField]
    private UnityEvent onActionCanceled;
    [SerializeField]
    private UnityEvent onActionDisrupted;

    [Space]
    [SerializeField]
    private float minPerformedUpdateInterval = 1 / 6.0f;
    [SerializeField]
    private UnityEvent onPerformedUpdate;

    private UIScreen uiScreen;
    private float lastTimeInvokePerformedUpdate = -1;
    private int enabledFrameCount = -1;
    private bool unlisteningToInputActionFlag = false;

    private float ActionTimeOut => useActionTimeOut ? 0.49f : -1;

    public bool Performed { get; private set; } = false;
    public float StartedTime { get; private set; } = -1;
    public float PerformedTime { get; private set; } = -1;

    /// <summary>
    /// Set is not really stable. Only set when know for sure this is not handling any event.
    /// </summary>
    public InputActionReference InputActionReference
    {
        get => inputActionReference;
                
        set
        {
            ///
            if (StartedTime >= 0)
            {
                InvokeDisruptedEvents();
            }

            ///
            UnlistenToInputAction(true);

            ///
            inputActionReference = value;

            ///
            ListenToInputAction();
        }
    }

    private static bool disabled = false;

    private static BalancerWithObjects enabledBalancer = new BalancerWithObjects();

    public static bool Disabled
    {
        get => disabled;

        private set
        {
            ///
            if (disabled == value)
            {
                return;
            }

            ///
            disabled = value;

            ///
            if (disabled)
            {
                OnDisabled?.Invoke();
            }
            else
            {
                OnEnabled?.Invoke();
            }
        }
    }

    public new bool interactable
    {
        get
        {
            ///
            if (uiScreen != null && !uiScreen.Interactable && !ignoreUIScreenInteractability)
            {
                return false;
            }

            ///
            if (Disabled)
            {
                return false;
            }

            ///
            return base.IsInteractable();
        }

        set
        {
            ///
            if (base.interactable == value)
            {
                return;
            }

            ///
            base.interactable = value;

            ///
            if (StartedTime >= 0 && !value)
            {
                InvokeDisruptedEvents();
            }
        }
    }

    public bool IsEnabledAndInteractable
    {
        get
        {
            return interactable && enabled;
        }
    }

    static UIInputActionBase()
    {
        enabledBalancer.OnBalanced += EnabledBalancer_OnBalanced;
        enabledBalancer.OnOffBalanced += EnabledBalancer_OnOffBalanced;
    }

    private static void EnabledBalancer_OnOffBalanced()
    {
        Disabled = true;
    }

    private static void EnabledBalancer_OnBalanced()
    {
        Disabled = false;
    }

    public static void AddDisabledLock(object @object)
    {
        enabledBalancer.AddObject(@object);
    }

    public static void RemoveDisabledLock(object @object)
    {
        enabledBalancer.RemoveObject(@object);
    }

    protected void StartWith(UIScreen uiScreen)
    {
        ///
        this.uiScreen = uiScreen;

        ///
        if (uiScreen != null)
        {
            uiScreen.OnBecomeActive += UiScreen_OnBecomeActive;
            uiScreen.OnBecomeInactive += UIScreen_OnBecomeInactive;
            uiScreen.OnInteractabilityChanged += UiScreen_OnInteractabilityChanged;
        }
    }

    private void UiScreen_OnInteractabilityChanged()
    {
        ///
        if (ignoreUIScreenInteractability)
        {
            return;
        }

        ///
        if (!uiScreen.Interactable)
        {
            if (StartedTime >= 0)
            {
                InvokeDisruptedEvents();
            }
        }
    }

    private void UIScreen_OnBecomeInactive()
    {
        if (StartedTime >= 0)
        {
            InvokeDisruptedEvents();
        }
    }

    private void UiScreen_OnBecomeActive()
    {

    }

    protected override void OnEnable()
    {
        ///
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return;
        }
#endif

        ///
        enabledFrameCount = Time.frameCount;

        ///
        ResetActionState();

        ///
        ListenToInputAction();

        ///
        OnDisabled += UIInputActionBase_OnDisabled;
    }

    protected override void OnDisable()
    {
        ///
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return;
        }
#endif

        ///
        base.OnDisable();

        ///
        UnlistenToInputActionSafely();

        ///
        OnDisabled -= UIInputActionBase_OnDisabled;

        ///
        if (StartedTime >= 0)
        {
            InvokeDisruptedEvents();
        }
    }

    private void ListenToInputAction()
    {
        ///
        inputActionReference.action.started += Action_started;
        inputActionReference.action.performed += Action_performed;
        inputActionReference.action.canceled += Action_canceled;

        ///
        unlisteningToInputActionFlag = false;
    }

    private void UIInputActionBase_OnDisabled()
    {
        if (StartedTime >= 0)
        {
            InvokeDisruptedEvents();
        }
    }

    private void UnlistenToInputActionSafely()
    {
        ///
        unlisteningToInputActionFlag = true;

        ///
        Entry.Instance.executionHelper.ExecuteOnEndOfFrame(UnlistenToInputAction);
    }

    private void UnlistenToInputAction()
    {
        UnlistenToInputAction(false);
    }

    private void UnlistenToInputAction(bool isForced)
    {
        ///
        if (!unlisteningToInputActionFlag && !isForced)
        {
            return;
        }

        ///
        inputActionReference.action.started -= Action_started;
        inputActionReference.action.performed -= Action_performed;
        inputActionReference.action.canceled -= Action_canceled;

        ///
        unlisteningToInputActionFlag = false;
    }

    protected virtual void Update()
    {
        ///
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return;
        }
#endif

        ///
        if (!interactable)
        {
            ///
            if (StartedTime >= 0)
            {
                InvokeDisruptedEvents();
            }

            ///
            return;
        }

        ///
        if (Performed && StartedTime >= 0)
        {
            if ((Time.unscaledTime - lastTimeInvokePerformedUpdate) >= minPerformedUpdateInterval)
            {
                ///
                lastTimeInvokePerformedUpdate = Time.unscaledTime;

                ///
                onPerformedUpdate?.Invoke();
                OnPerformedUpdate?.Invoke();
            }
        }
    }

    protected void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            ///
            if (StartedTime >= 0)
            {
                InvokeDisruptedEvents();
            }
        }
    }

    private void Action_canceled(InputAction.CallbackContext obj)
    {
        // Debug.Log("Action_canceled");

        ///
        if (unlisteningToInputActionFlag)
        {
            return;
        }

        ///
        if (Disabled)
        {
            return;
        }

        ///
        if (!interactable)
        {
            return;
        }

        ///
        if (IsBlockedByUIScreen())
        {
            return;
        }

        ///
        if (StartedTime < 0)
        {
            return;
        }

        ///
        onActionCanceled?.Invoke();
        OnActionCanceled?.Invoke(obj);

        ///
        ResetActionState();
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        // Debug.Log("Action_performed");

        ///
        if (unlisteningToInputActionFlag)
        {
            return;
        }

        ///
        if (Disabled)
        {
            return;
        }

        ///
        if (!interactable)
        {
            return;
        }

        ///
        if (IsBlockedByUIScreen())
        {
            return;
        }

        OnActionPerformedIgnoredStartedTime?.Invoke(obj);

        ///
        if ((StartedTime < 0) && (!hasNoStartEvent))
        {
            return;
        }

        ///
        if (ActionTimeOut > 0 && !hasNoStartEvent && (Time.unscaledTime - StartedTime) >= ActionTimeOut)
        {
            ///
            InvokeDisruptedEvents();

            ///
            return;
        }

        ///
        PerformedTime = Time.unscaledTime;
        Performed = true;
        lastTimeInvokePerformedUpdate = -1;

        ///
        onActionPerformed?.Invoke();
        OnActionPerformed?.Invoke(obj);
    }

    private void Action_started(InputAction.CallbackContext obj)
    {
        // Debug.Log("Action_started");

        ///
        if (unlisteningToInputActionFlag)
        {
            return;
        }

        ///
        if (Disabled)
        {
            return;
        }

        ///
        if (ignoreStartOnEnable && (enabledFrameCount == Time.frameCount))
        {
            return;
        }

        ///
        if (!interactable)
        {
            return;
        }

        ///
        if (IsBlockedByUIScreen())
        {
            return;
        }

        ///
        StartedTime = Time.unscaledTime;

        ///
        onActionStarted?.Invoke();
        OnActionStarted?.Invoke(obj);
    }

    private void ResetActionState()
    {
        Performed = false;
        StartedTime = -1;
        PerformedTime = -1;
    }

    private void InvokeDisruptedEvents()
    {
        ///
        onActionDisrupted?.Invoke();
        OnActionDisrupted?.Invoke(Performed, Time.unscaledTime - StartedTime, Time.unscaledTime - PerformedTime);

        ///
        ResetActionState();
    }

    private bool IsBlockedByUIScreen()
    {
        ///
        if (uiScreen == null)
        {
            return false;
        }

        ///
        return !uiScreen.IsScreenActive || (!uiScreen.Interactable && !ignoreUIScreenInteractability);
    }

    public void ExecuteOnActionPerformedUnityEvent()
    {
        onActionPerformed?.Invoke();
    }

#if UNITY_EDITOR
    protected override void Reset()
    {
        ///
        base.Reset();

        ///
        navigation = new Navigation() { mode = Navigation.Mode.None };
    }
#endif
}
