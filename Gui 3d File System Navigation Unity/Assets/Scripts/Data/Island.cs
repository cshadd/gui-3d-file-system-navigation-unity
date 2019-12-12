using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class Island : MonoBehaviour
    {
        public DirectoryNode currentDirectory;
        public int pageNumber = 0;

        private Island() : base() { return; }
    }
}
