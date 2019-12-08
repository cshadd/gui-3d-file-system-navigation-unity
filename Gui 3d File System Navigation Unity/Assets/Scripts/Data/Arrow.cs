using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class Arrow : MonoBehaviour
    {
        public DirectoryNode currentDirectory;
        public ArrowDirection direction;

        private Arrow() : base() { return; }
    }
    public enum ArrowDirection
    {
        Left,
        Right
    }
}
