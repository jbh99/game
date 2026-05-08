using UnityEngine;
using AIWars.Audio;

namespace AIWars.Units
{
    public class UnitCombat : MonoBehaviour
    {
        public const float POSSESS_BUFF = 1.5f; // +50% damage
        [SerializeField] float baseDamage = 10f;
        [SerializeField] float fireRate = 1f;
        [SerializeField] float range = 25f;
        [SerializeField] LayerMask hitMask = ~0;
        [SerializeField] Transform muzzle;
        [SerializeField] string sfxKey = "weapon_default";

        bool _manual;
        float _nextFireTime;

        public void Initialize(float damage)
        {
            baseDamage = damage;
        }

        public void SetManualControl(bool manual) => _manual = manual;

        public float GetCurrentDamage(bool possessed)
            => possessed ? baseDamage * POSSESS_BUFF : baseDamage;

        // Auto-attack used by AI when not possessed.
        public void TryAutoAttack(Transform target, bool possessed)
        {
            if (_manual) return;
            if (target == null) return;
            if (Time.time < _nextFireTime) return;
            if (Vector3.Distance(transform.position, target.position) > range) return;
            FireAt(target.position, possessed);
        }

        // Manual attack from possessed player click.
        public bool TryFireForward(Camera cam, bool possessed)
        {
            if (!_manual) return false;
            if (Time.time < _nextFireTime) return false;
            Ray ray = cam != null ? cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f)) : new Ray(transform.position, transform.forward);
            Vector3 target = ray.origin + ray.direction * range;
            if (Physics.Raycast(ray, out RaycastHit hit, range, hitMask))
            {
                target = hit.point;
                if (hit.collider.TryGetComponent(out UnitHealth health))
                    health.TakeDamage(GetCurrentDamage(possessed));
            }
            FireAt(target, possessed);
            return true;
        }

        void FireAt(Vector3 worldPos, bool possessed)
        {
            _nextFireTime = Time.time + 1f / fireRate;
            if (AudioManager.Instance != null && muzzle != null)
                AudioManager.Instance.PlayOneShot(sfxKey, muzzle.position);
        }
    }
}
