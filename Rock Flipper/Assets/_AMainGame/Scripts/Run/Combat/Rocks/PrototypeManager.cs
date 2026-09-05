using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Agame.Run.Combat
{
    public class PrototypeManager : ScriptableObjectWithInit
    {
        [SerializeField, FormerlySerializedAs("rocks")]
        private List<Rock> rockPrototypes = new List<Rock>();
        [SerializeField]
        private List<Chest> chestPrototypes = new List<Chest>();

        [System.NonSerialized]
        private Dictionary<RockTier, Rock> rockDictionary;
        [System.NonSerialized]
        private Dictionary<RockTier, Rock> pureRockDictionary;
        [System.NonSerialized]
        private Dictionary<ChestRarity, Chest> chestDictionary;

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
            BuildChestDictionaries();

            ///
            base.Init();
        }

        private void BuildRockDictionaries()
        {
            rockDictionary = new Dictionary<RockTier, Rock>();
            pureRockDictionary = new Dictionary<RockTier, Rock>();
            foreach (var rock in rockPrototypes)
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

        private void BuildChestDictionaries()
        {
            chestDictionary = new Dictionary<ChestRarity, Chest>();
            foreach (var chest in chestPrototypes)
            {
                chestDictionary[chest.Rarity] = chest;
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

        public Chest GetChestPrototype(ChestRarity rarity)
        {
            ///
            TryInit();

            ///
            if (chestDictionary.TryGetValue(rarity, out var chest))
            {
                return chest;
            }
            else
            {
                Debug.LogError($"No chest found for rarity {rarity} in PrototypeManager: {name}");
                return null;
            }
        }

#if UNITY_EDITOR
        private bool Validate()
        {
            ///
            return ValidateRocks() && ValidateChests();
        }

        private bool ValidateRocks()
        {
            bool isValid = true;

            ///
            HashSet<Rock> uniqueRocks = new HashSet<Rock>();

            ///
            foreach (var rock in rockPrototypes)
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

        private bool ValidateChests()
        {
            bool isValid = true;
            ///
            HashSet<Chest> uniqueChests = new HashSet<Chest>();
            ///
            foreach (var chest in chestPrototypes)
            {
                ///
                if (!uniqueChests.Add(chest))
                {
                    Debug.LogError($"Duplicate chest found in PrototypeManager: {name}. Chest name: {chest.name}");
                    isValid = false;
                }
                ///
                if (chest == null)
                {
                    Debug.LogError($"Chest is null in PrototypeManager: {name}");
                    isValid = false;
                }
            }
            ///
            return isValid;
        }
#endif
    }

}