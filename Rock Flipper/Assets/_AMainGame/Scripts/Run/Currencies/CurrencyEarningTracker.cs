using UnityEngine;

namespace Agame.Run
{
    /// <summary>
    /// Tracks how much of a currency is earned and computes the earning speed (amount per second)
    /// over a sliding time window. Earnings are the positive net change of the currency value per frame
    /// (via <see cref="RunData.OnCurrencyValueModifiedThisFrame"/>), so spending doesn't lower the speed,
    /// but spending and earning in the same frame cancel each other out.
    /// The clock only advances while this component is enabled, following <see cref="timeScaleMode"/>
    /// (so with the default scaled time, pausing freezes the window instead of dragging the speed down).
    /// </summary>
    public class CurrencyEarningTracker : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private Currency currency = Currency.CASH;
        [SerializeField, Min(0.1f)]
        private float windowDuration = 10f;
        [SerializeField, Min(0.05f)]
        private float bucketDuration = 0.5f;
        [SerializeField, Min(0f)]
        private float earningSpeedRefreshInterval = 0.25f;
        [SerializeField]
        private TimeScaleMode timeScaleMode = TimeScaleMode.ScaledTime;

        private double[] buckets;
        private long currentBucketIndex;
        private float trackedTime;
        private double totalEarned;
        private double lastCurrencyValue;
        private double earningSpeed;
        private float earningSpeedRefreshTime;

        public Currency Currency => currency;

        /// <summary>Total amount earned since tracking started (or was last reset).</summary>
        public double TotalEarned => totalEarned;

        /// <summary>Time tracked since tracking started (or was last reset), in seconds.</summary>
        public float TrackedTime => trackedTime;

        /// <summary>
        /// Amount earned per second over the last <see cref="windowDuration"/> seconds.
        /// Cached, refreshed every <see cref="earningSpeedRefreshInterval"/> seconds.
        /// </summary>
        public double EarningSpeed => earningSpeed;

        /// <summary>Amount earned per second since tracking started (or was last reset).</summary>
        public double AverageEarningSpeed => trackedTime > 0 ? totalEarned / trackedTime : 0;

        private double[] Buckets
        {
            get
            {
                if (buckets == null)
                {
                    // One extra bucket so the full buckets always cover the whole window,
                    // with the partially filled current bucket on top
                    buckets = new double[Mathf.CeilToInt(windowDuration / bucketDuration) + 1];
                }

                ///
                return buckets;
            }
        }

        protected void OnEnable()
        {
            ///
            lastCurrencyValue = RunData.GetCurrencyValue(currency);

            ///
            RunData.OnCurrencyValueModifiedThisFrame += RunData_OnCurrencyValueModifiedThisFrame;
            StateManager.OnBeforeCombatFromPrestige += StateManager_OnBeforeCombatFromPrestige;
        }

        protected void OnDisable()
        {
            RunData.OnCurrencyValueModifiedThisFrame -= RunData_OnCurrencyValueModifiedThisFrame;
            StateManager.OnBeforeCombatFromPrestige -= StateManager_OnBeforeCombatFromPrestige;
        }

        protected void Update()
        {
            trackedTime += GetDeltaTime(timeScaleMode);
            AdvanceBuckets();

            ///
            if (trackedTime >= earningSpeedRefreshTime)
            {
                earningSpeedRefreshTime = trackedTime + earningSpeedRefreshInterval;
                earningSpeed = ComputeEarningSpeed();
            }
        }

        public void ResetTracking()
        {
            System.Array.Clear(Buckets, 0, Buckets.Length);
            currentBucketIndex = 0;
            trackedTime = 0;
            totalEarned = 0;
            lastCurrencyValue = RunData.GetCurrencyValue(currency);
            earningSpeed = 0;
            earningSpeedRefreshTime = 0;
        }

        private void StateManager_OnBeforeCombatFromPrestige()
        {
            ResetTracking();
        }

        private void RunData_OnCurrencyValueModifiedThisFrame(Currency modifiedCurrency)
        {
            ///
            if (modifiedCurrency != currency)
            {
                return;
            }

            ///
            var currencyValue = RunData.GetCurrencyValue(currency);
            var earned = currencyValue - lastCurrencyValue;
            lastCurrencyValue = currencyValue;

            ///
            if (earned <= 0)
            {
                return;
            }

            ///
            totalEarned += earned;
            Buckets[currentBucketIndex % Buckets.Length] += earned;
        }

        private double ComputeEarningSpeed()
        {
            ///
            var currentBucketElapsed = trackedTime - currentBucketIndex * bucketDuration;
            var span = Mathf.Min(trackedTime, (Buckets.Length - 1) * bucketDuration + currentBucketElapsed);
            if (span <= 0)
            {
                return 0;
            }

            ///
            double sum = 0;
            foreach (var item in Buckets)
            {
                sum += item;
            }
            return sum / span;
        }

        private void AdvanceBuckets()
        {
            var targetBucketIndex = (long)(trackedTime / bucketDuration);
            var steps = targetBucketIndex - currentBucketIndex;
            if (steps <= 0)
            {
                return;
            }

            // Clear the buckets being entered; after a long gap the whole ring is cleared once
            var clearCount = System.Math.Min(steps, Buckets.Length);
            for (long i = 1; i <= clearCount; i++)
            {
                Buckets[(currentBucketIndex + i) % Buckets.Length] = 0;
            }

            ///
            currentBucketIndex = targetBucketIndex;
        }
    }

}
