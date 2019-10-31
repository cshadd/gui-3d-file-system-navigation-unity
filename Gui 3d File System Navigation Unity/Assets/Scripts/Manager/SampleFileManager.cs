#region SAMPLE
using Gui3dFileSystemNavigationUnity.Data;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public class SampleFileManager : MonoBehaviour
    {
        [SerializeField]
        private List<DriveNode> driveNodes;

        private SampleFileManager() : base()
        {
            return;
        }

        private void Start()
        {
            int drivePosition = 0;
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                cylinder.transform.parent = gameObject.transform;
                cylinder.transform.position = new Vector3(0, 0 - drivePosition, 0);
                var driveNode = cylinder.AddComponent<DriveNode>();
                driveNode.Assign(drive);
                driveNodes.Add(driveNode);
                drivePosition += 5;
            }
        }
    }
}
#endregion
