using Gui3dFileSystemNavigationUnity.Data;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public class CurrentDirectoryUIConnectorManager : AbstractUIConnectorManager
    {
        [SerializeField]
        private Text textCurrentDirectoryPath;

        private CurrentDirectoryUIConnectorManager() : base() { return; }

        public void Clear()
        {
            textCurrentDirectoryPath.text = "";
            return;
        }
        public new void ExecuteUI<T>(AbstractSystemNode<T> node)
             where T : FileSystemInfo
        {
            base.ExecuteUI<T>(node);
            var container = node.Container;

            textCurrentDirectoryPath.text = "Current Directory: " + container.FullName;
            return;
        }
    }
}
