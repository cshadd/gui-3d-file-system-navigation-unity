using Gui3dFileSystemNavigationUnity.Data;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public class FileManager : MonoBehaviour
    {
        private const int MaxIslandItemNumber = 3;

        [SerializeField]
        private Arrow arrowHover;
        [SerializeField]
        private CameraConnectorManager cameraConnector;
        [SerializeField]
        private CurrentDirectoryUIConnectorManager currentDirectoryUIConnector;
        [SerializeField]
        private int count;
        [SerializeField]
        private Island currentIsland;
        [SerializeField]
        private Rod currentRod;
        [SerializeField]
        private IconDatabase iconDatabase;
        [SerializeField]
        private int itemCounter;
        [SerializeField]
        private NodeHoverUIConnectorManager nodeHoverUIConnector;
        [SerializeField]
        private NodePropertiesUIConnectorManager nodePropertiesUIConnector;
        [SerializeField]
        private Ray ray;
        private RaycastHit raycastHit;
        [SerializeField]
        private Rod rodHover;
        [SerializeField]
        private RootNode root;
        [SerializeField]
        private GameObject selector;

        private FileManager() : base() { return; }

        private GameObject CreateArrow(DirectoryNode directoryNode, ArrowDirection direction)
        {
            var arrow = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            arrow.transform.parent = directoryNode.transform;
            arrow.transform.name = direction + " Arrow of " + directoryNode.name;

            var island = directoryNode.transform.Find("Island of " + directoryNode.name);
            if (direction == ArrowDirection.Left)
            {
                arrow.transform.position =
                  island.transform.position + new Vector3(-4, 2, 5);
            }
            else if (direction == ArrowDirection.Right)
            {
                arrow.transform.position =
                    island.transform.position + new Vector3(4, 2, 5);
            }


            var arrowData = arrow.AddComponent<Arrow>();
            arrowData.currentDirectory = directoryNode;
            arrowData.direction = direction;

            var arrowRenderer = arrow.GetComponent<Renderer>();
            arrowRenderer.material.SetColor("_Color", Color.gray);

            return arrow;
        }
        private GameObject CreateConnectingRod(DirectoryNode directoryNode)
        {
            var rod = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            rod.transform.parent = directoryNode.transform;
            rod.transform.name = "Rod of " + directoryNode.name;

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
            rodData.currentDirectory = directoryNode;
            rodData.parentDirectory = directoryNode.parentDirectory;
            currentRod = rodData;

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

            var islandData = island.AddComponent<Island>();
            islandData.currentDirectory = directoryNode;
            currentIsland = islandData;

            var islandRenderer = island.GetComponent<Renderer>();
            islandRenderer.material.SetColor("_Color", Color.white);

            return island;
        }
        public void Exit()
        {
            Application.Quit();
        }
        public void OpenDirectory(DirectoryNode directoryNode)
        {
            var currentIslandItemNumber = 0;
            var x = -4;
            var y = 0;
            var z = 4;

            

            foreach (DirectoryNode childDirectoryNode in directoryNode.parentDirectory.directoryNodes)
            {
                childDirectoryNode.Depopulate();
                var childDirectoryNodeIsland = childDirectoryNode.transform.Find("Island of "
                    + childDirectoryNode.name);
                var childDirectoryNodeRod = childDirectoryNode.transform.Find("Rod of "
                    + childDirectoryNode.name);
                var childDirectoryNodeLeftArrow = childDirectoryNode.transform.Find(ArrowDirection.Left + " Arrow of "
                    + childDirectoryNode.name);
                var childDirectoryNodeRightArrow = childDirectoryNode.transform.Find(ArrowDirection.Right + " Arrow of "
                    + childDirectoryNode.name);
                if (childDirectoryNodeIsland != null)
                {
                    Destroy(childDirectoryNodeIsland.gameObject);
                }
                if (childDirectoryNodeRod != null)
                {
                    Destroy(childDirectoryNodeRod.gameObject);
                }
                if (childDirectoryNodeLeftArrow != null)
                {
                    Destroy(childDirectoryNodeLeftArrow.gameObject);
                }
                if (childDirectoryNodeRightArrow != null)
                {
                    Destroy(childDirectoryNodeRightArrow.gameObject);
                }
            }
            directoryNode.Populate(PrimitiveType.Capsule, PrimitiveType.Cube);
            if (!directoryNode.extendedInfo.isAccessDenied)
            {
                var island = CreateIsland(directoryNode);
                var currentIslandPosition = island.transform.position;
                var pageData = island.GetComponent<Island>();
                CreateConnectingRod(directoryNode);
                
                cameraConnector.Transition(island);
                currentDirectoryUIConnector.ExecuteUI(directoryNode);
                foreach (DirectoryNode childDirectoryNode in directoryNode.directoryNodes)
                {
                    if (currentIslandItemNumber < MaxIslandItemNumber)
                    {
                        pageData.pageItemCounter++;
                        currentIslandItemNumber++;
                        childDirectoryNode.transform.position =
                            currentIslandPosition + new Vector3(x, y, z);

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
                    else
                    {
                        childDirectoryNode.transform.gameObject.SetActive(false);
                    }
                }
                foreach (FileNode childFileNode in directoryNode.fileNodes)
                {
                    if (currentIslandItemNumber < MaxIslandItemNumber)
                    {
                        pageData.pageItemCounter++;
                        currentIslandItemNumber++;
                        childFileNode.transform.position =
                        currentIslandPosition + new Vector3(x, y, z);
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
                    else
                    {
                        childFileNode.transform.gameObject.SetActive(false);
                    }
                }

                if (currentIslandItemNumber >= MaxIslandItemNumber)
                {
                    CreateArrow(directoryNode, ArrowDirection.Left);
                    CreateArrow(directoryNode, ArrowDirection.Right);
                    pageData.pageNumber++;
                }
            }
            return;
        }
        public void ResetCamera()
        {
            cameraConnector.Transition(currentIsland.gameObject);
            return;
        }
        private void Start()
        {
            Debug.Log(Application.productName + " started.");

            count = 0;

            nodePropertiesUIConnector.gameObject.SetActive(false);

            var rootGameObject = new GameObject
            {
                name = "ROOT"
            };
            rootGameObject.transform.parent = transform;
            root = rootGameObject.AddComponent<RootNode>();
            root.iconDatabase = iconDatabase;
            root.Populate(PrimitiveType.Cylinder);

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
                var raycastHitTransform = raycastHit.transform;
                var arrowData = raycastHitTransform.GetComponent<Arrow>();
                var fileNode = raycastHitTransform.GetComponent<FileNode>();
                var directoryNode = raycastHitTransform.GetComponent<DirectoryNode>();
                var driveNode = raycastHitTransform.GetComponent<DriveNode>();
                var islandData = raycastHitTransform.GetComponent<Island>();
                var rodData = raycastHitTransform.GetComponent<Rod>();

                if (Input.GetMouseButtonDown(0) && !nodePropertiesUIConnector.gameObject.activeInHierarchy)
                {
                    if (directoryNode != null
                        && !directoryNode.extendedInfo.isShowingInternal)
                    {
                        OpenDirectory(directoryNode);
                    }
                    else if (rodData != null)
                    {
                        var parent = rodData.parentDirectory;
                        var parentIsland = parent.transform.Find("Island of "
                            + parent.name);
                        var parentIslandData = parentIsland.GetComponent<Island>();
                        currentIsland = parentIslandData;

                        var parentRod = parent.transform.Find("Rod of "
                            + parent.name);
                        if (parentRod != null)
                        {
                            var parentRodData = parentRod.GetComponent<Rod>();
                            currentRod = parentRodData;
                        }
                        else
                        {
                            currentRod = null;
                        }
                        rodHover = null;
                        cameraConnector.Transition(parentIsland);
                        if (parent.Container != null)
                        {
                            currentDirectoryUIConnector.ExecuteUI(parent);
                        }
                        else
                        {
                            currentDirectoryUIConnector.Clear();
                        }

                        var next = rodData.currentDirectory;
                        var nextIsland = next.transform.Find("Island of "
                            + next.name);
                        var nextLeftArrow = next.transform.Find(ArrowDirection.Left + " Arrow of "
                            + next.name);
                        var nextRightArrow = next.transform.Find(ArrowDirection.Right + " Arrow of "
                            + next.name);
                        if (nextIsland != null)
                        {
                            Destroy(nextIsland.gameObject);
                        }
                        if (nextLeftArrow != null)
                        {
                            Destroy(nextLeftArrow.gameObject);
                        }
                        if (nextRightArrow != null)
                        {
                            Destroy(nextRightArrow.gameObject);
                        }
                        next.Depopulate();

                        Destroy(rodData.gameObject);
                    }
                    else if (arrowData != null)
                    {
                        var island = arrowData.currentDirectory.transform.Find("Island of " + arrowData.currentDirectory.name);
                        var currentIslandPosition = island.position;
                        var pageData = island.GetComponent<Island>();
                        var x = -4;
                        var y = 0;
                        var z = 4;
                        int activeCounter = 0;

                        if (arrowData.direction == ArrowDirection.Right)
                        {
                            // if (pageData.pageItemCounter <= arrowData.currentDirectory.directoryNodes.Count+arrowData.currentDirectory.fileNodes.Count)
                            Debug.LogWarning(arrowData.currentDirectory.directoryNodes.Count+arrowData.currentDirectory.fileNodes.Count);

                                pageData.pageNumber++;
                            foreach (DirectoryNode childDirectoryNode in arrowData.currentDirectory.directoryNodes)
                            {
                                activeCounter++;
                                if (pageData.pageItemCounter <= arrowData.currentDirectory.directoryNodes.Count + arrowData.currentDirectory.fileNodes.Count)
                                {

                                    if ((activeCounter <= pageData.pageItemCounter))
                                    {
                                        childDirectoryNode.transform.gameObject.SetActive(false);
                                    }
                                    else if ((activeCounter >= pageData.pageItemCounter) && (pageData.pageItemCounter < (pageData.pageNumber) * MaxIslandItemNumber))
                                    {
                                        childDirectoryNode.transform.gameObject.SetActive(true);
                                        pageData.pageItemCounter++;
                                        childDirectoryNode.transform.position =
                                             currentIslandPosition + new Vector3(x, y, z);

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
                                    else
                                    {
                                        childDirectoryNode.transform.gameObject.SetActive(false);
                                    }
                                }
                            }
                            foreach (FileNode childFileNode in arrowData.currentDirectory.fileNodes)
                            {

                                activeCounter++;
                                if (pageData.pageItemCounter <= arrowData.currentDirectory.directoryNodes.Count + arrowData.currentDirectory.fileNodes.Count)
                                {

                                    if ((activeCounter <= pageData.pageItemCounter))
                                    {
                                        childFileNode.transform.gameObject.SetActive(false);
                                    }
                                    else if ((activeCounter >= pageData.pageItemCounter) && (pageData.pageItemCounter < (pageData.pageNumber) * MaxIslandItemNumber))
                                    {
                                        childFileNode.transform.gameObject.SetActive(true);
                                        pageData.pageItemCounter++;
                                        childFileNode.transform.position =
                                             currentIslandPosition + new Vector3(x, y, z);
                                        childFileNode.transform.localScale = new Vector3(1, 2, 1);

                                        x += 1;
                                        if (x >= 5)
                                        {
                                            x = -4;
                                            z -= 1;
                                        }

                                        var childDirectoryNodeRenderer = childFileNode.GetComponent<Renderer>();
                                        if (childFileNode.extendedInfo.isAccessDenied)
                                        {
                                            childDirectoryNodeRenderer.material.SetColor("_Color", Color.red);
                                        }
                                        else
                                        {
                                            var vanillaFolderColor = new Color32(95, 90, 67, 255);
                                            childDirectoryNodeRenderer.material.SetColor("_Color", vanillaFolderColor);
                                        }
                                    }
                                    else
                                    {
                                        childFileNode.transform.gameObject.SetActive(false);
                                    }

                                }
                            }
                        }
                        else if (arrowData.direction == ArrowDirection.Left)
                        {
                            if (pageData.pageItemCounter > MaxIslandItemNumber)
                            {
                                pageData.pageNumber--;
                                foreach (DirectoryNode childDirectoryNode in arrowData.currentDirectory.directoryNodes)
                                {
                                    activeCounter++;
                                    if ((activeCounter < (pageData.pageItemCounter - (pageData.pageNumber * MaxIslandItemNumber)) - MaxIslandItemNumber))
                                    {
                                        childDirectoryNode.transform.gameObject.SetActive(false);
                                    }
                                    else if (((activeCounter > (pageData.pageNumber * MaxIslandItemNumber) - MaxIslandItemNumber)) && (activeCounter <= pageData.pageItemCounter))
                                    {
                                        childDirectoryNode.transform.gameObject.SetActive(true);
                                        pageData.pageItemCounter--;
                                        childDirectoryNode.transform.position =
                                             currentIslandPosition + new Vector3(x, y, z);

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
                                    else
                                    {
                                        childDirectoryNode.transform.gameObject.SetActive(false);
                                    }
                                }

                                foreach (FileNode childFileNode in arrowData.currentDirectory.fileNodes)
                                {
                                    activeCounter++;
                                    if ((activeCounter < (pageData.pageItemCounter - (pageData.pageNumber * MaxIslandItemNumber)) - MaxIslandItemNumber))
                                    {
                                        childFileNode.transform.gameObject.SetActive(false);
                                    }
                                    else if (((activeCounter > (pageData.pageNumber * MaxIslandItemNumber) - MaxIslandItemNumber)) && (activeCounter <= pageData.pageItemCounter))
                                    {
                                        childFileNode.transform.gameObject.SetActive(true);
                                        pageData.pageItemCounter--;
                                        childFileNode.transform.position =
                                        currentIslandPosition + new Vector3(x, y, z);
                                        childFileNode.transform.localScale = new Vector3(1, 2, 1);

                                        x += 1;
                                        if (x >= 5)
                                        {
                                            x = -4;
                                            z -= 1;
                                        }

                                        var childDirectoryNodeRenderer = childFileNode.GetComponent<Renderer>();
                                        if (childFileNode.extendedInfo.isAccessDenied)
                                        {
                                            childDirectoryNodeRenderer.material.SetColor("_Color", Color.red);
                                        }
                                        else
                                        {
                                            var vanillaFolderColor = new Color32(95, 90, 67, 255);
                                            childDirectoryNodeRenderer.material.SetColor("_Color", vanillaFolderColor);
                                        }
                                    }
                                    else
                                    {
                                        childFileNode.transform.gameObject.SetActive(false);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(1) && !nodePropertiesUIConnector.gameObject.activeInHierarchy)
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
                    else if (islandData != null
                        && islandData.currentDirectory.Container != null)
                    {
                        var islandDirectoryNode = islandData.currentDirectory;
                        Debug.Log("Sending " + islandDirectoryNode.name + " to UI.");
                        nodePropertiesUIConnector.ExecuteUI(islandDirectoryNode);
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
                    else if (arrowData != null)
                    {
                        var arrowRenderer = arrowData.GetComponent<Renderer>();
                        arrowRenderer.material.SetColor("_Color", Color.yellow);
                        arrowHover = arrowData;
                    }
                    else if (rodData != null)
                    {
                        var rodRenderer = rodData.GetComponent<Renderer>();
                        rodRenderer.material.SetColor("_Color", Color.yellow);
                        rodHover = rodData;
                        if (rodData.parentDirectory.Container != null)
                        {
                            nodeHoverUIConnector.ExecuteUI(rodData.parentDirectory);
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
                        if (rodHover)
                        {
                            var rodHoverRenderer = rodHover.GetComponent<Renderer>();
                            rodHoverRenderer.material.SetColor("_Color", Color.grey);
                            rodHover = null;
                        }
                    }
                }
            }
            else
            {
                nodeHoverUIConnector.Clear();
                selector.SetActive(false);
                if (arrowHover != null)
                {
                    var arrowRenderer = arrowHover.GetComponent<Renderer>();
                    arrowRenderer.material.SetColor("_Color", Color.grey);
                    arrowHover = null;
                }
                if (rodHover != null)
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
