using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    [CreateAssetMenu(fileName = "Icons", menuName = "ScriptableObjects/File Icons")]
    public class IconDatabase : ScriptableObject
    {
        [SerializeField]
        private List<IconEntry> entries;
        // private List<IconEntry> entries = new List<IconEntry>();

        private IconDatabase() : base() { return; }

        [Serializable]
        private class IconEntry
        {
            public string iconName;
            public Sprite iconSprite;

            private IconEntry() : base() { return; }
        }

        public Sprite GrabIcon(string name)
        {
            Sprite icon = null;
            foreach (IconEntry entry in entries)
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
