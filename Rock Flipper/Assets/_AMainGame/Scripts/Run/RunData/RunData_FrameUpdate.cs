using Agame.Run;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Agame
{
    public partial class RunData
    {
        [field: System.NonSerialized]
        public event Action<Currency> OnCurrencyValueModifiedThisFrame;
        /// <summary>
        /// Invoked once per frame for each currency added to this frame (via <see cref="AddCurrency(Currency, double)"/>),
        /// with the total amount added. Spending is not counted.
        /// </summary>
        [field: System.NonSerialized]
        public event Action<Currency, double> OnCurrencyAddedThisFrame;

        [System.NonSerialized]
        private int lastFrameUpdateLate = -1;
        [System.NonSerialized]
        private HashSet<Currency> modifiedCurrenciesThisFrame = new HashSet<Currency>();
        [System.NonSerialized]
        private HashSet<Currency> modifiedCurrenciesThisFrameTmp = new HashSet<Currency>();
        [System.NonSerialized]
        private Dictionary<Currency, double> addedCurrenciesThisFrame = new Dictionary<Currency, double>();
        [System.NonSerialized]
        private Dictionary<Currency, double> addedCurrenciesThisFrameTmp = new Dictionary<Currency, double>();

        private HashSet<Currency> ModifiedCurrenciesThisFrame
        {
            get
            {
                if (modifiedCurrenciesThisFrame == null)
                {
                    modifiedCurrenciesThisFrame = new HashSet<Currency>();
                }

                ///
                return modifiedCurrenciesThisFrame;
            }
        }
        private HashSet<Currency> ModifiedCurrenciesThisFrameTmp
        {
            get
            {
                if (modifiedCurrenciesThisFrameTmp == null)
                {
                    modifiedCurrenciesThisFrameTmp = new HashSet<Currency>();
                }

                ///
                return modifiedCurrenciesThisFrameTmp;
            }
        }
        private Dictionary<Currency, double> AddedCurrenciesThisFrame
        {
            get
            {
                if (addedCurrenciesThisFrame == null)
                {
                    addedCurrenciesThisFrame = new Dictionary<Currency, double>();
                }

                ///
                return addedCurrenciesThisFrame;
            }
        }
        private Dictionary<Currency, double> AddedCurrenciesThisFrameTmp
        {
            get
            {
                if (addedCurrenciesThisFrameTmp == null)
                {
                    addedCurrenciesThisFrameTmp = new Dictionary<Currency, double>();
                }

                ///
                return addedCurrenciesThisFrameTmp;
            }
        }

        public void FrameUpdateLate()
        {
            if (lastFrameUpdateLate >= Time.frameCount)
                throw new System.Exception("RunData.FrameUpdateLate called more than once per frame");

            ///
            lastFrameUpdateLate = Time.frameCount;

            // Modified currencies this frame events
            EvokeEventsForModifiedCurrenciesThisFrame();

            // Added currencies this frame events
            EvokeEventsForAddedCurrenciesThisFrame();
        }

        private void RegisterCurrencyAddedThisFrame(Currency currency, double amount)
        {
            AddedCurrenciesThisFrame.TryGetValue(currency, out var addedAmount);
            AddedCurrenciesThisFrame[currency] = addedAmount + amount;
        }

        private void EvokeEventsForModifiedCurrenciesThisFrame()
        {
            ModifiedCurrenciesThisFrameTmp.Clear();
            ModifiedCurrenciesThisFrameTmp.UnionWith(ModifiedCurrenciesThisFrame);
            ModifiedCurrenciesThisFrame.Clear();

            ///
            foreach (var item in ModifiedCurrenciesThisFrameTmp)
            {
                OnCurrencyValueModifiedThisFrame?.Invoke(item);
            }
        }

        private void EvokeEventsForAddedCurrenciesThisFrame()
        {
            AddedCurrenciesThisFrameTmp.Clear();
            foreach (var item in AddedCurrenciesThisFrame)
            {
                AddedCurrenciesThisFrameTmp.Add(item.Key, item.Value);
            }
            AddedCurrenciesThisFrame.Clear();

            ///
            foreach (var item in AddedCurrenciesThisFrameTmp)
            {
                OnCurrencyAddedThisFrame?.Invoke(item.Key, item.Value);
            }
        }
    }

}
