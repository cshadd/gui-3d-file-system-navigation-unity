using Gui3dFileSystemNavigationUnity.Data;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public class Sample2 : MonoBehaviour
    {
        [SerializeField]
        private List<DriveNode> driveNodes;

        RaycastHit hitInfo = new RaycastHit();

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
                platform.transform.parent = gameObject.transform.parent;
                platform.transform.position = new Vector3(0 - drivePosition, 0 , 10);
                platform.transform.localScale = new Vector3(1, 1, 1);
                var driveNode = platform.AddComponent<DriveNode>();
                driveNode.Assign(drive);
                driveNodes.Add(driveNode);
                drivePosition += 15;
            }
        }

        int count = 0;
        private void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit)
                {
                    DirectoryNode dn = hitInfo.transform.GetComponent<DirectoryNode>();
             
                    if (dn != null && !dn.extendedInfo.isShowingInternal)
                    {                        
                        dn.Populate(PrimitiveType.Capsule, PrimitiveType.Cube);

                        Vector3 islandPos = new Vector3(0, 0, 0);
                        if (!dn.extendedInfo.isAccessDenied)
                            islandPos = createIsland(dn);

                        count++;

                        int x = -4, y = 0, z = 4;
                        foreach (DirectoryNode directoryNode in dn.directoryNodes)
                        {
                            DirectoryNode folders = directoryNode;
                            var item = folders.gameObject.transform;
                            item.transform.position = new Vector3(islandPos.x + x, islandPos.y + y, islandPos.z + z);
                            folders.gameObject.transform.position = new Vector3(islandPos.x + x, islandPos.y + y, islandPos.z + z);

                            x += 1;
                            if (x >= 5)
                            {
                                x = -4;
                                z -= 1;
                            }

                            var renderer = item.GetComponent<Renderer>();
                            renderer.material.SetColor("_Color", Color.black);
                        }

                        foreach (FileNode fileNode in dn.fileNodes)
                        {
                            FileNode files = fileNode;
                            var items = files.gameObject.transform;
                            items.transform.position = new Vector3(islandPos.x + x, islandPos.y + y, islandPos.z + z);
                            items.transform.localScale = new Vector3(1, 2, 1);
                       
                            x += 1;
                            if (x >= 5)
                            {
                                x = -4;
                                z -= 1;
                            }
                       
                            var renderer = items.GetComponent<Renderer>();
                            renderer.material.SetColor("_Color", Color.red);
                        }

                        //DirectoryNode nd;
                        //if (count >= 1)
                        //{
                        //    nd = dn.gameObject.transform.parent.GetComponent<DirectoryNode>();
                        //    nd.Depopulate();
                        //}


                    }
                    else
                        Debug.Log("dn = null");
                }
            }
        }

        Vector3 createIsland(DirectoryNode directoryNode)
        {
            var island = GameObject.CreatePrimitive(PrimitiveType.Cube);
            island.transform.parent = directoryNode.gameObject.transform;
            island.transform.name = "Island";

            DirectoryNode parent;
            parent = directoryNode.parentDirectory;

            Debug.Log(count);
            Vector3 position;
            if (count >= 1)
            {
                var parentIsland = parent.transform.Find("Island");
                position = island.transform.position = new Vector3(
                              parentIsland.gameObject.transform.position.x,
                              parentIsland.gameObject.transform.position.y,
                              parentIsland.gameObject.transform.position.z + 15);
            }
            else
                position =
                   island.transform.position = new Vector3(
                            directoryNode.gameObject.transform.position.x,
                            directoryNode.gameObject.transform.position.y,
                            directoryNode.gameObject.transform.position.z + 15);

            island.transform.localScale = new Vector3(10, 1, 10);
        
            return position;
        }
    }
}
