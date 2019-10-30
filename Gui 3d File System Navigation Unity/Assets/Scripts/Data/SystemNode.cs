using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public abstract class SystemNode<T> : MonoBehaviour, ISystemNode<T>
        where T : FileSystemInfo
    {
        [SerializeField]
        protected DirectoryNode parentDirectory;

        public T Container { get; protected set; }
        public ExtendedInfo ExtendedInfo { get; protected set; }

        protected SystemNode() : base() { return; }

        public ISystemNode<T> Assign(T container)
        {
            return Assign(container, null);
        }
        public ISystemNode<T> Assign(T container, DirectoryNode parent)
        {
            Container = container;
            parentDirectory = parent;
            if (Container.Exists)
            {
                Debug.Log("SystemNode assigned: " + Container.FullName);
                gameObject.name = Container.FullName;
            }
            else
            {
                Debug.LogWarning("SystemNode does not exist: " + Container.FullName);
            }
            return this;
        }

        public ISystemNode<T> Unassign()
        {
            Debug.Log("SystemNode unassigned: " + Container.FullName);
            Container = null;
            ExtendedInfo = null;
            parentDirectory = null;
            return this;
        }
    }
}
