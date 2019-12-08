using Gui3dFileSystemNavigationUnity.Data;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public class NodePropertiesUIConnectorManager : AbstractUIConnectorManager
    {
        [SerializeField]
        private Button buttonCreateFile;
        [SerializeField]
        private Button buttonCreateFileExecute;
        [SerializeField]
        private Button buttonCreateFolder;
        [SerializeField]
        private Button buttonCreateFolderExecute;
        [SerializeField]
        private Button buttonDelete;
        [SerializeField]
        private Button buttonOpen;
        [SerializeField]
        private Button buttonRename;
        [SerializeField]
        private Button buttonRenameExecute;
        [SerializeField]
        private Button buttonSaveContent;
        [SerializeField]
        private FileManager fileManager;
        [SerializeField]
        private Image imageIcon;
        [SerializeField]
        private InputField inputFieldContentEditor;
        [SerializeField]
        private InputField inputFieldEditor;
        [SerializeField]
        private ScrollRect scrollRectContent;
        [SerializeField]
        private DriveNode selectedDriveNode;
        [SerializeField]
        private DirectoryNode selectedDirectoryNode;
        [SerializeField]
        private FileNode selectedFileNode;
        [SerializeField]
        private Text textContent;

        private NodePropertiesUIConnectorManager() : base() { return; }

        public override void ExecuteUI(DirectoryNode directoryNode)
        {
            base.ExecuteUI<DirectoryInfo>(directoryNode);
            selectedDirectoryNode = directoryNode;
            selectedDriveNode = null;
            selectedFileNode = null;
            if (directoryNode.extendedInfo.isAccessDenied)
            {
                buttonCreateFile.gameObject.SetActive(false);
                buttonCreateFolder.gameObject.SetActive(false);
            }
            else
            {
                buttonCreateFile.gameObject.SetActive(true);
                buttonCreateFolder.gameObject.SetActive(true);
            }
            ExecuteUI<DirectoryInfo>(directoryNode);
            return;
        }
        public override void ExecuteUI(DriveNode driveNode)
        {
            base.ExecuteUI<DirectoryInfo>(driveNode);
            selectedDirectoryNode = driveNode;
            selectedDriveNode = driveNode;
            selectedFileNode = null;
            if (driveNode.extendedInfo.isAccessDenied)
            {
                buttonCreateFile.gameObject.SetActive(false);
                buttonCreateFolder.gameObject.SetActive(false);
            }
            else
            {
                buttonCreateFile.gameObject.SetActive(true);
                buttonCreateFolder.gameObject.SetActive(true);
            }
            ExecuteUI<DirectoryInfo>(driveNode);
            return;
        }
        public override void ExecuteUI(FileNode fileNode)
        {
            base.ExecuteUI<FileInfo>(fileNode);
            selectedDirectoryNode = null;
            selectedDriveNode = null;
            selectedFileNode = fileNode;
            buttonCreateFile.gameObject.SetActive(false);
            buttonCreateFolder.gameObject.SetActive(false);
            ExecuteUI<FileInfo>(fileNode);
            return;
        }
        public override void ExecuteUI<T>(AbstractSystemNode<T> node)
        {
            base.ExecuteUI(node);
            imageIcon.sprite = node.extendedInfo.fileIcon;
            HandleGeneral();

            if (node.extendedInfo.isAccessDenied)
            {
                buttonDelete.gameObject.SetActive(false);
                buttonOpen.gameObject.SetActive(false);
                buttonRename.gameObject.SetActive(false);
            }
            else
            {
                buttonDelete.gameObject.SetActive(true);
                buttonOpen.gameObject.SetActive(true);
                buttonRename.gameObject.SetActive(true);
            }
            return;
        }
        public void HandleCreateFile()
        {
            buttonCreateFileExecute.gameObject.SetActive(true);
            buttonCreateFolderExecute.gameObject.SetActive(false);
            buttonRenameExecute.gameObject.SetActive(false);
            buttonSaveContent.gameObject.SetActive(false);
            imageIcon.gameObject.SetActive(false);
            inputFieldContentEditor.gameObject.SetActive(false);
            inputFieldEditor.gameObject.SetActive(true);
            scrollRectContent.gameObject.SetActive(false);
            textContent.gameObject.SetActive(false);

            inputFieldContentEditor.text = "";
            return;
        }
        public void HandleCreateFileExecute()
        {
            if (selectedDirectoryNode != null)
            {
                var directoryPath = selectedDirectoryNode.Container.FullName;
                var path = Path.Combine(directoryPath, inputFieldEditor.text);
                if (!File.Exists(path))
                {
                    try
                    {
                        var fs = File.CreateText(path);
                        fs.Write("");
                        fs.Close();
                    }
                    catch (IOException ex)
                    {
                        Debug.LogWarning("SystemNode had IOException (caught): " + ex);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Debug.LogWarning("SystemNode had UnauthorizedAccessException (caught): " + ex);
                    }
                }
                fileManager.OpenDirectory(selectedDirectoryNode);
            }
            panelContainer.gameObject.SetActive(false);
            return;
        }
        public void HandleCreateFolder()
        {
            buttonCreateFileExecute.gameObject.SetActive(false);
            buttonCreateFolderExecute.gameObject.SetActive(true);
            buttonRenameExecute.gameObject.SetActive(false);
            buttonSaveContent.gameObject.SetActive(false);
            imageIcon.gameObject.SetActive(false);
            inputFieldContentEditor.gameObject.SetActive(false);
            inputFieldEditor.gameObject.SetActive(true);
            scrollRectContent.gameObject.SetActive(false);
            textContent.gameObject.SetActive(false);

            inputFieldContentEditor.text = "";
            return;
        }
        public void HandleCreateFolderExecute()
        {
            if (selectedDirectoryNode != null)
            {
                var directoryPath = selectedDirectoryNode.Container.FullName;
                var path = Path.Combine(directoryPath, inputFieldEditor.text);
                if (!Directory.Exists(path))
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                    }
                    catch (IOException ex)
                    {
                        Debug.LogWarning("SystemNode had IOException (caught): " + ex);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Debug.LogWarning("SystemNode had UnauthorizedAccessException (caught): " + ex);
                    }
                    fileManager.OpenDirectory(selectedDirectoryNode);
                }
            }
            panelContainer.gameObject.SetActive(false);
            return;
        }
        public void HandleDelete()
        {
            if (selectedDirectoryNode != null)
            {
                var directoryPath = selectedDirectoryNode.Container.FullName;
                var newDirectory = selectedDirectoryNode.parentDirectory;
                if (Directory.Exists(directoryPath))
                {
                    try
                    {
                        Directory.Delete(directoryPath);
                    }
                    catch (IOException ex)
                    {
                        Debug.LogWarning("SystemNode had IOException (caught): " + ex);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Debug.LogWarning("SystemNode had UnauthorizedAccessException (caught): " + ex);
                    }
                    fileManager.OpenDirectory(newDirectory);
                }
            }
            else if (selectedFileNode != null)
            {
                var filePath = selectedFileNode.Container.FullName;
                if (File.Exists(filePath))
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (IOException ex)
                    {
                        Debug.LogWarning("SystemNode had IOException (caught): " + ex);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Debug.LogWarning("SystemNode had UnauthorizedAccessException (caught): " + ex);
                    }
                    panelContainer.gameObject.SetActive(false);
                    fileManager.OpenDirectory(selectedFileNode.parentDirectory);
                }
            }
            panelContainer.gameObject.SetActive(false);
            return;
        }
        public void HandleDetails()
        {
            buttonCreateFileExecute.gameObject.SetActive(false);
            buttonCreateFolderExecute.gameObject.SetActive(false);
            buttonRenameExecute.gameObject.SetActive(false);
            buttonSaveContent.gameObject.SetActive(false);
            imageIcon.gameObject.SetActive(true);
            inputFieldContentEditor.gameObject.SetActive(false);
            inputFieldEditor.gameObject.SetActive(false);
            scrollRectContent.gameObject.SetActive(true);
            textContent.gameObject.SetActive(true);

            if (selectedDriveNode != null)
            {
                var baseContainer = selectedDriveNode.BaseContainer;
                var container = selectedDriveNode.Container;
                textContent.text = "<b>Name: </b>" + container.Name + "\n" +
                               "<b>Type of File: </b>" + container.Extension + "\n" +
                               "<b>Folder Path: </b>" + container.GetType() + "\n" + //I need this
                               "<b>Size: </b>" + container.Attributes + "\n" + //I need this
                               "<b>Date Created: </b>" + container.CreationTime + "\n" +
                               "<b>Date Modified: </b>" + container.LastWriteTime + "\n" +
                               "<b>Attributes: </b>" + container.Attributes + "\n" +
                               "<b>Owner: </b>" + container.LastWriteTime + "\n" + //I need this
                               "<b>Computer: </b>" + container.Attributes; //I need this
            }
            else if (selectedDirectoryNode != null)
            {
                var container = selectedDirectoryNode.Container;
                textContent.text = "<b>Name: </b>" + container.Name + "\n" +
                               "<b>Type of File: </b>" + container.Extension + "\n" +
                               "<b>Folder Path: </b>" + container.GetType() + "\n" + //I need this
                               "<b>Size: </b>" + container.Attributes + "\n" + //I need this
                               "<b>Date Created: </b>" + container.CreationTime + "\n" +
                               "<b>Date Modified: </b>" + container.LastWriteTime + "\n" +
                               "<b>Attributes: </b>" + container.Attributes + "\n" +
                               "<b>Owner: </b>" + container.LastWriteTime + "\n" + //I need this
                               "<b>Computer: </b>" + container.Attributes; //I need this
            }
            else if (selectedFileNode != null)
            {
                var container = selectedFileNode.Container;
                textContent.text = "<b>Name: </b>" + container.Name + "\n" +
                               "<b>Type of File: </b>" + container.Extension + "\n" +
                               "<b>Folder Path: </b>" + container.GetType() + "\n" + //I need this
                               "<b>Size: </b>" + container.Attributes + "\n" + //I need this
                               "<b>Date Created: </b>" + container.CreationTime + "\n" +
                               "<b>Date Modified: </b>" + container.LastWriteTime + "\n" +
                               "<b>Attributes: </b>" + container.Attributes + "\n" +
                               "<b>Owner: </b>" + container.LastWriteTime + "\n" + //I need this
                               "<b>Computer: </b>" + container.Attributes; //I need this
            }
            return;
        }
        public void HandleGeneral()
        {
            buttonCreateFileExecute.gameObject.SetActive(false);
            buttonCreateFolderExecute.gameObject.SetActive(false);
            buttonRenameExecute.gameObject.SetActive(false);
            buttonSaveContent.gameObject.SetActive(false);
            imageIcon.gameObject.SetActive(true);
            inputFieldContentEditor.gameObject.SetActive(false);
            inputFieldEditor.gameObject.SetActive(false);
            scrollRectContent.gameObject.SetActive(true);
            textContent.gameObject.SetActive(true);

            if (selectedDriveNode != null)
            {
                var baseContainer = selectedDriveNode.BaseContainer;
                var container = selectedDriveNode.Container;
                textContent.text = "<b>Name: </b>" + container.Name + "\n" +
                               "<b>---------------------</b>" + "\n" +
                               "<b>Type of File: </b>" + container.Extension + "\n" +
                               "<b>Description: </b>" + container.GetType() + "\n" +
                               "<b>---------------------</b>" + "\n" +
                               "<b>Location: </b>" + container.Attributes + "\n" + //I need this
                               "<b>Size: </b>" + container.Attributes + "\n" + //I need this
                               "<b>Size on Disk: </b>" + container.Attributes + "\n" + //I need this
                               "<b>---------------------</b>" + "\n" +
                               "<b>Date Created: </b>" + container.CreationTime + "\n" +
                               "<b>Date Modified: </b>" + container.LastWriteTime + "\n" +
                               "<b>Date Accessed: </b>" + container.LastAccessTime;
            }
            if (selectedDirectoryNode != null)
            {
                var container = selectedDirectoryNode.Container;
                textContent.text = "<b>Name: </b>" + container.Name + "\n" +
                               "<b>---------------------</b>" + "\n" +
                               "<b>Type of File: </b>" + container.Extension + "\n" +
                               "<b>Description: </b>" + container.GetType() + "\n" +
                               "<b>---------------------</b>" + "\n" +
                               "<b>Location: </b>" + container.Attributes + "\n" + //I need this
                               "<b>Size: </b>" + container.Attributes + "\n" + //I need this
                               "<b>Size on Disk: </b>" + container.Attributes + "\n" + //I need this
                               "<b>---------------------</b>" + "\n" +
                               "<b>Date Created: </b>" + container.CreationTime + "\n" +
                               "<b>Date Modified: </b>" + container.LastWriteTime + "\n" +
                               "<b>Date Accessed: </b>" + container.LastAccessTime;
            }
            else if (selectedFileNode != null)
            {
                var container = selectedFileNode.Container;
                textContent.text = "<b>Name: </b>" + container.Name + "\n" +
                               "<b>---------------------</b>" + "\n" +
                               "<b>Type of File: </b>" + container.Extension + "\n" +
                               "<b>Description: </b>" + container.GetType() + "\n" +
                               "<b>---------------------</b>" + "\n" +
                               "<b>Location: </b>" + container.Attributes + "\n" + //I need this
                               "<b>Size: </b>" + container.Attributes + "\n" + //I need this
                               "<b>Size on Disk: </b>" + container.Attributes + "\n" + //I need this
                               "<b>---------------------</b>" + "\n" +
                               "<b>Date Created: </b>" + container.CreationTime + "\n" +
                               "<b>Date Modified: </b>" + container.LastWriteTime + "\n" +
                               "<b>Date Accessed: </b>" + container.LastAccessTime;
            }
            return;
        }
        public void HandleOpen()
        {
            buttonCreateFileExecute.gameObject.SetActive(false);
            buttonCreateFolderExecute.gameObject.SetActive(false);
            buttonRenameExecute.gameObject.SetActive(false);
            buttonSaveContent.gameObject.SetActive(true);
            imageIcon.gameObject.SetActive(true);
            inputFieldContentEditor.gameObject.SetActive(true);
            inputFieldEditor.gameObject.SetActive(false);
            scrollRectContent.gameObject.SetActive(true);
            textContent.gameObject.SetActive(false);

            if (selectedDirectoryNode != null)
            {
                fileManager.OpenDirectory(selectedDirectoryNode);
                panelContainer.gameObject.SetActive(false);
            }
            else if (selectedFileNode != null)
            {
                var filePath = selectedFileNode.Container.FullName;
                inputFieldContentEditor.text = "";
                try
                {
                    var fileRead = File.ReadAllText(filePath);
                    inputFieldContentEditor.text = fileRead;
                }
                catch (IOException ex)
                {
                    Debug.LogWarning("SystemNode had IOException (caught): " + ex);
                }
                catch (UnauthorizedAccessException ex)
                {
                    Debug.LogWarning("SystemNode had UnauthorizedAccessException (caught): " + ex);
                }
            }
            return;
        }
        public void HandleRename()
        {
            buttonCreateFileExecute.gameObject.SetActive(false);
            buttonCreateFolderExecute.gameObject.SetActive(false);
            buttonRenameExecute.gameObject.SetActive(true);
            buttonSaveContent.gameObject.SetActive(false);
            imageIcon.gameObject.SetActive(false);
            inputFieldContentEditor.gameObject.SetActive(false);
            inputFieldEditor.gameObject.SetActive(true);
            scrollRectContent.gameObject.SetActive(false);
            textContent.gameObject.SetActive(false);

            if (selectedDirectoryNode != null)
            {
                var directoryPath = selectedDirectoryNode.Container.FullName;
                inputFieldEditor.text = directoryPath;
            }
            else if (selectedFileNode != null)
            {
                var filePath = selectedFileNode.Container.FullName;
                inputFieldEditor.text = filePath;
            }
            else
            {
                inputFieldEditor.text = "";
            }
            return;
        }
        public void HandleRenameExecute()
        {
            if (selectedDirectoryNode != null)
            {
                var directoryPath = selectedDirectoryNode.Container.FullName;
                var newDirectory = selectedDirectoryNode.parentDirectory;
                if (Directory.Exists(directoryPath) && !Directory.Exists(inputFieldEditor.text))
                {
                    try
                    {
                        Directory.Move(directoryPath, inputFieldEditor.text);
                    }
                    catch (IOException ex)
                    {
                        Debug.LogWarning("SystemNode had IOException (caught): " + ex);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Debug.LogWarning("SystemNode had UnauthorizedAccessException (caught): " + ex);
                    }
                    fileManager.OpenDirectory(newDirectory);
                }
            }
            else if (selectedFileNode != null)
            {
                var filePath = selectedFileNode.Container.FullName;
                if (File.Exists(filePath) && !File.Exists(inputFieldEditor.text))
                {
                    try
                    {
                        File.Move(filePath, inputFieldEditor.text);
                    }
                    catch (IOException ex)
                    {
                        Debug.LogWarning("SystemNode had IOException (caught): " + ex);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Debug.LogWarning("SystemNode had UnauthorizedAccessException (caught): " + ex);
                    }
                    panelContainer.gameObject.SetActive(false);
                    fileManager.OpenDirectory(selectedFileNode.parentDirectory);
                }
            }
            panelContainer.gameObject.SetActive(false);
            return;
        }
        public void HandleSecurity()
        {
            buttonCreateFileExecute.gameObject.SetActive(false);
            buttonCreateFolderExecute.gameObject.SetActive(false);
            buttonRenameExecute.gameObject.SetActive(false);
            buttonSaveContent.gameObject.SetActive(false);
            imageIcon.gameObject.SetActive(true);
            inputFieldContentEditor.gameObject.SetActive(false);
            inputFieldEditor.gameObject.SetActive(false);
            scrollRectContent.gameObject.SetActive(true);
            textContent.gameObject.SetActive(true);

            if (selectedDriveNode != null)
            {
                var baseContainer = selectedDriveNode.BaseContainer;
                var container = selectedDriveNode.Container;
                textContent.text = "";
            }
            if (selectedDirectoryNode != null)
            {
                var container = selectedDirectoryNode.Container;
                textContent.text = "";
            }
            else if (selectedFileNode != null)
            {
                var container = selectedFileNode.Container;
                textContent.text = "";
            }
            return;
        }
        public void HandleSaveContent()
        {
            if (selectedFileNode != null)
            {
                var filePath = selectedFileNode.Container.FullName;
                if (File.Exists(filePath))
                {
                    try
                    {
                        File.WriteAllText(filePath, inputFieldContentEditor.text);
                    }
                    catch (IOException ex)
                    {
                        Debug.LogWarning("SystemNode had IOException (caught): " + ex);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Debug.LogWarning("SystemNode had UnauthorizedAccessException (caught): " + ex);
                    }
                }
            }
            panelContainer.gameObject.SetActive(false);
            return;
        }
    }
}
