using UnityEngine;
using AIWars.Units;
#if CINEMACHINE
using Cinemachine;
#endif

namespace AIWars.CameraSystems
{
    // Handles the RTS top-down ↔ 1st-person possession swap from the plan.
    // Works with or without Cinemachine (define CINEMACHINE in player settings if installed).
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance { get; private set; }

#if CINEMACHINE
        [SerializeField] CinemachineVirtualCamera rtsCamera;
        [SerializeField] CinemachineVirtualCamera fpsCamera;
#else
        [SerializeField] Camera rtsCamera;
        [SerializeField] Camera fpsCamera;
#endif
        public Transform CurrentFpsAnchor { get; private set; }
        public Camera ActiveCamera { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            EnterRtsView();
        }

        public void EnterRtsView()
        {
#if CINEMACHINE
            if (rtsCamera != null) rtsCamera.Priority = 20;
            if (fpsCamera != null) fpsCamera.Priority = 0;
#else
            if (rtsCamera != null) rtsCamera.gameObject.SetActive(true);
            if (fpsCamera != null) fpsCamera.gameObject.SetActive(false);
            ActiveCamera = rtsCamera;
#endif
            CurrentFpsAnchor = null;
        }

        public void EnterFpsView(PossessionTarget target)
        {
            if (target == null) return;
            CurrentFpsAnchor = target.headBone != null ? target.headBone : target.transform;

#if CINEMACHINE
            if (fpsCamera != null)
            {
                fpsCamera.Follow = CurrentFpsAnchor;
                fpsCamera.LookAt = CurrentFpsAnchor;
                fpsCamera.Priority = 30;
            }
            if (rtsCamera != null) rtsCamera.Priority = 0;
#else
            if (fpsCamera != null)
            {
                fpsCamera.gameObject.SetActive(true);
                fpsCamera.transform.SetParent(CurrentFpsAnchor, false);
                fpsCamera.transform.localPosition = Vector3.zero;
                fpsCamera.transform.localRotation = Quaternion.identity;
                ActiveCamera = fpsCamera;
            }
            if (rtsCamera != null) rtsCamera.gameObject.SetActive(false);
#endif
        }
    }
}
