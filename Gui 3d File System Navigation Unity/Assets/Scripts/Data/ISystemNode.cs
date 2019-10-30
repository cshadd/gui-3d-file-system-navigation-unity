using System.IO;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public interface ISystemNode<T>
        where T : FileSystemInfo
    {
        T Container { get; }
        ExtendedInfo ExtendedInfo { get; }

        ISystemNode<T> Assign(T container);
        ISystemNode<T> Assign(T container, DirectoryNode parent);
        ISystemNode<T> Unassign();
    }
}
