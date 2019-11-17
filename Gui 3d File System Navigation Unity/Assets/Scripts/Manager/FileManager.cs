using Gui3dFileSystemNavigationUnity.Data;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public class FileManager : MonoBehaviour
    {
        private int count;
        [SerializeField]
        private List<DriveNode> driveNodes;
        private RaycastHit hitInfo;

        private FileManager() : base() { return; }

        private Vector3 createIsland(DirectoryNode dn)
        {
            Debug.Log(count);

            var island = GameObject.CreatePrimitive(PrimitiveType.Cube);
            island.transform.parent = dn.transform;
            island.transform.name = "Island of " + dn.name;

            var parent = dn.parentDirectory;

            Vector3 position;
            if (count >= 1)
            {
                var parentIsland = parent.transform.Find("Island of " + parent.name);
                position = island.transform.position = new Vector3(
                              parentIsland.transform.position.x,
                              parentIsland.transform.position.y,
                              parentIsland.transform.position.z + 15);
            }
            else
            {
                position = island.transform.position = new Vector3(
                            dn.transform.position.x,
                            dn.transform.position.y,
                            dn.transform.position.z + 15);
            }

            island.transform.localScale = new Vector3(10, 1, 10);

            return position;
        }

        private void Start()
        {
            Debug.Log(Application.productName + " started.");
            count = 0;
            // hitInfo = new RaycastHit();

            var drivePosition = 0;
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                var platform = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                platform.transform.parent = transform.parent;
                platform.transform.position = new Vector3(0 - drivePosition, 0 , 10);
                platform.transform.localScale = new Vector3(1, 1, 1);
                var driveNode = platform.AddComponent<DriveNode>();
                driveNode.Assign(drive);
                driveNodes.Add(driveNode);
                drivePosition += 15;
            }
            return;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Mouse click left.");
                var hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit)
                {
                    var dn = hitInfo.transform.GetComponent<DirectoryNode>();
                    if (dn != null && !dn.extendedInfo.isShowingInternal)
                    {
                        dn.Populate(PrimitiveType.Capsule, PrimitiveType.Cube);

                        var islandPos = new Vector3(0, 0, 0);
                        if (!dn.extendedInfo.isAccessDenied)
                        {
                            islandPos = createIsland(dn);
                        }

                        count++;

                        var x = -4;
                        var y = 0;
                        var z = 4;
                        foreach (DirectoryNode directoryNode in dn.directoryNodes)
                        {
                            directoryNode.transform.position = new Vector3(
                                islandPos.x + x,
                                islandPos.y + y,
                                islandPos.z + z);

                            x += 1;
                            if (x >= 5)
                            {
                                x = -4;
                                z -= 1;
                            }

                            var renderer = directoryNode.GetComponent<Renderer>();
                            renderer.material.SetColor("_Color", Color.black);
                        }

                        foreach (FileNode fileNode in dn.fileNodes)
                        {
                            fileNode.transform.position = new Vector3(islandPos.x + x, islandPos.y + y, islandPos.z + z);
                            fileNode.transform.localScale = new Vector3(1, 2, 1);

                            x += 1;
                            if (x >= 5)
                            {
                                x = -4;
                                z -= 1;
                            }

                            var renderer = fileNode.GetComponent<Renderer>();
                            renderer.material.SetColor("_Color", Color.red);
                        }

                        //DirectoryNode nd;
                        //if (count >= 1)
                        //{
                        //    nd = dn.gameObject.transform.parent.GetComponent<DirectoryNode>();
                        //    nd.Depopulate();
                        //}
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Mouse click right.");
                var hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit)
                {
                    var dn = hitInfo.transform.GetComponent<DirectoryNode>();
                    var fn = hitInfo.transform.GetComponent<FileNode>();
                    if (dn != null)
                    {

                    }
                    else if (fn)
                    {

                    }
                }
            }
            return;
        }
    }
}
