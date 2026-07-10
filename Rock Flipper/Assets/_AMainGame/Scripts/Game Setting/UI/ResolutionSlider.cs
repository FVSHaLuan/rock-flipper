using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using BT.FeatureBranching;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider)), DisallowMultipleComponent]
public class ResolutionSlider : MonoBehaviour
{
    [Space]
    [SerializeField]
    private List<float> scales = new List<float>();

    [Space]
    [SerializeField]
    private bool changeImmediately;
    [SerializeField]
    private float changeDelay = 1;

    [Space]
    [SerializeField]
    private UnifiedText currentResolutionText;

    private Slider slider;

    private List<Resolution> resolutions = new List<Resolution>();
    private int currentResolutionIndex;
    private int savedCurrentResolutionIndex;

    private struct Resolution
    {
        public int x;
        public int y;
        public bool isRecommended;

        public Resolution(int x, int y, bool isRecommended)
        {
            this.x = x;
            this.y = y;
            this.isRecommended = isRecommended;
        }

        public static int CompareX(Resolution r1, Resolution r2)
        {
            return r1.x - r2.x;
        }
    }

    protected void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void OnEnable()
    {
        ///
        if (PlatformBranchInfo.Current != PlatformBranch.PC
            && PlatformBranchInfo.Current != PlatformBranch.Linux
            && PlatformBranchInfo.Current != PlatformBranch.Mac)
        {
            return;
        }

        ///
        slider.enabled = true;

        ///
        ListResolutions();
        savedCurrentResolutionIndex = currentResolutionIndex;
        slider.wholeNumbers = true;
        slider.minValue = 0;
        slider.maxValue = resolutions.Count - 1;
        slider.value = currentResolutionIndex;

        ///
        ViewCurrentResolutionText();

        ///
        slider.onValueChanged.AddListener(OnSliderChangedValue);
    }

    public void OnDisable()
    {
        slider.onValueChanged.RemoveListener(OnSliderChangedValue);
    }

    private void OnSliderChangedValue(float value)
    {
        currentResolutionIndex = Mathf.RoundToInt(value);
        ViewCurrentResolutionText();

        ///
        if (changeImmediately)
        {
            SetCurrentResolution();
            StopAllCoroutines();
            StartCoroutine(DelayNewChange(changeDelay));
        }
    }

    /// <summary>
    /// Fill the resolutions list and find currentResolutionIndex
    /// </summary>
    private void ListResolutions()
    {
        ///
        resolutions.Clear();

        ///
        var nativeX = Display.main.systemWidth;
        var nativeY = Display.main.systemHeight;

        ///
        for (int i = 0; i < scales.Count; i++)
        {
            ///
            var scale = scales[i];

            ///
            var x = Mathf.RoundToInt(nativeX * scale);
            var y = Mathf.RoundToInt(nativeY * scale);

            ///
            GetWidthRange(y, out var minWidth, out var maxWidth);
            x = Mathf.Clamp(x, (int)minWidth, (int)maxWidth);

            ///
            var res = new Resolution()
            {
                x = x,
                y = y,
                isRecommended = i == 0,
            };

            ///
            resolutions.Add(res);
        }

        ///
        resolutions.Sort(Resolution.CompareX);

        // Find current res
        currentResolutionIndex = -1;
        for (int i = 0; i < resolutions.Count; i++)
        {
            ///
            var res = resolutions[i];

            ///
            if (res.x == Screen.width
                && res.y == Screen.height)
            {
                currentResolutionIndex = i;

                ///
                break;
            }
        }

        // Insert current res
        if (currentResolutionIndex < 0)
        {
            for (int i = 0; i < resolutions.Count; i++)
            {
                ///
                var res = resolutions[i];

                ///
                int insertIndex = -1;

                ///
                if (i == 0 && res.x >= Screen.width)
                {
                    insertIndex = 0;
                }
                else if (i == resolutions.Count - 1 && res.x <= Screen.width)
                {
                    insertIndex = resolutions.Count;
                }
                else if (i < resolutions.Count - 1
                    && res.x <= Screen.width
                    && resolutions[i + 1].x > Screen.width)
                {
                    insertIndex = i + 1;
                }

                ///
                if (insertIndex < 0)
                {
                    continue;
                }

                ///
                resolutions.Insert(insertIndex, new Resolution(Screen.width, Screen.height, false));
                currentResolutionIndex = insertIndex;

                ///
                break;
            }
        }
    }

    private void ViewCurrentResolutionText()
    {
        var resolution = resolutions[currentResolutionIndex];
        currentResolutionText.Text = string.Format("{0} x {1}", resolution.x, resolution.y);
        if (resolution.isRecommended)
        {
            currentResolutionText.Text += "\r\n(" + ScriptLocalization.Common.Recommended + ")";
        }
    }

    public void SetCurrentResolution()
    {
        ///
        if (savedCurrentResolutionIndex == currentResolutionIndex
            || resolutions == null
            || currentResolutionIndex >= resolutions.Count
            || currentResolutionIndex < 0)
        {
            return;
        }

        ///
        var resolution = resolutions[currentResolutionIndex];

        ///
        Screen.SetResolution(resolution.x, resolution.y, Screen.fullScreenMode);
    }

    private void GetWidthRange(float height, out float minWidth, out float maxWidth)
    {
        minWidth = height * ScreenSizeConfig.MinWidthRatio;
        maxWidth = height * ScreenSizeConfig.MaxWidthRatio;
    }

    private bool IsWidthInRange(float x, float y)
    {
        GetWidthRange(y, out float minWidth, out float maxWidth);
        return x >= minWidth && x <= maxWidth;
    }

    private IEnumerator DelayNewChange(float delay)
    {
        ///
        yield return null;

        slider.enabled = false;
        Entry.Instance.completeInputBlocker.AddBlockLock(this);

        ///
        yield return new WaitForSecondsRealtime(delay);

        ///
        Entry.Instance.completeInputBlocker.RemoveBlockLock(this);
        slider.enabled = true;

        ///
        Entry.Instance.uiSelectedEventManager.SetCurrentSelectedGameObject(slider.gameObject);
    }
}
