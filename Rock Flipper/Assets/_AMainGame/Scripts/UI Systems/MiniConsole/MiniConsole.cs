using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BSB.UISystems
{
    public class MiniConsole : ExtendedMonoBehaviour
    {
        /// <summary>
        /// perform cleaning unused queue items when queue list count hits this threshold
        /// </summary>
        private const int QueueItemCleanThreshold = 1000;

        [SerializeField]
        private MiniConsoleItem itemPrototype;

        [Space]
        [SerializeField]
        private int maxActiveCount = 2;
        [SerializeField]
        private bool newItemIsFirstSibling = false;
        [SerializeField]
        private bool removeFirstActiveItemImmediately;
        [SerializeField]
        private float maxItemsPerSecond = 60;
        [SerializeField]
        private TimeScaleMode timeScaleMode = TimeScaleMode.GameplayUnscaledTime;

        [Space]
        [SerializeField]
        private GameAudioPlayer itemSfx;

        [Space]
        [SerializeField]
        private int maxEffectiveQueuedItemCount = 50;

        [Header("Test")]
        [SerializeField]
        private int testItemCount = 1;

        private List<MiniConsoleItem> activeItems = new List<MiniConsoleItem>();
        private List<MiniConsoleItemData> queuedItemsData = new List<MiniConsoleItemData>();

        private bool shouldPlayItemSfxThisFrame;
        private float timePassedSinceLastItem = 0;
        private int effectiveQueueStartIndex = 0;

        protected override bool Init()
        {
            ///
            itemPrototype.gameObject.SetActive(false);

            ///
            return base.Init();
        }

        public void PushItem(MiniConsoleItemData data)
        {
            ///
            TryInit();

            ///
            if (queuedItemsData.Count == 0)
            {
                timePassedSinceLastItem = 1.0f / maxItemsPerSecond;
            }

            ///
            queuedItemsData.Add(data);

            ///
            UpdateEffectiveQueueStartIndex();

            ///
            if (queuedItemsData.Count >= QueueItemCleanThreshold)
            {
                CleanUnusedQueueItems();
            }
        }

        public void PushItem(string text)
        {
            PushItem(new MiniConsoleItemData() { text = text });
        }

        [ContextMenu("Clear"), PlayModeOnly]
        public void Clear()
        {
            ///
            TryInit();

            ///
            queuedItemsData.Clear();
            effectiveQueueStartIndex = 0;

            ///
            foreach (var item in activeItems)
            {
                item.TryReturnToPoolAndDeactivate();
            }

            ///
            activeItems.Clear();
        }

        protected void Update()
        {
            shouldPlayItemSfxThisFrame = false;

            ///
            RemoveInactiveItems();

            ///
            ShowItemsFromQueue();

            ///
            if (shouldPlayItemSfxThisFrame && itemSfx != null)
            {
                itemSfx.Play();
            }
        }

        private void RemoveInactiveItems()
        {
            for (int i = activeItems.Count - 1; i >= 0; i--)
            {
                if (activeItems[i].IsInPool)
                {
                    activeItems.RemoveAt(i);
                }
            }
        }

        private void ShowItemsFromQueue()
        {
            ///
            var timePerItem = 1.0f / maxItemsPerSecond;
            var savedActiveItems = activeItems.Count;

            ///
            while ((activeItems.Count < maxActiveCount || (savedActiveItems > 0 && removeFirstActiveItemImmediately))
                && GetEffectiveQueueCount() > 0
                && timePassedSinceLastItem > timePerItem)
            {
                ///
                if (activeItems.Count >= maxActiveCount)
                {
                    activeItems[0].TryReturnToPoolAndDeactivate();
                    activeItems.RemoveAt(0);
                    savedActiveItems--;
                }

                ///
                ShowFirstItemInQueue();

                ///
                timePassedSinceLastItem -= timePerItem;
            }

            ///
            if (GetEffectiveQueueCount() > 0)
            {
                timePassedSinceLastItem += GetDeltaTime(timeScaleMode);
            }


        }

        private int GetEffectiveQueueCount()
        {
            return queuedItemsData.Count - effectiveQueueStartIndex;
        }

        private void ShowFirstItemInQueue()
        {
            ///
            var data = queuedItemsData[effectiveQueueStartIndex];
            queuedItemsData.RemoveAt(effectiveQueueStartIndex);

            ///
            var item = CurrentSceneGeneralPool.TakeInstance(itemPrototype, this);
            item.transform.SetParent(itemPrototype.transform.parent, false);
            item.transform.localScale = Vector3.one;
            if (newItemIsFirstSibling)
            {
                item.transform.SetAsFirstSibling();
            }
            else
            {
                item.transform.SetAsLastSibling();
            }
            item.SetData(data);
            item.gameObject.SetActive(true);

            ///
            activeItems.Add(item);

            ///
            if (data.icon != null || !string.IsNullOrWhiteSpace(data.text))
            {
                shouldPlayItemSfxThisFrame = true;
            }
        }

        private void UpdateEffectiveQueueStartIndex()
        {
            if (GetEffectiveQueueCount() > maxEffectiveQueuedItemCount)
            {
                effectiveQueueStartIndex = queuedItemsData.Count - maxEffectiveQueuedItemCount;
            }
        }

        private void CleanUnusedQueueItems()
        {
            ///
            queuedItemsData.RemoveRange(0, effectiveQueueStartIndex);

            ///
            effectiveQueueStartIndex = 0;
        }

#if UNITY_EDITOR
        [ContextMenu("Editor_AddTestItem"), PlayModeOnly]
        private void Editor_AddTestItem()
        {
            for (int i = 0; i < testItemCount; i++)
            {
                PushItem(new MiniConsoleItemData() { text = Random.Range(0, int.MaxValue).ToLargeNumberString() });
            }
        }
#endif
    }
}