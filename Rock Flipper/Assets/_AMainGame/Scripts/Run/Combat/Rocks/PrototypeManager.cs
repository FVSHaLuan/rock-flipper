using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class PrototypeManager : ScriptableObjectWithInit
    {
        [SerializeField]
        private List<Rock> rocks = new List<Rock>();

        [System.NonSerialized]
        private Dictionary<RockTier, Rock> rockDictionary;
        [System.NonSerialized]
        private Dictionary<RockTier, Rock> pureRockDictionary;

        protected override void Init()
        {
            ///
#if UNITY_EDITOR
            if (!Validate())
            {
                throw new System.Exception("Validation failed in PrototypeManager.");
            }
#endif

            ///
            BuildRockDictionaries();

            ///
            base.Init();
        }

        private void BuildRockDictionaries()
        {
            rockDictionary = new Dictionary<RockTier, Rock>();
            pureRockDictionary = new Dictionary<RockTier, Rock>();
            foreach (var rock in rocks)
            {
                if (rock.IsPure)
                {
                    pureRockDictionary[rock.Tier] = rock;
                }
                else
                {
                    rockDictionary[rock.Tier] = rock;
                }
            }
        }

        public Rock GetRockPrototype(RockTier tier, float purity)
        {
            return GetRockPrototype(tier, Random.value < purity);
        }

        public Rock GetRockPrototype(RockTier tier, bool isPure)
        {
            ///
            TryInit();

            ///
            if (isPure)
            {
                if (pureRockDictionary.TryGetValue(tier, out var pureRock))
                {
                    return pureRock;
                }
                else
                {
                    Debug.LogError($"No pure rock found for tier {tier} in PrototypeManager: {name}");
                    return null;
                }
            }
            else
            {
                if (rockDictionary.TryGetValue(tier, out var rock))
                {
                    return rock;
                }
                else
                {
                    Debug.LogError($"No rock found for tier {tier} in PrototypeManager: {name}");
                    return null;
                }
            }
        }

#if UNITY_EDITOR
        private bool Validate()
        {
            ///
            return ValidateRocks();
        }

        private bool ValidateRocks()
        {
            bool isValid = true;

            ///
            HashSet<Rock> uniqueRocks = new HashSet<Rock>();

            ///
            foreach (var rock in rocks)
            {
                ///
                if (!uniqueRocks.Add(rock))
                {
                    Debug.LogError($"Duplicate rock found in PrototypeManager: {name}. Rock name: {rock.name}");
                    isValid = false;
                }

                ///
                if (rock == null)
                {
                    Debug.LogError($"Rock is null in PrototypeManager: {name}");
                    isValid = false;
                }
                else
                {
                    var upperName = rock.name.ToUpper();
                    if (upperName == "ROCK" || upperName == "ROCK PURE")
                    {
                        Debug.LogError($"Base rocks are not allowed. Rock name: {rock.name}");
                        isValid = false;
                    }
                }
            }

            ///
            return isValid;
        }
#endif
    }

}