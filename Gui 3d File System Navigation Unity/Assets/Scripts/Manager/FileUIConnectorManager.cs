using Gui3dFileSystemNavigationUnity.Data;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public class FileUIConnectorManager : MonoBehaviour
    {
        [SerializeField]
        private Text textTest;

        private FileUIConnectorManager() : base() { return; }

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
        private void ExecuteUI<T>(SystemNode<T> node) where T : FileSystemInfo
        {
            var container = node.Container;
            var extendedInfo = node.extendedInfo;

            textTest.text = container.Name;
            return;
        }
    }
}
