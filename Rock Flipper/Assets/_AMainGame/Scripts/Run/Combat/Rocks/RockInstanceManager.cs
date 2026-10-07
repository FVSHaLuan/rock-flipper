using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class RockInstanceManager : ExtendedMonoBehaviourRun
    {
        private readonly List<Rock> activeRocks = new List<Rock>();

        /// <summary>
        /// All rocks currently on the playfield (spawned and not yet despawned)
        /// </summary>
        public IReadOnlyList<Rock> ActiveRocks => activeRocks;

        private Rock Spawn(RockPoolHandler rockPoolHandler)
        {
            var rock = CurrentSceneGeneralPool.TakeInstance(rockPoolHandler, this).TargetObject;            

            ///
            activeRocks.Add(rock);
            return rock;
        }

        public void Despawn(Rock rock)
        {
            activeRocks.Remove(rock);
            rock.PoolHandler.TryReturnToPoolAndDeactivate();
        }

        /// <summary>
        /// spawn a rock at the start of a new combat
        /// </summary>
        /// <param name="rockPoolHandler"></param>
        /// <returns></returns>
        public Rock SpawnAsOldRock(RockPoolHandler rockPoolHandler)
        {
            var rock = Spawn(rockPoolHandler);

            ///
            rock.transform.position = Playfield.GetRandomPoint(Vector2.zero);
            rock.StartNewLife(false);

            ///
            return rock;
        }

        public Rock SpawnAsReplacement(RockPoolHandler rockPoolHandler, Vector2 startPosition)
        {
            var rock = Spawn(rockPoolHandler);

            ///
            rock.transform.position = startPosition;
            rock.StartNewLife(true);

            ///
            return rock;
        }

        public Rock SpawnAsNewRock(RockPoolHandler rockPoolHandler, Vector2 startPosition)
        {
            var rock = Spawn(rockPoolHandler);

            ///
            rock.StartNewLife(false);

            ///
            rock.transform.position = startPosition;            
            rock.DoNewRockFlipping(Playfield.GetRandomPoint(Vector2.zero));            

            ///
            return rock;
        }
    }
}