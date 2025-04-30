using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Core.GameSystem
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "SO/PlayerInput", order = 0)]
    public class PlayerInputSO : ScriptableObject,Controls.IPlayerActions
    {
        [SerializeField] private LayerMask whatIsGround;

        public event Action<bool> OnAimEvent;
        public event Action<bool> OnSprintEvent;
        private Controls _controls;
        public Vector2 MovementKey { get; private set; }

        private Vector2 _screenPosition;
        private Vector3 _worldPosition;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }
        private void OnDisable()
        {
            _controls.Player.Disable();
        }
        public void OnAim(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnAimEvent?.Invoke(true);
            else if (context.canceled)
                OnAimEvent?.Invoke(false);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
        }

        public void OnJump(InputAction.CallbackContext context)
        {
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MovementKey = context.ReadValue<Vector2>();
        }

        public void OnPointer(InputAction.CallbackContext context)
        {
            _screenPosition = context.ReadValue<Vector2>();
        }
        public Vector3 GetWorldPosition()
        {
            Camera mainCam = Camera.main;
            Debug.Assert(mainCam != null, "No main Cam in Scene");
            Ray camKey = mainCam.ScreenPointToRay(_screenPosition);
            if (Physics.Raycast(camKey, out RaycastHit hit, mainCam.farClipPlane, whatIsGround))
            {
                _worldPosition = hit.point;
            }
            return _worldPosition;
        }
        public Ray GetCameraRay()=> Camera.main.ScreenPointToRay(_screenPosition);
        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnSprintEvent?.Invoke(true);
            else if (context.canceled)
                OnSprintEvent?.Invoke(false);
        }
    }
}
