using Gui3dFileSystemNavigationUnity.Data;
using System;
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
        [SerializeField]
        private FileUIConnectorManager uiConnector;
        private RaycastHit hitInfo;
        [SerializeField]
        private Root root;

        private FileManager() : base() { return; }

        private class Root : DirectoryNode
        {
            [SerializeField]
            private List<DriveNode> driveNodes;

            public Root() : base("") { return; }

        }

        private Vector3 createIsland(DirectoryNode directoryNode)
        {
            Debug.Log(count);

            var island = GameObject.CreatePrimitive(PrimitiveType.Cube);
            island.transform.parent = directoryNode.transform;
            island.transform.name = "Island of " + directoryNode.name;

            var parent = directoryNode.parentDirectory;

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
                            directoryNode.transform.position.x,
                            directoryNode.transform.position.y,
                            directoryNode.transform.position.z + 15);
            }

            island.transform.localScale = new Vector3(10, 1, 10);

            var renderer = island.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.white);

            return position;
        }

        private void Start()
        {
            root = gameObject.AddComponent<Root>();
            Debug.Log(Application.productName + " started.");
            count = 0;
            var drivePosition = 0;
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                var driveObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                driveObject.transform.parent = transform.parent;
                driveObject.transform.position = new Vector3(0 - drivePosition, 0 , 10);
                driveObject.transform.localScale = new Vector3(1, 1, 1);
                var driveNode = driveObject.AddComponent<DriveNode>();
                driveNode.Assign(drive);
                driveNodes.Add(driveNode);

                var renderer = driveObject.GetComponent<Renderer>();
                if (driveNode.extendedInfo.isAccessDenied)
                {
                    renderer.material.SetColor("_Color", Color.red);
                }
                else
                {
                    renderer.material.SetColor("_Color", Color.gray);
                }

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
                    var directoryNode = hitInfo.transform.GetComponent<DirectoryNode>();
                    if (directoryNode != null && !directoryNode.extendedInfo.isShowingInternal)
                    {
                        directoryNode.Populate(PrimitiveType.Capsule, PrimitiveType.Cube);

                        var islandPosition = new Vector3(0, 0, 0);
                        if (!directoryNode.extendedInfo.isAccessDenied)
                        {
                            islandPosition = createIsland(directoryNode);
                        }

                        count++;

                        var x = -4;
                        var y = 0;
                        var z = 4;
                        foreach (DirectoryNode childDirectoryNode in directoryNode.directoryNodes)
                        {
                            childDirectoryNode.transform.position = new Vector3(
                                islandPosition.x + x,
                                islandPosition.y + y,
                                islandPosition.z + z);

                            x += 1;
                            if (x >= 5)
                            {
                                x = -4;
                                z -= 1;
                            }

                            var renderer = childDirectoryNode.GetComponent<Renderer>();
                            if (childDirectoryNode.extendedInfo.isAccessDenied)
                            {
                                renderer.material.SetColor("_Color", Color.red);
                            }
                            else
                            {
                                var vanillaFolderColor = new Color32(95, 90, 67, 255);
                                renderer.material.SetColor("_Color", vanillaFolderColor);
                            }
                        }

                        foreach (FileNode childFileNode in directoryNode.fileNodes)
                        {
                            childFileNode.transform.position = new Vector3(
                                islandPosition.x + x,
                                islandPosition.y + y,
                                islandPosition.z + z);
                            childFileNode.transform.localScale = new Vector3(1, 2, 1);

                            x += 1;
                            if (x >= 5)
                            {
                                x = -4;
                                z -= 1;
                            }

                            var renderer = childFileNode.GetComponent<Renderer>();
                            if (childFileNode.extendedInfo.isAccessDenied)
                            {
                                renderer.material.SetColor("_Color", Color.red);
                            }
                            else
                            {
                                renderer.material.SetColor("_Color", Color.blue);
                            }
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
                    var driveNode = hitInfo.transform.GetComponent<DriveNode>();
                    var directoryNode = hitInfo.transform.GetComponent<DirectoryNode>();
                    var fileNode = hitInfo.transform.GetComponent<FileNode>();

                    if (driveNode != null)
                    {
                        uiConnector.fileSystemInfo = driveNode.Container;
                        uiConnector.driveInfo = driveNode.BaseContainer;
                    }
                    else if (directoryNode != null)
                    {
                        uiConnector.fileSystemInfo = directoryNode.Container;
                    }
                    else if (fileNode != null)
                    {
                        uiConnector.fileSystemInfo = fileNode.Container;
                    }

                    uiConnector.UpdateUI();
                }
            }
            return;
        }
    }
}
