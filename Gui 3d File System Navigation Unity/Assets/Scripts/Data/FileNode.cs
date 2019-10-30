using System.IO;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class FileNode : SystemNode<FileInfo>
    {
        public FileNode() : base() { return; }

        public ISystemNode<FileInfo> Assign(FileInfo container,
            DirectoryNode parent)
        {
            return base.Assign(container, parent);
        }
        public override ISystemNode<FileInfo> Grab(string path)
        {
            return Assign(new FileInfo(path));
        }
    }
}
