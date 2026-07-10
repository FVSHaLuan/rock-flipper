using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GD;
using System;

namespace BT
{
    public partial class PlayerData
    {
        [NonSerialized]
        private bool correctedData;

        public void CorrectData(PlayerData defaultData)
        {
            ///
            isCorrectingData = true;

            ///
            CorrectNullList();

            ///
            if (randomSeed == 0)
            {
                randomSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
                if (randomSeed == 0)
                {
                    randomSeed = 1;
                }
            }

            ///
            isCorrectingData = false;
            correctedData = true;
        }

        private void CorrectNullList()
        {
            
        }

    }

}