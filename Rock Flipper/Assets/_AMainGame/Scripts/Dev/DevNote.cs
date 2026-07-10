using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Dev
{
    public class DevNote : MonoBehaviour
    {
        [SerializeField, TextArea(minLines: 1, maxLines: 15)]
        private string note;
    }

}