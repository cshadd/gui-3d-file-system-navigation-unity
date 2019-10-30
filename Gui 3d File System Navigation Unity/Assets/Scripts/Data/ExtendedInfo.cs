using System.IO;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class ExtendedInfo
    {
        private FileSystemInfo Container { get; set; }

        public ExtendedInfo(FileSystemInfo container = null)
        {
            Container = container;
            return;
        }
    }
}
