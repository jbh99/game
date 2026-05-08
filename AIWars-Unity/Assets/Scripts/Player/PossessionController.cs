using UnityEngine;
using AIWars.CameraSystems;
using AIWars.Units;

namespace AIWars.Player
{
    // Drives the C/V toggle, manual movement, look, and fire when possessing a unit.
    public class PossessionController : MonoBehaviour
    {
        [SerializeField] KeyCode possessKey = KeyCode.C;
        [SerializeField] KeyCode releaseKey = KeyCode.V;
        [SerializeField] float lookSensitivity = 2.5f;
        [SerializeField] Transform pitchPivot; // optional: parent of FPS camera for pitch rotation
        [SerializeField] float minPitch = -80f, maxPitch = 80f;

        Unit _possessed;
        PossessionTarget _target;
        float _yaw, _pitch;

        public Unit PossessedUnit => _possessed;

        void Update()
        {
            HandleToggleInput();
            if (_possessed == null) return;
            HandleLook();
            HandleMove();
            HandleFire();
        }

        void HandleToggleInput()
        {
            if (Input.GetKeyDown(possessKey) && _possessed == null)
                TryPossessSelected();
            if (Input.GetKeyDown(releaseKey) && _possessed != null)
                ReleaseCurrent();
        }

        void TryPossessSelected()
        {
            // Hook this into the RTS selection system. For demo we raycast from RTS camera.
            Camera src = CameraManager.Instance != null ? CameraManager.Instance.ActiveCamera : Camera.main;
            if (src == null) return;
            Ray ray = src.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, 500f)) return;
            if (!hit.collider.TryGetComponent(out Unit unit)) return;
            if (!unit.TryGetComponent(out PossessionTarget target)) return;
            Possess(unit, target);
        }

        public void Possess(Unit unit, PossessionTarget target)
        {
            _possessed = unit;
            _target = target;
            unit.SetPossessed(true);
            target.OnPossessed();
            if (CameraManager.Instance != null) CameraManager.Instance.EnterFpsView(target);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void ReleaseCurrent()
        {
            if (_possessed == null) return;
            _possessed.SetPossessed(false);
            _target?.OnReleased();
            if (CameraManager.Instance != null) CameraManager.Instance.EnterRtsView();
            _possessed = null;
            _target = null;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        void HandleLook()
        {
            float mx = Input.GetAxis("Mouse X") * lookSensitivity;
            float my = Input.GetAxis("Mouse Y") * lookSensitivity;
            _yaw   += mx;
            _pitch  = Mathf.Clamp(_pitch - my, minPitch, maxPitch);
            _possessed.transform.rotation = Quaternion.Euler(0f, _yaw, 0f);
            if (pitchPivot != null) pitchPivot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }

        void HandleMove()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            var loco = _possessed.GetComponent<UnitLocomotion>();
            Camera cam = CameraManager.Instance != null ? CameraManager.Instance.ActiveCamera : Camera.main;
            loco.ApplyManualMove(new Vector2(h, v), cam != null ? cam.transform : _possessed.transform, true);
        }

        void HandleFire()
        {
            if (Input.GetMouseButton(0))
            {
                var combat = _possessed.GetComponent<UnitCombat>();
                Camera cam = CameraManager.Instance != null ? CameraManager.Instance.ActiveCamera : Camera.main;
                combat.TryFireForward(cam, true);
            }
        }
    }
}
