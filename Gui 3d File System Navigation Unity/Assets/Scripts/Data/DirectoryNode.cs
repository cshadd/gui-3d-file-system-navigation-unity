using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class DirectoryNode : SystemNode<DirectoryInfo>
    {
        [SerializeField]
        public List<DirectoryNode> directoryNodes;
        [SerializeField]
        public List<FileNode> fileNodes;
        [SerializeField]
        private bool isShowingInternal;
        [SerializeField]
        public DirectoryNode parentDirectory;

        public DirectoryNode(string path = null) : base(null) { return; }

        public ISystemNode<DirectoryInfo> Assign(DirectoryInfo container,
            DirectoryNode parent = null)
        {
            parentDirectory = parent;
            return base.Assign(container);
        }
        public override ISystemNode<DirectoryInfo> Grab(string path)
        {
            return Assign(new DirectoryInfo(path));
        }
        public ISystemNode<DirectoryInfo> Populate()
        {
            var sample = new GameObject();
            Populate(sample, sample);
            Destroy(sample);
            return this;
        }
        public ISystemNode<DirectoryInfo> Populate(PrimitiveType directoryPrimitiveType,
            PrimitiveType filePrimitiveType)
        {
            var sample1 = GameObject.CreatePrimitive(directoryPrimitiveType);
            var sample2 = GameObject.CreatePrimitive(filePrimitiveType);
            Populate(sample1, sample2);
            Destroy(sample1);
            Destroy(sample2);
            return this;
        }
        public ISystemNode<DirectoryInfo> Populate(GameObject directoryTemplate,
    GameObject fileTemplate)
        {
            if (Container.Exists && !isShowingInternal)
            {
                try
                {
                    foreach (DirectoryInfo directory in Container.GetDirectories())
                    {
                        var directoryGameObject = Instantiate(directoryTemplate);
                        directoryGameObject.transform.parent = transform;
                        var directoryNode = directoryGameObject.AddComponent<DirectoryNode>();
                        directoryNode.Assign(directory, this);
                        directoryNodes.Add(directoryNode);
                    }

                    foreach (FileInfo file in Container.GetFiles())
                    {
                        var fileGameObject = Instantiate(fileTemplate);
                        fileGameObject.transform.parent = transform;
                        var fileNode = fileGameObject.AddComponent<FileNode>();
                        fileNode.Assign(file, this);
                        fileNodes.Add(fileNode);
                    }

                    extendedInfo.isAccessDenied = false;
                }
                catch (UnauthorizedAccessException)
                {
                    Debug.LogWarning("SystemNode cannot be expanded, access denied: "
                        + Container.FullName);
                }

                isShowingInternal = true;
            }
            return this;
        }
        public ISystemNode<DirectoryInfo> Depopulate()
        {
            if (Container.Exists && isShowingInternal)
            {
                foreach (DirectoryNode directoryNode in directoryNodes)
                {
                    directoryNode.Depopulate().Unassign();
                    Destroy(directoryNode.gameObject);
                }

                directoryNodes.Clear();

                foreach (FileNode fileNode in fileNodes)
                {
                    fileNode.Unassign();
                    Destroy(fileNode.gameObject);
                }

                fileNodes.Clear();

                isShowingInternal = false;
            }
            return this;
        }
        #region SAMPLE
      // private void OnMouseDown()
      // {
      //     if (isShowingInternal)
      //     {
      //         Depopulate();
      //     }
      //     else
      //     {
      //         Populate(PrimitiveType.Capsule, PrimitiveType.Cube);
      //     }
      //     return;
      // }
        #endregion
        private void Start()
        {
            // For some reason directoryNodes is null even though
            // it is being serialized, a fix around this is to
            // create a new instance.
            directoryNodes = new List<DirectoryNode>();

            // For some reason fileNodes is null even though
            // it is being serialized, a fix around this is to
            // create a new instance.
            fileNodes = new List<FileNode>();
            return;
        }
        public new ISystemNode<DirectoryInfo> Unassign()
        {
            Depopulate();
            directoryNodes = null;
            fileNodes = null;
            return base.Unassign();
        }
    }
}
