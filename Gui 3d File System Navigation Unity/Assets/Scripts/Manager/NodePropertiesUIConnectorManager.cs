using Gui3dFileSystemNavigationUnity.Data;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public class NodePropertiesUIConnectorManager : AbstractUIConnectorManager
    {
        [SerializeField]
        private Text textDetails;
        [SerializeField]
        private Text textGeneral;
        [SerializeField]
        private Text textSecurity;

        private NodePropertiesUIConnectorManager() : base() { return; }

        public new void ExecuteUI(DriveNode driveNode)
        {
            var baseContainer = driveNode.BaseContainer;
            base.ExecuteUI<DirectoryInfo>(driveNode);
            ExecuteUI<DirectoryInfo>(driveNode);
            return;
        }
        public new void ExecuteUI<T>(AbstractSystemNode<T> node)
             where T : FileSystemInfo
        {
            base.ExecuteUI<T>(node);
            var container = node.Container;
            var extendedInfo = node.extendedInfo;

            textDetails.text = "TEST";
            textGeneral.text = "TEST";
            textSecurity.text = "TEST";
            return;
        }
    }
}
