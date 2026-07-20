using UnityEngine;

namespace Agame.Run.Combat
{
    public class RockInstanceManager : ExtendedMonoBehaviourRun
    {
        private Rock Spawn(RockPoolHandler rockPoolHandler)
        {
            var rock = CurrentSceneGeneralPool.TakeInstance(rockPoolHandler, this).Rock;

            ///
            return rock;
        }

        public Rock SpawnAsOldRock(RockPoolHandler rockPoolHandler)
        {
            var rock = Spawn(rockPoolHandler);

            ///
            rock.transform.position = Playfield.GetRandomPoint(Vector2.zero);
            rock.StartNewLife();

            ///
            return rock;
        }
    }
}