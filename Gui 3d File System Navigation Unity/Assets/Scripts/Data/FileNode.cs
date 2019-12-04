using System.IO;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class FileNode : AbstractSystemNode<FileInfo>
    {
        public DirectoryNode parentDirectory;

        public FileNode(string path = null) : base(path) { return; }

        public ISystemNode<FileInfo> Assign(FileInfo container,
            DirectoryNode parent = null)
        {
            var assignment = base.Assign(container);
            parentDirectory = parent;
            if (fileIconDatabase != null)
            {
                extendedInfo.fileIcon = fileIconDatabase.GrabIcon("Default File");
            }
            return assignment;
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
