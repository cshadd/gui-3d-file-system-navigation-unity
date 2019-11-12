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

        RaycastHit hitInfo = new RaycastHit();
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
                if (hit)
                {
                    DirectoryNode dn = hitInfo.transform.GetComponent<DirectoryNode>();
                    ExtendedInfo extendedInfo = dn.extendedInfo;
                    if (dn != null)
                    {
                        Debug.Log(dn.transform.localPosition.x + " " + dn.transform.localPosition.y + " "+ dn.transform.localPosition.z);
                        dn.Populate(PrimitiveType.Capsule, PrimitiveType.Cube);

                        Vector3 islandPos = new Vector3(0, 0, 0);
                        if (!extendedInfo.isAccessDenied)
                               islandPos = createIsland(dn);

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
                    }
                    else
                        Debug.Log("dn = null");
                }
            }
        }

        Vector3 createIsland(DirectoryNode dn)
        {
            var platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
            platform.transform.parent = dn.gameObject.transform;
            platform.transform.name = "Island";

            Debug.Log(dn.transform.localPosition.x + " " + dn.transform.localPosition.y + " " + dn.transform.localPosition.z);

            Vector3 position = platform.transform.position = new Vector3(
                            dn.gameObject.transform.localPosition.x,
                            dn.gameObject.transform.localPosition.y,
                            dn.gameObject.transform.localPosition.z+15);

            //Vector3 position = platform.transform.position = new Vector3(10, 10, 10);

            Debug.Log(dn.gameObject.transform.localPosition.x + " " + dn.gameObject.transform.localPosition.y + " " + dn.gameObject.transform.localPosition.z);

            platform.transform.localScale = new Vector3(10, 1, 10);

            return position;
        }

    }
}
