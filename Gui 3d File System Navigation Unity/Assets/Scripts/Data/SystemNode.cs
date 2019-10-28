using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public abstract class SystemNode<T> : MonoBehaviour, ISystemNode<T>
        where T : FileSystemInfo
    {
        [SerializeField]
        protected DirectoryNode parentDirectory;

        protected T Container { get; set; }

        protected SystemNode() : base()
        {
            return;
        }

        public void Assign(T container)
        {
            Assign(container, null);
            return;
        }
        public void Assign(T container, DirectoryNode parent)
        {
            Container = container;
            parentDirectory = parent;
            if (Container.Exists)
            {
                Debug.Log("Item found: " + Container.FullName);
                gameObject.name = Container.FullName;
            }
            else
            {
                Debug.LogWarning("Item does not exist: " + Container.FullName);
            }
            return;
        }

        protected void OnMouseDown()
        {
            Debug.Log("Clicked on item: " + Container.FullName);
            return;
        }
    }
}
