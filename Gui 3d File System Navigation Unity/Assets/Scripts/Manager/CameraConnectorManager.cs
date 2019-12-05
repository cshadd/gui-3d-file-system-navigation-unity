using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public class CameraConnectorManager : MonoBehaviour
    {
        private const float Sensitivity = 0.5f;
        private const float Speed = 16f;

        [SerializeField]
        private new Camera camera;
        [SerializeField]
        private Vector3 locationToLerp;

        private CameraConnectorManager() : base() { return; }

        public Camera Transition(GameObject o)
        {
            return Transition(o.transform);
        }
        public Camera Transition(Transform t)
        {
            locationToLerp = t.position + new Vector3(0, 10, -10);
            return camera;
        }
        private void FixedUpdate()
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position,
                locationToLerp, 0.1f); ;
            return;
        }
        private void Start()
        {
            camera.transform.Rotate(new Vector3(45, 0, 0));
            return;
        }
        private void Update()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                locationToLerp += Vector3.forward * 1f / 2f;
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                locationToLerp += Vector3.left * 1f / 2f;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                locationToLerp += Vector3.right * 1f / 2f;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                locationToLerp += Vector3.back * 1f / 2f;
            }
        }
    }
}
