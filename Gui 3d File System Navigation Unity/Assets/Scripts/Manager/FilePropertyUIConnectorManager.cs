using Gui3dFileSystemNavigationUnity.Data;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public class FilePropertyUIConnectorManager : AbstractUIConnectorManager
    {
        [SerializeField]
        private Text generalText;

        private FilePropertyUIConnectorManager() : base() { return; }

        public override void ExecuteUI<T>(AbstractSystemNode<T> node)
        {
            var container = node.Container;
            var extendedInfo = node.extendedInfo;

            generalText.text = container.FullName;
            return;
        }
    }
}
