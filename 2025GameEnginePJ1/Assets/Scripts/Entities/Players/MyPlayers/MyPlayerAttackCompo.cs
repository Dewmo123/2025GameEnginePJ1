using Assets.Scripts.Entities;
using Scripts.Core;
using Scripts.Core.GameSystem;
using Scripts.Entities.Players.MyPlayers;
using Scripts.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Entities.Players.MyPlayers
{
    public class MyPlayerAttackCompo : MonoBehaviour, IEntityComponent
    {

        private PlayerInputSO playerInput;
        [SerializeField] private LineRenderer line;
        [SerializeField] private LayerMask _wallLayer;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private List<Gun> guns;
        private Gun _currentGun;
        private Vector3 _direction;
        private bool _isAiming;
        private Coroutine _shooting;
        private float _lastAttackTime;
        private Transform firePos => _currentGun.FirePos;
        private float attackDelay => _currentGun.attackDelay;
        public void Initialize(NetworkEntity entity)
        {
            _currentGun = guns[0];
            playerInput = (entity as MyPlayer).PlayerInput;
            playerInput.OnAimEvent += HandleAim;
            line.transform.parent = null;
            line.transform.position = Vector3.zero;
            _direction = Vector3.one;
        }
        private void OnDestroy()
        {
            playerInput.OnAimEvent -= HandleAim;
        }
        public void HandleAttack(bool obj)
        {
            if (obj)
            {
                if (Time.time - _lastAttackTime >= attackDelay)
                    _shooting = StartCoroutine(Shoot());
            }
            else if (!obj && _shooting != null)
                StopCoroutine(_shooting);
        }

        private IEnumerator Shoot()
        {
            while (true)
            {
                _lastAttackTime = Time.time;
                Instantiate(bulletPrefab, firePos.position, Quaternion.LookRotation(_direction));
                var ray = Physics2D.Raycast(firePos.position, _direction, 123, _wallLayer);
                C_ShootReq req = new C_ShootReq();
                req.firePos = firePos.position.ToPacket();
                req.direction = _direction.ToPacket();
                if (ray && ray.collider.TryGetComponent(out Player player))
                    req.hitPlayerIndex = player.Index;
                else
                    req.hitPlayerIndex = 0;
                NetworkManager.Instance.SendPacket(req);
                yield return _currentGun.Wait;
            }
        }
        private void HandleAim(bool obj)
        {
            _isAiming = obj;
        }

        private void FixedUpdate()
        {
            if (_isAiming)
                LookCameraPos();
        }
        private void Update()
        {
            if (_isAiming)
                SetLine();
        }

        private void LookCameraPos()
        {
            Ray ray = playerInput.GetCameraRay();

            float y = _currentGun.transform.position.y;
            float t = (ray.origin.y - y) / ray.direction.y;
            Vector3 hitPoint = ray.origin - ray.direction * t;

            Vector3 dir = hitPoint - _currentGun.transform.position;
            dir.y = 0f;
            Quaternion rotation = Quaternion.LookRotation(dir);
            if (dir != Vector3.zero)
            {
                Quaternion slerp = Quaternion.Slerp(_currentGun.transform.rotation, rotation, Time.fixedDeltaTime * 20);
                _currentGun.transform.rotation = slerp;
                _direction = Quaternion.Slerp(Quaternion.LookRotation(_direction), rotation, Time.fixedDeltaTime * 20) * Vector3.forward;
            }
        }

        Vector3[] positions = new Vector3[2];
        private void SetLine()
        {
            positions[0] = firePos.position;
            Debug.DrawRay(_currentGun.transform.position, _direction * 10f, Color.red);

            if (Physics.Raycast(_currentGun.transform.position, _direction, out RaycastHit ray, 100, _wallLayer))
                positions[1] = ray.point;
            else
                positions[1] = firePos.position;
            line.SetPositions(positions);
        }
    }
}
