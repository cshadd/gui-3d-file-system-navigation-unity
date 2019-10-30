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
            Assign(container);
            return;
        }

        public ExtendedInfo Assign(FileSystemInfo container)
        {
            fileIcon = null; // May be changed.
            isAccessDenied = false; // May be changed.
            Container = container;
            return this;
        }
        public ExtendedInfo Unassign()
        {
            fileIcon = null;
            isAccessDenied = false;
            Container = null;
            return this;
        }
    }
}
