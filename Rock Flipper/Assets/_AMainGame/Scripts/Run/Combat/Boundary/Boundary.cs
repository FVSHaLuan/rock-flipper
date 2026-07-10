using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class Boundary : ExtendedMonoBehaviourRun
    {
        [Header("Walls")]
        [SerializeField]
        GameObject wallLeft;
        [SerializeField]
        GameObject wallRight;
        [SerializeField]
        GameObject wallTop;
        [SerializeField]
        GameObject wallBottom;

        [Space]
        [SerializeField]
        private float padding;

        protected override void ExtendedAwake()
        {
            RunEntry.playfield.DoAfterCalculated(SetWallsPosition);
        }

        private void SetWallsPosition()
        {
            ///
            transform.position = Playfield.CenterPoint;

            ///
            float z = wallLeft.transform.localPosition.z;

            ///
            var playfield = RunEntry.playfield;

            ///
            wallLeft.transform.localPosition = new Vector3(-playfield.HalfWidth - padding, 0, z);
            wallRight.transform.localPosition = new Vector3(playfield.HalfWidth + padding, 0, z);
            wallTop.transform.localPosition = new Vector3(0, playfield.HalfHeight + padding, z);
            wallBottom.transform.localPosition = new Vector3(0, -playfield.HalfHeight - padding, z);
        }
    }

}