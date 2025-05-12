using Scripts.Core.GameSystem;
using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Entities.Players.MyPlayers
{
    public class Gun : MonoBehaviour
    {
        [field: SerializeField] public Transform FirePos { get; private set; }
        [field: SerializeField] public float attackDelay { get; private set; } = 0.2f;
        private WaitForSeconds _wait;
        public WaitForSeconds Wait => _wait ??= new WaitForSeconds(attackDelay);

    }
}
