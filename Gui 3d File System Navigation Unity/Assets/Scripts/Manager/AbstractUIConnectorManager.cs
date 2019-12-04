using Gui3dFileSystemNavigationUnity.Data;
using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public abstract class AbstractUIConnectorManager : MonoBehaviour, IUIConnectorManager {
        [SerializeField]
        private CanvasRenderer panelContainer;

        protected AbstractUIConnectorManager() : base() { return; }

        public void ExecuteUI(DirectoryNode directoryNode)
        {
            ExecuteUI<DirectoryInfo>(directoryNode);
            return;
        }
        public void ExecuteUI(DriveNode driveNode)
        {
            ExecuteUI<DirectoryInfo>(driveNode);
            return;
        }
        public void ExecuteUI(FileNode fileNode)
        {
            ExecuteUI<FileInfo>(fileNode);
            return;
        }
        public void ExecuteUI<T>(AbstractSystemNode<T> node)
            where T : FileSystemInfo
        {
            panelContainer.gameObject.SetActive(true);
            return;
        }
    }
}

