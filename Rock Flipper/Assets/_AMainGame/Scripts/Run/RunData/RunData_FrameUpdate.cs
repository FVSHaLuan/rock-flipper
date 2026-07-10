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

        [System.NonSerialized]
        private int lastFrameUpdateLate = -1;
        [System.NonSerialized]
        private HashSet<Currency> modifiedCurrenciesThisFrame = new HashSet<Currency>();
        [System.NonSerialized]
        private HashSet<Currency> modifiedCurrenciesThisFrameTmp = new HashSet<Currency>();

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

        public void FrameUpdateLate()
        {
            if (lastFrameUpdateLate >= Time.frameCount)
                throw new System.Exception("RunData.FrameUpdateLate called more than once per frame");

            ///
            lastFrameUpdateLate = Time.frameCount;

            // Modified currencies this frame events
            EvokeEventsForModifiedCurrenciesThisFrame();
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
    }

}