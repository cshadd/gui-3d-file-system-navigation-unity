using Gui3dFileSystemNavigationUnity.Data;
using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public abstract class AbstractUIConnectorManager : MonoBehaviour, IUIConnectorManager {
        protected AbstractUIConnectorManager() : base() { return; }

        public void ExecuteUI(DirectoryNode directoryNode)
        {
            ExecuteUI<DirectoryInfo>(directoryNode);
            return;
        }
        public void ExecuteUI(DriveNode driveNode)
        {
            var baseContainer = driveNode.BaseContainer;
            ExecuteUI<DirectoryInfo>(driveNode);
            return;
        }
        public void ExecuteUI(FileNode fileNode)
        {
            ExecuteUI<FileInfo>(fileNode);
            return;
        }

        public abstract void ExecuteUI<T>(AbstractSystemNode<T> node)
            where T : FileSystemInfo;
    }
}

