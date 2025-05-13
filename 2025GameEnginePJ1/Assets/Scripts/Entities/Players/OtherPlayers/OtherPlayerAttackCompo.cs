using Core.EventSystem;
using Scripts.Core.EventSystem;
using Scripts.Entities;
using Scripts.Entities.Players.MyPlayers;
using UnityEngine;

namespace Assets.Scripts.Entities.Players.OtherPlayers
{
    public class OtherPlayerAttackCompo : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] public Transform currentGun;//임시용 나중에 총 여러개 만든다 했을때 어케할지 고민해야할듯
        public void Initialize(NetworkEntity entity)
        {
        }
        public void Shoot(Vector3 position, Vector3 direction)
        {
            Instantiate(bulletPrefab, position, Quaternion.LookRotation(direction));
        }
    }
}
