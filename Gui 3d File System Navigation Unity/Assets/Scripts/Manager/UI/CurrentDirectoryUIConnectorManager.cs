using Gui3dFileSystemNavigationUnity.Data;
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
        public override void ExecuteUI<T>(AbstractSystemNode<T> node)
        {
            base.ExecuteUI(node);
            var container = node.Container;

            textCurrentDirectoryPath.text = "Current Directory: " + container.FullName;
            return;
        }
    }
}
