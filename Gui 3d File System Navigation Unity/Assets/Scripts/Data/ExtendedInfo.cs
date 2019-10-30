using System.IO;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class ExtendedInfo
    {
        private FileSystemInfo Container { get; set; }

        private ExtendedInfo() : this(null) { return; }
        public ExtendedInfo(FileSystemInfo container)
        {
            Container = container;
            return;
        }
    }
}
