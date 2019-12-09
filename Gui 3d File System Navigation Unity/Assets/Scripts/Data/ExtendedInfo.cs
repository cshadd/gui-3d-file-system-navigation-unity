using System;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    [Serializable]
    public class ExtendedInfo
    {
        public string computer;
        public Sprite icon; 
        public bool isAccessDenied;
        public bool isShowingInternal;
        public string location;
        public long size;

        public ExtendedInfo() : base() { return; }

        public ExtendedInfo Unassign()
        {
            computer = null;
            icon = null;
            isAccessDenied = false;
            isShowingInternal = false;
            location = null;
            size = 0;
            return this;
        }
    }
}
