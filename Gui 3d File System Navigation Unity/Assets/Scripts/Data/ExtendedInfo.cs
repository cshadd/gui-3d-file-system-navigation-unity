using System;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    [Serializable]
    public class ExtendedInfo
    {
        public string computer;
        public Sprite fileIcon; // TODO: Change to just icon
        public bool isAccessDenied;
        public bool isShowingInternal;
        public string location;
        public string size;

        public ExtendedInfo() : base() { return; }

        public ExtendedInfo Unassign()
        {
            computer = null;
            fileIcon = null;
            isAccessDenied = false;
            isShowingInternal = false;
            location = null;
            size = null;
            return this;
        }
    }
}
