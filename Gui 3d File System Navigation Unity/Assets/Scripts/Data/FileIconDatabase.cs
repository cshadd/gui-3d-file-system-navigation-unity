using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    [CreateAssetMenu(fileName = "File Icons", menuName = "ScriptableObjects/File Icons")]
    public class FileIconDatabase : ScriptableObject
    {
        public List<FileIconEntry> entries;

        [Serializable]
        public class FileIconEntry
        {
            public string iconName;
            public Sprite iconSprite;
        }

        public Sprite GrabIcon(string name)
        {
            Debug.Log("Searching for icon " + name);
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
        private void Start()
        {
            entries = new List<FileIconEntry>();
        }
    }
}
