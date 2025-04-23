using Scripts.Core.GameSystem;
using UnityEngine;

namespace Scripts.Entities.Players.MyPlayers
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private LineRenderer line;
        [SerializeField] private float height = 3;
        [SerializeField] private LayerMask _wallLayer;
        private Vector3 _direction;
        private void LateUpdate()
        {
            LookCameraPos();
        }

        private void LookCameraPos()
        {
            Ray ray = playerInput.GetCameraRay();

            float y = transform.position.y;
            float t = (ray.origin.y - y) / ray.direction.y;
            Vector3 hitPoint = ray.origin - ray.direction * t;

            Vector3 dir = hitPoint - transform.position;
            dir.y = 0f;

            if (dir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(dir);
            _direction = dir;
        }

        private void Update()
        {
            SetLine();
        }

        Vector3[] positions = new Vector3[2];
        private void SetLine()
        {
            positions[0] = transform.position;
            Debug.DrawRay(transform.position, _direction * 10f, Color.red);

            if (Physics.Raycast(transform.position, _direction, out RaycastHit ray, 100, _wallLayer))
                positions[1] = ray.point;
            else
                positions[1] = transform.position;
            line.SetPositions(positions);
        }
    }
}
