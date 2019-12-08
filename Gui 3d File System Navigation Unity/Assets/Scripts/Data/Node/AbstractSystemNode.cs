using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public abstract class AbstractSystemNode<T> : MonoBehaviour, ISystemNode<T>
        where T : FileSystemInfo
    {
        public ExtendedInfo extendedInfo;
        public FileIconDatabase fileIconDatabase;

        public T Container { get; protected set; }

        protected AbstractSystemNode(string path = null) : base() {
            if (path != null)
            {
                Grab(path);
            }
            return;
        }

        public abstract ISystemNode<T> Grab(string path);

        public virtual ISystemNode<T> Assign(T container)
        {
            Container = container;
            extendedInfo = new ExtendedInfo();
            if (Container.Exists)
            {
                gameObject.name = Container.FullName;
                extendedInfo.Unassign();
                extendedInfo.computer = System.Environment.MachineName;
                Debug.LogWarning(System.Environment.SystemDirectory);
                if (fileIconDatabase != null)
                {
                    extendedInfo.fileIcon = fileIconDatabase.GrabIcon("Unknown");
                }
                Debug.Log("SystemNode assigned: " + Container.FullName);
            }
            else
            {
                Debug.LogWarning("SystemNode does not exist: " + Container.FullName);
            }
            return this;
        }
        public virtual ISystemNode<T> Unassign() {
            Debug.Log("SystemNode unassigned: " + Container.FullName);
            extendedInfo.Unassign();
            extendedInfo = null;
            fileIconDatabase = null;
            Container = null;
            return this;
        }
    }
}
