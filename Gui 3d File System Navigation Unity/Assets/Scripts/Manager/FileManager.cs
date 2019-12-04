using Gui3dFileSystemNavigationUnity.Data;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public class FileManager : MonoBehaviour
    {
        [SerializeField]
        private CameraConnectorManager cameraConnector;
        [SerializeField]
        private CurrentDirectoryUIConnectorManager currentDirectoryUIConnector;
        private int count;
        [SerializeField]
        private FileIconDatabase fileIconDatabase;
        [SerializeField]
        private NodeHoverUIConnectorManager nodeHoverUIConnector;
        [SerializeField]
        private NodePropertiesUIConnectorManager nodePropertiesUIConnector;
        private Ray ray;
        private RaycastHit raycastHit;
        [SerializeField]
        private Rod rodHover;
        [SerializeField]
        private RootNode root;
        [SerializeField]
        private GameObject selector;

        private FileManager() : base() { return; }

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

            var rodRenderer = rod.GetComponent<Renderer>();
            rodRenderer.material.SetColor("_Color", Color.gray);

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

            var islandRenderer = island.GetComponent<Renderer>();
            islandRenderer.material.SetColor("_Color", Color.white);

            return island;
        }
        private void Start()
        {
            Debug.Log(Application.productName + " started.");
            root = gameObject.AddComponent<RootNode>();
            root.fileIconDatabase = fileIconDatabase;
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
                var rod = raycastHit.transform.GetComponent<Rod>();

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
                            currentDirectoryUIConnector.ExecuteUI(directoryNode);

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

                                var childDirectoryNodeRenderer = childDirectoryNode.GetComponent<Renderer>();
                                if (childDirectoryNode.extendedInfo.isAccessDenied)
                                {
                                    childDirectoryNodeRenderer.material.SetColor("_Color", Color.red);
                                }
                                else
                                {
                                    var vanillaFolderColor = new Color32(95, 90, 67, 255);
                                    childDirectoryNodeRenderer.material.SetColor("_Color", vanillaFolderColor);
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

                                var childFileNodeRenderer = childFileNode.GetComponent<Renderer>();
                                if (childFileNode.extendedInfo.isAccessDenied)
                                {
                                    childFileNodeRenderer.material.SetColor("_Color", Color.red);
                                }
                                else
                                {
                                    childFileNodeRenderer.material.SetColor("_Color", Color.blue);
                                }
                            }
                        }
                    }
                    else if (rod != null)
                    {
                        var parent = rod.currentDirectory;
                        var parentIsland = parent.transform.Find("Island of "
                            + parent.name);
                        cameraConnector.Transition(parentIsland);
                        if (parent.Container != null)
                        {
                            currentDirectoryUIConnector.ExecuteUI(parent);
                        }
                        else
                        {
                            currentDirectoryUIConnector.Clear();
                        }

                        var next = rod.nextDirectory;
                        var nextIsland = next.transform.Find("Island of "
                            + next.name);
                        if (nextIsland != null)
                        {
                            Destroy(nextIsland.gameObject);
                        }
                        next.Depopulate();
                        Destroy(rod.gameObject);
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    if (driveNode != null)
                    {
                        Debug.Log("Sending " + driveNode.name + " to UI.");
                        nodePropertiesUIConnector.ExecuteUI(driveNode);
                    }
                    else if (directoryNode != null)
                    {
                        Debug.Log("Sending " + directoryNode.name + " to UI.");
                        nodePropertiesUIConnector.ExecuteUI(directoryNode);
                    }
                    else if (fileNode != null)
                    {
                        Debug.Log("Sending " + fileNode.name + " to UI.");
                        nodePropertiesUIConnector.ExecuteUI(fileNode);
                    }
                }
                else
                {
                    if (driveNode != null)
                    {
                        selector.SetActive(true);
                        selector.transform.position =
                            raycastHit.transform.position;
                        nodeHoverUIConnector.ExecuteUI(driveNode);
                    }
                    else if (directoryNode != null)
                    {
                        selector.SetActive(true);
                        selector.transform.position =
                            raycastHit.transform.position;
                        nodeHoverUIConnector.ExecuteUI(directoryNode);
                    }
                    else if(fileNode != null)
                    {
                        selector.SetActive(true);
                        selector.transform.position =
                            raycastHit.transform.position;
                        nodeHoverUIConnector.ExecuteUI(fileNode);
                    }
                    else if (rod != null)
                    {
                        var rodRenderer = rod.GetComponent<Renderer>();
                        rodRenderer.material.SetColor("_Color", Color.yellow);
                        rodHover = rod;
                        if (rod.currentDirectory.Container != null)
                        {
                            nodeHoverUIConnector.ExecuteUI(rod.currentDirectory);
                        }
                        else
                        {
                            nodeHoverUIConnector.Clear();
                        }
                    }
                    else
                    {
                        nodeHoverUIConnector.Clear();
                        selector.SetActive(false);
                    }
                }
            }
            else
            {
                nodeHoverUIConnector.Clear();
                selector.SetActive(false);
                if (rodHover)
                {
                    var rodHoverRenderer = rodHover.GetComponent<Renderer>();
                    rodHoverRenderer.material.SetColor("_Color", Color.grey);
                    rodHover = null;
                }
            }

            return;
        }
    }
}
