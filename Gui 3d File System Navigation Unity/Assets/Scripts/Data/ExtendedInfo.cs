using System.IO;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class ExtendedInfo
    {
        private FileSystemInfo Container { get; set; }

        public ExtendedInfo() : this(null) { return; }
        public ExtendedInfo(FileSystemInfo container) : base() {
            Container = container;
            return;
        }
    }
}
