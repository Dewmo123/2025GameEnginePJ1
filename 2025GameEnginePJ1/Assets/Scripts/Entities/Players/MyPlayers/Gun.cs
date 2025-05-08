using Scripts.Core.GameSystem;
using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Entities.Players.MyPlayers
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private LineRenderer line;
        [SerializeField] private float height = 3;
        [SerializeField] private float attackDelay = 0.2f;
        [SerializeField] private LayerMask _wallLayer;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePos;
        private Vector3 _direction;
        private bool _isAiming;
        private Coroutine _shooting;
        private WaitForSeconds wait;
        private float _lastAttackTime;
        private void Awake()
        {
            wait = new WaitForSeconds(attackDelay);
            playerInput.OnAimEvent += HandleAim;
            playerInput.OnAttackEvent += HandleAttack;
            line.transform.parent = null;
            line.transform.position = Vector3.zero;
            _direction = Vector3.one;
        }
        private void OnDestroy()
        {
            playerInput.OnAimEvent -= HandleAim;
        }
        private void HandleAttack(bool obj)
        {
            if (obj && _isAiming)
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
                yield return wait;
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

            float y = transform.position.y;
            float t = (ray.origin.y - y) / ray.direction.y;
            Vector3 hitPoint = ray.origin - ray.direction * t;

            Vector3 dir = hitPoint - transform.position;
            dir.y = 0f;
            Quaternion rotation = Quaternion.LookRotation(dir);
            if (dir != Vector3.zero)
            {
                Quaternion slerp = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * 20);
                transform.rotation = slerp;
                _direction = Quaternion.Slerp(Quaternion.LookRotation(_direction), rotation, Time.fixedDeltaTime * 20) * Vector3.forward;
            }
        }

        Vector3[] positions = new Vector3[2];
        private void SetLine()
        {
            positions[0] = firePos.position;
            Debug.DrawRay(transform.position, _direction * 10f, Color.red);

            if (Physics.Raycast(transform.position, _direction, out RaycastHit ray, 100, _wallLayer))
                positions[1] = ray.point;
            else
                positions[1] = firePos.position;
            line.SetPositions(positions);
        }
    }
}
