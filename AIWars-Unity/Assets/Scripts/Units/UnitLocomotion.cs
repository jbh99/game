using UnityEngine;
using UnityEngine.AI;

namespace AIWars.Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnitLocomotion : MonoBehaviour
    {
        public const float POSSESS_BUFF = 1.5f; // +50%
        [SerializeField] float baseSpeed = 4f;
        NavMeshAgent _agent;
        bool _manual;

        void Awake() { _agent = GetComponent<NavMeshAgent>(); }

        public void Initialize(float speed)
        {
            baseSpeed = speed;
            if (_agent != null) _agent.speed = speed;
        }

        public void SetManualControl(bool manual)
        {
            _manual = manual;
            if (_agent != null) _agent.enabled = !manual;
        }

        public float GetCurrentSpeed(bool possessed)
            => possessed ? baseSpeed * POSSESS_BUFF : baseSpeed;

        public void MoveTo(Vector3 worldPosition)
        {
            if (_manual || _agent == null || !_agent.enabled) return;
            _agent.SetDestination(worldPosition);
        }

        // Called from PlayerController.PossessionController when possessed.
        public void ApplyManualMove(Vector2 input, Transform cameraRig, bool possessed)
        {
            if (!_manual) return;
            Vector3 fwd = cameraRig != null ? cameraRig.forward : transform.forward;
            Vector3 right = cameraRig != null ? cameraRig.right : transform.right;
            fwd.y = 0; right.y = 0; fwd.Normalize(); right.Normalize();

            Vector3 move = (right * input.x + fwd * input.y).normalized;
            transform.position += move * GetCurrentSpeed(possessed) * Time.deltaTime;
        }
    }
}
