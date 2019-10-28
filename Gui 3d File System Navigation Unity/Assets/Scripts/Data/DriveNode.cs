using System.IO;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class DriveNode : DirectoryNode
    {
        private new DriveInfo Container { get; set; }

        private DirectoryInfo BaseContainer
        {
            get
            {
                return base.Container;
            }
        }

        public DriveNode() : base()
        {
            return;
        }

        public new void Assign(DirectoryInfo container)
        {
            Assign(container, null);
            return;
        }
        public new void Assign(DirectoryInfo container, DirectoryNode parent)
        {
            Assign(new DriveInfo(container.FullName), parent);
            return;
        }

        public void Assign(DriveInfo container)
        {
            Assign(container, null);
            return;
        }
        public void Assign(DriveInfo container, DirectoryNode parent)
        {
            base.Assign(new DirectoryInfo(container.Name), parent);
            Container = container;

            // Debug.LogWarning(Container + ", " + BaseContainer + ", " + Container.GetType() + ", " + BaseContainer.GetType() + ", " + Container.Equals(BaseContainer));
            return;
        }
    }
}
