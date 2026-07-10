using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BT.Run.Dev
{
    public class GameplayTimeScaleSlider : ExtendedMonoBehaviourRun, ITimeScaleControl
    {
        public event Action OnControlValueChanged;

        [SerializeField]
        private float minScale = 0.2f;
        [SerializeField]
        private float maxScale = 5f;

        [SerializeField]
        private Key hotKey1 = Key.Digit1;
        [SerializeField]
        private Key hotKey2 = Key.Digit2;
        [SerializeField]
        private Key hotKey3 = Key.Digit3;

        [Space]
        [SerializeField]
        private float presetScale1 = 2f;
        [SerializeField]
        private float presetScale2 = 5f;
        [SerializeField]
        private float presetScale3 = 10f;

        [Space]
        [SerializeField]
        private Slider slider;
        [SerializeField]
        private UnifiedText currentScaleText;

        public TimeScaleControlType ControlType => TimeScaleControlType.Multiply;

        public float ControlValue => Mathf.Lerp(minScale, maxScale, slider.normalizedValue);

        protected void OnDestroy()
        {
            entry.timeScaleManager.RemoveControl(this);
        }

        protected void Start()
        {
            ///
            slider.normalizedValue = Mathf.InverseLerp(minScale, maxScale, 1);

            ///
            entry.timeScaleManager.AddControl(this);

            ///
            slider.onValueChanged.AddListener(OnSliderValueChanged);
            RunEntry.runStateManager.OnCombatStarted += RunStateManager_OnCombatStarted;
        }

        private void RunStateManager_OnCombatStarted()
        {
            SetToOne();
        }

        private void OnSliderValueChanged(float arg0)
        {
            currentScaleText.Text = $"Time Scale: {ControlValue:0.00}";
            OnControlValueChanged?.Invoke();
        }

        public void SetToOne()
        {
            Set(1);
        }

        private void Set(float scale)
        {
            slider.normalizedValue = Mathf.InverseLerp(minScale, maxScale, scale);
        }

#if UNITY_EDITOR
        protected void Update()
        {
            UpdateHotKey();

            ///
            if (!entry.timeScaleManager.IsGameplayBeingPaused)
            {
                RunData.PlayTimeAdjustment += Time.unscaledDeltaTime * (ControlValue - 1);
            }
        }
#endif

        private void UpdateHotKey()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            ///
            if (keyboard[hotKey1].wasPressedThisFrame)
            {
                Set(presetScale1);
            }
            else if (keyboard[hotKey2].wasPressedThisFrame)
            {
                Set(presetScale2);
            }
            else if (keyboard[hotKey3].wasPressedThisFrame)
            {
                Set(presetScale3);
            }

            ///
            if (keyboard[hotKey1].wasReleasedThisFrame
                || keyboard[hotKey2].wasReleasedThisFrame
                || keyboard[hotKey3].wasReleasedThisFrame)
            {
                SetToOne();
            }
        }
    }
}