using System.IO;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class FileNode : SystemNode<FileInfo>
    {
        public DirectoryNode parentDirectory;

        public FileNode(string path = null) : base(path) { return; }

        public ISystemNode<FileInfo> Assign(FileInfo container,
            DirectoryNode parent = null)
        {
            parentDirectory = parent;
            return base.Assign(container);
        }
        public override ISystemNode<FileInfo> Grab(string path)
        {
            return Assign(new FileInfo(path));
        }
        public new ISystemNode<FileInfo> Unassign()
        {
            parentDirectory = null;
            return base.Unassign();
        }
    }
}
