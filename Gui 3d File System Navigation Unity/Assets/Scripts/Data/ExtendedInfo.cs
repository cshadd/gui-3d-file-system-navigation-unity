using System;
using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    [Serializable]
    public class ExtendedInfo
    {
        public Sprite fileIcon;
        public bool isAccessDenied;
        public bool isShowingInternal;

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
            isShowingInternal = false;
            Container = null;
            return this;
        }
    }
}
