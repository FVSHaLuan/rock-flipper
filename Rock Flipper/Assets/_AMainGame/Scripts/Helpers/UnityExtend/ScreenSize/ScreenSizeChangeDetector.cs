using UnityEngine;

public class ScreenSizeChangeDetector : MonoBehaviour
{
    public event System.Action OnScreenSizeChanged;
    public event System.Action OnFullScreenModeChanged;

    private static ScreenSizeChangeDetector instance;
    public static ScreenSizeChangeDetector DirectInstance => instance;
    public static ScreenSizeChangeDetector Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindOrCreateInstance();
            }

            ///
            return instance;
        }
    }

    private int lastWidth;
    private int lastHeight;
    private FullScreenMode lastFullScreenMode;

    private static ScreenSizeChangeDetector FindOrCreateInstance()
    {
        ///
        var c = GameObject.FindAnyObjectByType<ScreenSizeChangeDetector>();
        if (c != null)
        {
            return c;
        }

        ///
        Debug.Log("Spawn new ScreenSizeChangeDetector object");
        GameObject obj = new GameObject("ScreenSizeChangeDetector");
        DontDestroyOnLoad(obj);
        return obj.AddComponent<ScreenSizeChangeDetector>();
    }

    protected void Awake()
    {
        ///
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        ///
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        ///
        lastWidth = Screen.width;
        lastHeight = Screen.height;
        lastFullScreenMode = Screen.fullScreenMode;
    }

    protected void Update()
    {
        ///
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            lastWidth = Screen.width;
            lastHeight = Screen.height;
            OnScreenSizeChanged?.Invoke();
        }

        ///
        if (Screen.fullScreenMode != lastFullScreenMode)
        {
            lastFullScreenMode = Screen.fullScreenMode;
            OnFullScreenModeChanged?.Invoke();
        }
    }
}
