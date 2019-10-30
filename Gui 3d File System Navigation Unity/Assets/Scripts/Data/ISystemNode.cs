using System.IO;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public interface ISystemNode<T>
        where T : FileSystemInfo
    {
        T Container { get; }

        ISystemNode<T> Assign(T container,
            ISystemNode<DirectoryInfo> parent = null);
        ISystemNode<T> Grab(string path);
        ISystemNode<T> Unassign();
    }
}
