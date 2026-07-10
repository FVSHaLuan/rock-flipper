using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace FH.Core.Architecture.Pool
{
    public class GeneralPoolMember : GeneralPoolMemberSimplified
    {
        [SerializeField]
        private UnityEvent onSpawned;
        [SerializeField]
        private UnityEvent onAfterSetPosition;

        public UnityEvent OnSpawned => onSpawned;
        public UnityEvent OnAfterSetPosition => onAfterSetPosition;      
    }

}