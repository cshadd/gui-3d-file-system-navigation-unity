using System.IO;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class DriveNode : DirectoryNode
    {
        public DriveInfo BaseContainer { get; protected set; }

        public DriveNode(string path = null) : base(path) { return; }

        public new ISystemNode<DirectoryInfo> Assign(DirectoryInfo container,
            DirectoryNode parent = null)
        {
            BaseContainer = new DriveInfo(container.FullName);
            return base.Assign(container, parent);
        }
        public ISystemNode<DirectoryInfo> Assign(DriveInfo container,
            DirectoryNode parent = null)
        {
            BaseContainer = container;
            return base.Assign(new DirectoryInfo(container.Name), parent);
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
