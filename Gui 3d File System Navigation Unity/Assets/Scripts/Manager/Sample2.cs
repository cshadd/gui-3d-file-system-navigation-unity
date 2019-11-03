using Gui3dFileSystemNavigationUnity.Data;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public class Sample2 : MonoBehaviour
    {
        [SerializeField]
        private List<DriveNode> driveNodes;
        private List<DirectoryNode> DirectoryNodes;
        private List<FileNode> FileNodes;

        private Sample2() : base()
        {
            return;
        }

        private void Start()
        {
            int drivePosition = 0;
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                var platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
                platform.transform.parent = gameObject.transform;
                platform.transform.position = new Vector3(0 - drivePosition, 0 , 10);
                platform.transform.localScale = new Vector3(1, 1, 1);
                var driveNode = platform.AddComponent<DriveNode>();
                driveNode.Assign(drive);
                driveNodes.Add(driveNode);
                drivePosition += 15;
            }
        }

        RaycastHit hitInfo = new RaycastHit();
        float x, y, z;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit)
                {
                    DirectoryNode dn = hitInfo.transform.GetComponent<DirectoryNode>();
                    //FileNode fn = hitInfo.transform.GetComponent<FileNode>();
                    dn.Populate(PrimitiveType.Capsule, PrimitiveType.Cube);

                    int x=-4, y=0, z=4;
                    Vector3 islandPos = createIsland(dn.transform.localPosition.x, dn.transform.localPosition.y, dn.transform.localPosition.z);
                    foreach (DirectoryNode directoryNode in dn.directoryNodes)
                    {
                        dn = directoryNode;
                        var item = dn.gameObject.transform;
                        item.transform.position = new Vector3(islandPos.x + x, islandPos.y + y, islandPos.z + z);

                        x += 1;
                        if(x >= 5)
                        {
                            x = -4;
                            z -= 1;
                        }
                        Debug.Log(x);

                        var renderer = item.GetComponent<Renderer>();
                        renderer.material.SetColor("_Color", Color.black);
                    }

                    //foreach (FileNode fileNode in fn.fileNodes)
                    //{
                    //    fn = fileNode;
                    //    var items = fn.gameObject.transform;
                    //    items.transform.position = new Vector3(5, 10, 15 + position);
                    //    position += 5;
                    //}
                }
            }
        }

        Vector3 createIsland(float x, float y, float z)
        {
            var platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
            platform.transform.parent = gameObject.transform;
            Vector3 position = platform.transform.position = new Vector3(x,y+5,z+15);
            platform.transform.localScale = new Vector3(10, 1, 10);
            return position;
        }

    }
}
