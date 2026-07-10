using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace BT
{
    public partial class PlayerData
    {
        public void UpdateBeforeSave()
        {
            // for demonstration
            SaveTaskInstancesToList();

        }

        private void SaveTaskInstancesToList()
        {
            ///
            if (!correctedData)
            {
                return;
            }
        }
    }

}