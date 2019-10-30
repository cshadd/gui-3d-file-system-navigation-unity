using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class DriveNode : DirectoryNode
    {
        public DriveInfo BaseContainer { get; protected set; }

        public DriveNode(string path = null) : base(path) { return; }

        public new ISystemNode<DirectoryInfo> Assign(DirectoryInfo container,
            ISystemNode<DirectoryInfo> parent = null)
        {
            base.Assign(container, parent);
            BaseContainer = new DriveInfo(container.FullName);
            return this;
        }
        public ISystemNode<DirectoryInfo> Assign(DriveInfo container,
            DirectoryNode parent = null)
        {
            base.Assign(new DirectoryInfo(container.Name), parent);
            BaseContainer = container;
            // Debug.LogWarning(Container + ", " + BaseContainer + ", " + Container.GetType() + ", " + BaseContainer.GetType() + ", " + Container.Equals(BaseContainer));
            return this;
        }
        public ISystemNode<DirectoryInfo> Assign(DriveInfo container,
            ISystemNode<DirectoryInfo> parent = null)
        {
            base.Assign(new DirectoryInfo(container.Name), parent);
            BaseContainer = container;
            // Debug.LogWarning(Container + ", " + BaseContainer + ", " + Container.GetType() + ", " + BaseContainer.GetType() + ", " + Container.Equals(BaseContainer));
            return this;
        }

        public new ISystemNode<DirectoryInfo> Unassign()
        {
            base.Unassign();
            BaseContainer = null;
            return this;
        }
    }
}
