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
        //[SerializeField]
        //private List<DriveNode> driveNodes;
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
                position = island.transform.position =
                              parentIsland.transform.position + new Vector3(0, 0, 15);
            }
            else
            {
                position = island.transform.position =
                    directoryNode.transform.position + new Vector3(0, 0, 15);
            }

            island.transform.localScale = new Vector3(10, 1, 10);

            // May be changed.
            Camera.main.transform.position =
                island.transform.position + new Vector3(0, 10, -10);

            var renderer = island.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.white);

            return position;
        }

        private void Start()
        {
            Debug.Log(Application.productName + " started.");
            root = gameObject.AddComponent<Root>();
            root.Populate(PrimitiveType.Cylinder);
            count = 0;

            /*var drivePosition = 0;
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                var driveGameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                driveGameObject.transform.parent = transform.parent;
                driveGameObject.transform.position = new Vector3(0 - drivePosition, 0 , 10);
                driveGameObject.transform.localScale = new Vector3(1, 1, 1);
                var driveNode = driveGameObject.AddComponent<DriveNode>();
                driveNode.Assign(drive);
                driveNodes.Add(driveNode);

                var renderer = driveGameObject.GetComponent<Renderer>();
                if (driveNode.extendedInfo.isAccessDenied)
                {
                    renderer.material.SetColor("_Color", Color.red);
                }
                else
                {
                    renderer.material.SetColor("_Color", Color.gray);
                }

                drivePosition += 15;
            }*/

            var islandPosition = new Vector3(0, 0, 0);
            islandPosition = createIsland(root);

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

            // May be changed.
            Camera.main.transform.Rotate(new Vector3(45, 0, 0));

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

                if (Input.GetMouseButtonDown(0))
                {
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

                        /*DirectoryNode nd;
                        if (count >= 1)
                        {
                            nd = dn.gameObject.transform.parent.GetComponent<DirectoryNode>();
                            nd.Depopulate();
                        }*/
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
