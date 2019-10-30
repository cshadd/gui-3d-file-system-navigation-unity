using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public abstract class SystemNode<T> : MonoBehaviour, ISystemNode<T>
        where T : FileSystemInfo
    {
        [SerializeField]
        public ExtendedInfo extendedInfo;

        [SerializeField]
        protected ISystemNode<DirectoryInfo> parentDirectory;

        public T Container { get; protected set; }

        protected SystemNode(string path = null) : base() {
            Grab(path);
            return;
        }

        public abstract ISystemNode<T> Grab(string path);

        public ISystemNode<T> Assign(T container,
            ISystemNode<DirectoryInfo> parent = null)
        {
            Container = container;
            extendedInfo = new ExtendedInfo(Container);
            parentDirectory = parent;
            if (Container.Exists) {
                Debug.Log("SystemNode assigned: " + Container.FullName);
                gameObject.name = Container.FullName;
            }
            else
            {
                Debug.LogWarning("SystemNode does not exist: " + Container.FullName);
            }
            return this;
        }

        public ISystemNode<T> Unassign() {
            Debug.Log("SystemNode unassigned: " + Container.FullName);
            Container = null;
            extendedInfo = null;
            parentDirectory = null;
            return this;
        }
    }
}
