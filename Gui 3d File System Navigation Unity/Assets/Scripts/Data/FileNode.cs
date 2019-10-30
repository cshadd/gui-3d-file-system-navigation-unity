using System.IO;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class FileNode : SystemNode<FileInfo>
    {
        public FileNode(string path = null) : base(path) { return; }

        public ISystemNode<FileInfo> Assign(FileInfo container,
            DirectoryNode parent = null)
        {
            return base.Assign(container, parent);
        }
        public override ISystemNode<FileInfo> Grab(string path)
        {
            return Assign(new FileInfo(path));
        }
    }
}
