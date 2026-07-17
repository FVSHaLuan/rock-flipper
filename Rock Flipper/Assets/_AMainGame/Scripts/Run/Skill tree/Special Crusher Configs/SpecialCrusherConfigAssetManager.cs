using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run
{
    public class SpecialCrusherConfigAssetManager : ScriptableObjectWithInit
    {
        [SerializeField]
        private List<SpecialCrusherConfigAsset> configAssets;

        public SpecialCrusherConfigAsset GetConfigAsset(SpecialCrusherId specialCrusherId)
        {
            ///
            foreach (var configAsset in configAssets)
            {
                if (configAsset.SpecialCrusherId == specialCrusherId)
                {
                    return configAsset;
                }
            }

            ///
            throw new System.ArgumentOutOfRangeException($"Special Crusher Config Asset with ID {specialCrusherId} not found.");
        }
    }

}