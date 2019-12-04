using Gui3dFileSystemNavigationUnity.Data;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public class FileManager : MonoBehaviour
    {
        [SerializeField]
        private CameraConnectorManager cameraConnector;
        private int count;
        private Ray ray;
        private RaycastHit raycastHit;
        [SerializeField]
        private Root root;
        [SerializeField]
        private GameObject selector;
        [SerializeField]
        private FileUIConnectorManager uiConnector;

        private FileManager() : base() { return; }

        private class Root : DirectoryNode
        {
            public List<DriveNode> driveNodes;

            public new ISystemNode<DirectoryInfo> Assign(DirectoryInfo container,
                DirectoryNode parent = null)
            {
                throw new NotSupportedException("This method is not supported for a Root.");
            }
            public override ISystemNode<DirectoryInfo> Grab(string path)
            {
                throw new NotSupportedException("This method is not supported for a Root.");
            }
            public new ISystemNode<DirectoryInfo> Populate()
            {
                var sample = new GameObject();
                Populate(sample, sample);
                Destroy(sample);
                return this;
            }
            [Obsolete("This method is obsolete.")]
            public ISystemNode<DirectoryInfo> Populate(PrimitiveType drivePrimitiveType)
            {
                var sample = GameObject.CreatePrimitive(drivePrimitiveType);
                Populate(sample);
                Destroy(sample);
                return this;
            }
            [Obsolete("This method is obsolete.")]
            public new ISystemNode<DirectoryInfo> Populate(PrimitiveType drivePrimitiveType,
                PrimitiveType notUsed)
            {
                return Populate(drivePrimitiveType);
            }
            public new ISystemNode<DirectoryInfo> Populate(GameObject driveTemplate,
                GameObject notUsed = null)
            {
                driveNodes = new List<DriveNode>();
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    var driveGameObject = Instantiate(driveTemplate);
                    driveGameObject.transform.parent = transform;
                    var driveNode = driveGameObject.AddComponent<DriveNode>();
                    driveNode.Assign(drive, this);
                    driveNodes.Add(driveNode);
                }
                return this;
            }
        }

        private GameObject CreateConnectingRod(DirectoryNode directoryNode)
        {
            var rod = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            rod.transform.parent = directoryNode.parentDirectory.transform;
            rod.transform.name = "Rod of " + directoryNode.parentDirectory.name;

            var parent = directoryNode.parentDirectory;

            if (count >= 1)
            {
                var parentIsland = parent.transform.Find("Island of " + parent.name);
                rod.transform.position =
                              parentIsland.transform.position + new Vector3(0, 0, 8);
            }
            else
            {
                rod.transform.position =
                              directoryNode.transform.position + new Vector3(0, 0, 8);
            }

            rod.transform.localScale = new Vector3(1, 3, 1);
            rod.transform.rotation = Quaternion.Euler(0, 90, 90);

            var rodData = rod.AddComponent<Rod>();
            rodData.currentDirectory = directoryNode.parentDirectory;
            rodData.nextDirectory = directoryNode;

            var renderer = rod.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.gray);

            return rod;
        }
        private GameObject CreateIsland(DirectoryNode directoryNode)
        {
            Debug.Log(count);

            var island = GameObject.CreatePrimitive(PrimitiveType.Cube);
            island.transform.parent = directoryNode.transform;
            island.transform.name = "Island of " + directoryNode.name;

            var parent = directoryNode.parentDirectory;

            if (count >= 1)
            {
                var parentIsland = parent.transform.Find("Island of " + parent.name);
                island.transform.position =
                              parentIsland.transform.position + new Vector3(0, 0, 15);
            }
            else
            {
                island.transform.position =
                    directoryNode.transform.position + new Vector3(0, 0, 15);
            }

            island.transform.localScale = new Vector3(10, 1, 10);

            var renderer = island.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.white);

            return island;
        }
        private void Start()
        {
            Debug.Log(Application.productName + " started.");
            root = gameObject.AddComponent<Root>();
            root.Populate(PrimitiveType.Cylinder);
            count = 0;

            var island = CreateIsland(root);
            var islandPosition = island.transform.position;

            cameraConnector.Transition(island);

            count++;

            var x = -4;
            var y = 0;
            var z = 4;
            foreach (DriveNode childDriveNode in root.driveNodes)
            {
                childDriveNode.transform.position =
                    islandPosition + new Vector3(x, y, z);

                x += 1;
                if (x >= 5)
                {
                    x = -4;
                    z -= 1;
                }

                var childDriveNodeRenderer = childDriveNode.GetComponent<Renderer>();
                if (childDriveNode.extendedInfo.isAccessDenied)
                {
                    childDriveNodeRenderer.material.SetColor("_Color", Color.red);
                }
                else
                {
                    childDriveNodeRenderer.material.SetColor("_Color", Color.gray);
                }
            }

            var selectorRenderer = selector.GetComponent<Renderer>();
            selectorRenderer.material.SetColor("_Color", Color.yellow);

            return;
        }

        private void Update()
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = Physics.Raycast(ray, out raycastHit);
            if (hit)
            {
                var fileNode = raycastHit.transform.GetComponent<FileNode>();
                var directoryNode = raycastHit.transform.GetComponent<DirectoryNode>();
                var driveNode = raycastHit.transform.GetComponent<DriveNode>();
                var rodData = raycastHit.transform.GetComponent<Rod>();

                if (Input.GetMouseButtonDown(0))
                {
                    if (directoryNode != null && !directoryNode.extendedInfo.isShowingInternal)
                    {
                        directoryNode.Populate(PrimitiveType.Capsule, PrimitiveType.Cube);

                        if (!directoryNode.extendedInfo.isAccessDenied)
                        {
                            var island = CreateIsland(directoryNode);
                            var islandPosition = island.transform.position;
                            CreateConnectingRod(directoryNode);

                            cameraConnector.Transition(island);

                            count++;

                            var x = -4;
                            var y = 0;
                            var z = 4;
                            foreach (DirectoryNode childDirectoryNode in directoryNode.directoryNodes)
                            {
                                childDirectoryNode.transform.position =
                                    islandPosition + new Vector3(x, y, z);

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
                                childFileNode.transform.position =
                                    islandPosition + new Vector3(x, y, z);
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
                        }
                    }
                    else if (rodData != null)
                    {
                        var parent = rodData.currentDirectory;
                        var parentIsland = parent.transform.Find("Island of "
                            + parent.name);
                        cameraConnector.Transition(parentIsland);
                    }
                }

                else if (Input.GetMouseButtonDown(1))
                {
                    if (driveNode != null)
                    {
                        Debug.Log("Sending " + driveNode.name + " to UI.");
                        uiConnector.ExecuteUI(driveNode);
                    }
                    else if (directoryNode != null)
                    {
                        Debug.Log("Sending " + directoryNode.name + " to UI.");
                        uiConnector.ExecuteUI(directoryNode);
                    }
                    else if (fileNode != null)
                    {
                        Debug.Log("Sending " + fileNode.name + " to UI.");
                        uiConnector.ExecuteUI(fileNode);
                    }
                }
                else
                {
                    if (driveNode != null
                        || directoryNode != null
                        || fileNode != null)
                    {
                        selector.SetActive(true);
                        selector.transform.position =
                            raycastHit.transform.position + new Vector3(0, 0, -0.5f);
                    }
                    else
                    {
                        selector.SetActive(false);
                    }
                }
            }
            else
            {
                selector.SetActive(false);
            }

            return;
        }
    }
}
