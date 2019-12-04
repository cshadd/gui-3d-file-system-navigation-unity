using Gui3dFileSystemNavigationUnity.Data;
using System.IO;

namespace Gui3dFileSystemNavigationUnity.Manager
{
    public interface UIConnectorManager
    {
        void ExecuteUI(DirectoryNode directoryNode);
        void ExecuteUI(DriveNode driveNode);
        void ExecuteUI(FileNode fileNode);
        void ExecuteUI<T>(AbstractSystemNode<T> node)
            where T : FileSystemInfo;
    }
}
