using Gui3dFileSystemNavigationUnity.Data;
using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public abstract class AbstractUIConnectorManager : MonoBehaviour, IUIConnectorManager {
        [SerializeField]
        protected CanvasRenderer panelContainer;

        protected AbstractUIConnectorManager() : base() { return; }

        public virtual void ExecuteUI(DirectoryNode directoryNode)
        {
            ExecuteUI<DirectoryInfo>(directoryNode);
            return;
        }
        public virtual void ExecuteUI(DriveNode driveNode)
        {
            ExecuteUI<DirectoryInfo>(driveNode);
            return;
        }
        public virtual void ExecuteUI(FileNode fileNode)
        {
            ExecuteUI<FileInfo>(fileNode);
            return;
        }
        public virtual void ExecuteUI<T>(AbstractSystemNode<T> node)
            where T : FileSystemInfo
        {
            panelContainer.gameObject.SetActive(true);
            return;
        }
    }
}

