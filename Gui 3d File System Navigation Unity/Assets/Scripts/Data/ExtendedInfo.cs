using System;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    [Serializable]
    public class ExtendedInfo
    {
        public Sprite fileIcon;
        public bool isAccessDenied;
        public bool isShowingInternal;

        public ExtendedInfo() : base() { return; }

        public ExtendedInfo Unassign()
        {
            fileIcon = null;
            isAccessDenied = false;
            isShowingInternal = false;
            return this;
        }
    }
}
