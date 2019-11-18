using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public abstract class SystemNode<T> : MonoBehaviour, ISystemNode<T>
        where T : FileSystemInfo
    {
        public ExtendedInfo extendedInfo;

        public T Container { get; protected set; }

        protected SystemNode(string path = null) : base() {
            if (path != null)
            {
                Grab(path);
            }
            return;
        }

        public abstract ISystemNode<T> Grab(string path);

        public ISystemNode<T> Assign(T container)
        {
            Container = container;
            extendedInfo = new ExtendedInfo(Container);
            if (Container.Exists)
            {
                gameObject.name = Container.FullName;
                Debug.Log("SystemNode assigned: " + Container.FullName);
            }
            else
            {
                Debug.LogWarning("SystemNode does not exist: " + Container.FullName);
            }
            return this;
        }
        public ISystemNode<T> Unassign() {
            Debug.Log("SystemNode unassigned: " + Container.FullName);
            extendedInfo.Unassign();
            extendedInfo = null;
            Container = null;
            return this;
        }
    }
}
