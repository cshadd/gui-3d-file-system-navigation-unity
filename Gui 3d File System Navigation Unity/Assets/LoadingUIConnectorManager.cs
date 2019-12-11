using Gui3dFileSystemNavigationUnity.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public class LoadingUIConnectorManager : AbstractUIConnectorManager
    {
        [SerializeField]
        private Text textLoading;

        private LoadingUIConnectorManager() : base() { return; }

        public void Clear()
        {
            textLoading.text = "";
            return;
        }
        public override void ExecuteUI<T>(AbstractSystemNode<T> node)
        {
            base.ExecuteUI(node);
            var container = node.Container;
            Debug.LogWarning("Loading Node: " + container.FullName);
            textLoading.text = "Loading Node: " + container.FullName;
            return;
        }
    }
}
