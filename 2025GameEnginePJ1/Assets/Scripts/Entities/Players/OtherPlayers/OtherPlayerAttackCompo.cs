using Core.EventSystem;
using Scripts.Core.EventSystem;
using Scripts.Entities;
using UnityEngine;

namespace Assets.Scripts.Entities.Players.OtherPlayers
{
    public class OtherPlayerAttackCompo : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private GameObject bulletPrefab;
        public void Initialize(NetworkEntity entity)
        {
        }
        public void Shoot(Vector3 position, Vector3 direction)
        {
            Instantiate(bulletPrefab, position, Quaternion.LookRotation(direction));
        }
    }
}
