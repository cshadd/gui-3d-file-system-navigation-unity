using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public class FileUIConnectorManager : MonoBehaviour
    {
        public DriveInfo driveInfo;
        public FileSystemInfo fileSystemInfo;
        [SerializeField]
        private Text textTest;

        private FileUIConnectorManager() : base() { return; }

        public void UpdateUI()
        {
            textTest.text = fileSystemInfo.FullName;
            return;
        }
    }
}
