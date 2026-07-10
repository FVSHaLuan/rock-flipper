using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent()]
public abstract class ContextButtonPrompt : ExtendedMonoBehaviour
{
    public event System.Action OnTextChanged;
    public event System.Action OnInteractabilityChanged;
    public event System.Action OnAdditionalIconChanged;

    [Header("Icon")]
    [SerializeField]
    private InputActionReference inputAction;
    [SerializeField]
    private InputActionReference additionalInputAction;
    [SerializeField]
    private bool enableAdditionalIcon = true;

    [Header("Text")]
    [SerializeField, Multiline]
    private string text;

    [Header("Oder Id")]
    [SerializeField]
    private int overrideOrderId = -1;

    [Space]
    [SerializeField]
    private Color textColor = Color.white;

    private UIScreen uiScreen;
    private bool interactable = true;
    private bool additionalActionInteractable = true;

    public UIScreen UIScreenParent
    {
        get
        {
            ///
            TryInit();

            ///
            return uiScreen;
        }
    }

    public virtual UIInputActionBase UIInputActionBase { get; protected set; }
    public bool EnableAdditionalIcon
    {
        get => enableAdditionalIcon;
        set
        {
            ///
            enableAdditionalIcon = value;

            ///
            OnAdditionalIconChanged?.Invoke();
        }
    }

    public InputActionReference InputAction
    {
        get => inputAction;
        protected set => inputAction = value;
    }
    public InputActionReference AdditionalInputAction => EnableAdditionalIcon ? additionalInputAction : null;

    public bool Interactable
    {
        get => interactable;

        protected set
        {
            ///
            if (interactable == value)
            {
                return;
            }

            ///
            interactable = value;

            ///
            OnInteractabilityChanged?.Invoke();
        }
    }

    public bool AdditionalIconInteractable
    {
        get => additionalActionInteractable;

        protected set
        {
            ///
            if (additionalActionInteractable == value)
            {
                return;
            }

            ///
            additionalActionInteractable = value;

            ///
            OnInteractabilityChanged?.Invoke();
        }
    }

    public string Text
    {
        get
        {
            return text;
        }

        set
        {
            text = value;
            OnTextChanged?.Invoke();
        }
    }
    public Color TextColor => textColor;
    public int OrderId
    {
        get
        {
            var order = entry.buttonPromptManager.GetPromptSprites(inputAction.action).activeContextOrderId;
            return overrideOrderId >= 0 ? overrideOrderId : order;
        }
    }

    public bool IsInTheManager { get; private set; }

    protected override bool Init()
    {
        ///
        uiScreen = GetComponentInParent<UIScreen>(true);

        ///
        return base.Init();
    }

    protected bool AddToTheManager()
    {
        ///
        IsInTheManager = true;

        ///
        return entry.activeContextButtonPromptManager.AddPrompt(this);
    }

    protected bool RemoveFromTheManager()
    {
        ///
        IsInTheManager = false;

        ///
        return entry.activeContextButtonPromptManager.RemovePrompt(this);
    }
}
