using Gui3dFileSystemNavigationUnity.Data;
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
            imageNodeHoverIcon.gameObject.SetActive(false);
            textNodeHoverName.text = "";
            return;
        }
        public override void ExecuteUI<T>(AbstractSystemNode<T> node)
        {
            base.ExecuteUI<T>(node);
            var container = node.Container;
            var extendedInfo = node.extendedInfo;

            textNodeHoverName.text = container.Name;
            imageNodeHoverIcon.sprite = extendedInfo.icon;
            imageNodeHoverIcon.gameObject.SetActive(true);
            return;
        }
    }
}
