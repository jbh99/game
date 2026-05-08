using UnityEngine;

namespace AIWars.Player
{
    // Simple RTS pan/zoom controller for the top-down view.
    public class RTSCameraController : MonoBehaviour
    {
        [SerializeField] float panSpeed = 30f;
        [SerializeField] float zoomSpeed = 200f;
        [SerializeField] float minHeight = 15f;
        [SerializeField] float maxHeight = 250f;
        [SerializeField] float edgeThreshold = 8f;

        void Update()
        {
            Vector3 move = Vector3.zero;
            if (Input.GetKey(KeyCode.UpArrow)    || Input.mousePosition.y >= Screen.height - edgeThreshold) move.z += 1;
            if (Input.GetKey(KeyCode.DownArrow)  || Input.mousePosition.y <= edgeThreshold)                 move.z -= 1;
            if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width  - edgeThreshold) move.x += 1;
            if (Input.GetKey(KeyCode.LeftArrow)  || Input.mousePosition.x <= edgeThreshold)                 move.x -= 1;
            transform.position += move * panSpeed * Time.deltaTime;

            float scroll = Input.mouseScrollDelta.y;
            if (Mathf.Abs(scroll) > 0.01f)
            {
                Vector3 p = transform.position;
                p.y = Mathf.Clamp(p.y - scroll * zoomSpeed * Time.deltaTime, minHeight, maxHeight);
                transform.position = p;
            }
        }
    }
}
