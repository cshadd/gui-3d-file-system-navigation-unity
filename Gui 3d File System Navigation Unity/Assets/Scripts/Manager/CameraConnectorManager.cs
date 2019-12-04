using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public class CameraConnectorManager : MonoBehaviour
    {
        [SerializeField]
        private new Camera camera;

        private CameraConnectorManager() : base() { return; }

        public Camera Transition(GameObject o)
        {
            return Transition(o.transform);
        }
        public Camera Transition(Transform t)
        {
            // TODO
            camera.transform.position =
                t.position + new Vector3(0, 10, -10);
            return camera;
        }
        private void Start()
        {
            // TODO
            camera.transform.Rotate(new Vector3(45, 0, 0));
            return;
        }
    }
}
