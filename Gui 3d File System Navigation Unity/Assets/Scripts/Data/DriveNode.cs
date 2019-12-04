using System.IO;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class DriveNode : DirectoryNode
    {
        public DriveInfo BaseContainer { get; protected set; }

        private DriveNode(string path = null) : base(path) { return; }

        public new ISystemNode<DirectoryInfo> Assign(DirectoryInfo container,
            DirectoryNode parent = null)
        {
            var assignment = base.Assign(container, parent);
            BaseContainer = new DriveInfo(container.FullName);
            if (fileIconDatabase != null)
            {
                extendedInfo.fileIcon = fileIconDatabase.GrabIcon("Default Drive");
            }
            return assignment;
        }
        public ISystemNode<DirectoryInfo> Assign(DriveInfo container,
            DirectoryNode parent = null)
        {
            var assignment = base.Assign(new DirectoryInfo(container.Name), parent);
            BaseContainer = container;
            if (fileIconDatabase != null)
            {
                extendedInfo.fileIcon = fileIconDatabase.GrabIcon("Default Drive");
            }
            return assignment;
        }
        public override ISystemNode<DirectoryInfo> Grab(string path)
        {
            return Assign(new DirectoryInfo(path));
        }
        public new ISystemNode<DirectoryInfo> Unassign()
        {
            BaseContainer = null;
            return base.Unassign();
        }
    }
}
