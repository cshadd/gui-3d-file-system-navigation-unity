using System;
using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    [Serializable]
    public class ExtendedInfo
    {
        [SerializeField]
        public Sprite fileIcon;
        [SerializeField]
        public bool isAccessDenied;

        private FileSystemInfo Container { get; set; }

        public ExtendedInfo(FileSystemInfo container = null)
        {
            Container = container;
            return;
        }
    }
}
