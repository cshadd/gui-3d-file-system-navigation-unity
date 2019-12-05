using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public class CameraConnectorManager : MonoBehaviour
    {
        [SerializeField]
        private new Camera camera;
        [SerializeField]
        private Vector3 locationToLerp;
        [SerializeField]
        private Vector3 max;
        [SerializeField]
        private Vector3 min;
        [SerializeField]
        private bool minSet;

        private CameraConnectorManager() : base() { return; }

        public Camera Transition(GameObject o)
        {
            return Transition(o.transform);
        }
        public Camera Transition(Transform t)
        {
            locationToLerp = t.position + new Vector3(0, 10, -10);
            max = locationToLerp;
            if (!minSet)
            {
                min = locationToLerp;
                minSet = true;
            }
            return camera;
        }
        private void FixedUpdate()
        {
            var location = Vector3.Lerp(camera.transform.position,
                locationToLerp, 0.1f);
            camera.transform.position = location;
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
                var location = Vector3.forward * 0.5f;
                var futureLocation = locationToLerp + location;
                if (futureLocation.magnitude <= max.magnitude)
                {
                    locationToLerp += location;
                }

            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                var location = Vector3.back * 0.5f;
                var futureLocation = locationToLerp + location;
                if (futureLocation.magnitude >= min.magnitude)
                {
                    locationToLerp += location;
                }
            }
        }
    }
}
