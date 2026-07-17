using System.Runtime.Serialization;
using Agame.Run;
using UnityEngine;

namespace Agame
{
    [System.Serializable]
    public class SkillNodeStateDictionary : SerializableDictionary<string, SkillNodeState>
    {
        public SkillNodeStateDictionary() { }

        protected SkillNodeStateDictionary(SerializationInfo information, StreamingContext context)
        {
        }
    }

}