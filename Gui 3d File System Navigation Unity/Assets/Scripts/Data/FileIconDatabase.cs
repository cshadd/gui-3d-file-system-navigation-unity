using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    [CreateAssetMenu(fileName = "File Icons", menuName = "ScriptableObjects/File Icons")]
    public class FileIconDatabase : ScriptableObject
    {
        [SerializeField]
        private List<FileIconEntry> entries;
        // private List<FileIconEntry> entries = new List<FileIconEntry>();

        private FileIconDatabase() : base() { return; }

        [Serializable]
        public class FileIconEntry
        {
            public string iconName;
            public Sprite iconSprite;

            private FileIconEntry() : base() { return; }
        }

        public Sprite GrabIcon(string name)
        {
            Sprite icon = null;
            foreach (FileIconEntry entry in entries)
            {
                if (entry.iconName.Equals(name))
                {
                    icon = entry.iconSprite;
                    break;
                }
            }
            return icon;
        }
    }
}
