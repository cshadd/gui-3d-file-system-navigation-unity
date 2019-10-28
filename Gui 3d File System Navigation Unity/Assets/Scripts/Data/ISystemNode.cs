using System.IO;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public interface ISystemNode<T>
        where T : FileSystemInfo
    {
        void Assign(T container);
        void Assign(T container, DirectoryNode parent);
    }
}
