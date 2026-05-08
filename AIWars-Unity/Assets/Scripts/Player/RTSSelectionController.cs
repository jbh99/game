using System.Collections.Generic;
using UnityEngine;
using AIWars.Units;

namespace AIWars.Player
{
    public class RTSSelectionController : MonoBehaviour
    {
        [SerializeField] LayerMask selectableMask = ~0;
        public readonly List<Unit> Selected = new();

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Camera cam = Camera.main;
                if (cam == null) return;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 500f, selectableMask)
                    && hit.collider.TryGetComponent(out Unit u))
                {
                    Selected.Clear();
                    Selected.Add(u);
                }
            }

            if (Input.GetMouseButtonDown(1) && Selected.Count > 0)
            {
                Camera cam = Camera.main;
                if (cam == null) return;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 500f))
                    foreach (var u in Selected)
                        u.GetComponent<UnitLocomotion>().MoveTo(hit.point);
            }
        }
    }
}
