using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class Arrow : MonoBehaviour
    {
        public DirectoryNode currentDirectory;
        public ArrowDirection direction;

        public enum ArrowDirection
        {
            Left,
            Right
        }

        private Arrow() : base() { return; }
    }
}
