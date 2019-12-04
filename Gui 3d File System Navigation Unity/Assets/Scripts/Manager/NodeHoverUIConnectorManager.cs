using Gui3dFileSystemNavigationUnity.Data;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public class NodeHoverUIConnectorManager : AbstractUIConnectorManager
    {
        [SerializeField]
        private Image imageNodeHoverIcon;
        [SerializeField]
        private Text textNodeHoverName;

        private NodeHoverUIConnectorManager() : base() { return; }

        public void Clear()
        {
            imageNodeHoverIcon.sprite = null;
            textNodeHoverName.text = "";
            return;
        }
        public new void ExecuteUI<T>(AbstractSystemNode<T> node)
             where T : FileSystemInfo
        {
            base.ExecuteUI<T>(node);
            var container = node.Container;
            var extendedInfo = node.extendedInfo;

            textNodeHoverName.text = container.Name;
            imageNodeHoverIcon.sprite = extendedInfo.fileIcon;
            return;
        }
    }
}
