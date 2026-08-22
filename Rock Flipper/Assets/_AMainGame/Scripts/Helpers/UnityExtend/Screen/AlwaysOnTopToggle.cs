using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class AlwaysOnTopToggle : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private Sprite onSprite;
    [SerializeField]
    private Sprite offSprite;

#if UNITY_STANDALONE_WIN 
    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    static extern bool SetWindowPos(
        IntPtr hWnd,
        IntPtr hWndInsertAfter,
        int X, int Y, int cx, int cy,
        uint uFlags
    );

    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

    const uint SWP_NOMOVE = 0x0002;
    const uint SWP_NOSIZE = 0x0001;
    const uint SWP_SHOWWINDOW = 0x0040;

    IntPtr windowHandle;
#endif

    private bool isTopMost = false;

    protected void OnDestroy()
    {
        SetTopMost(false);

        ///
        ScreenSizeChangeDetector.DirectInstance.OnFullScreenModeChanged -= Instance_OnFullScreenModeChanged;
    }

    protected void Start()
    {
#if UNITY_STANDALONE_WIN 
        windowHandle = GetActiveWindow();
#endif

        ///
        UpdateInteractability();

        ///
        ScreenSizeChangeDetector.Instance.OnFullScreenModeChanged += Instance_OnFullScreenModeChanged;
    }

    private void Instance_OnFullScreenModeChanged()
    {
        ///
        UpdateInteractability();
        SetTopMost(false);
    }

    public void Toggle()
    {
        SetTopMost(!isTopMost);
    }

    private void SetTopMost(bool isTopMost)
    {
        this.isTopMost = isTopMost;

#if UNITY_STANDALONE_WIN 
        SetWindowPos(
            windowHandle,
            isTopMost ? HWND_TOPMOST : HWND_NOTOPMOST,
            0, 0, 0, 0,
            SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW
        );
#endif
        ///
        iconImage.sprite = isTopMost ? offSprite : onSprite;

        ///
        Debug.Log("Always On Top: " + isTopMost);
    }

    private void UpdateInteractability()
    {
        button.interactable = FullscreenToggle.IsWindowedMode();
    }
}
