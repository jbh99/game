using UnityEngine;

namespace AIWars.Units
{
    // Marker component placed on units that can be possessed by the player.
    // Holds references the camera/possession system needs.
    [RequireComponent(typeof(Unit))]
    public class PossessionTarget : MonoBehaviour
    {
        [Tooltip("Bone or empty transform near the unit's head — used as FPS camera anchor.")]
        public Transform headBone;

        [Tooltip("Optional FPS-only weapon model toggled when possessed.")]
        public GameObject fpsArmsModel;

        [Tooltip("Optional 3rd-person mesh hidden when possessed.")]
        public GameObject thirdPersonMesh;

        public void OnPossessed()
        {
            if (fpsArmsModel != null) fpsArmsModel.SetActive(true);
            if (thirdPersonMesh != null) thirdPersonMesh.SetActive(false);
        }

        public void OnReleased()
        {
            if (fpsArmsModel != null) fpsArmsModel.SetActive(false);
            if (thirdPersonMesh != null) thirdPersonMesh.SetActive(true);
        }
    }
}
