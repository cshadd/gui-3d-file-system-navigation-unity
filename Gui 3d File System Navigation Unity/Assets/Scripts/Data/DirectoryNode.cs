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
        public ISystemNode<DirectoryInfo> Populate(PrimitiveType directoryPrimitiveType,
            PrimitiveType filePrimitiveType)
        {
            if (Container.Exists && !isShowingInternal)
            {
                try
                {
                    foreach (DirectoryInfo directory in Container.GetDirectories())
                    {
                        var directoryGameObject = GameObject.CreatePrimitive(directoryPrimitiveType);
                        directoryGameObject.transform.parent = transform;
                        var directoryNode = directoryGameObject.AddComponent<DirectoryNode>();
                        directoryNode.Assign(directory, this);
                        directoryNodes.Add(directoryNode);
                    }

                    foreach (FileInfo file in Container.GetFiles())
                    {
                        var fileGameObject = GameObject.CreatePrimitive(filePrimitiveType);
                        fileGameObject.transform.parent = transform;
                        var fileNode = fileGameObject.AddComponent<FileNode>();
                        fileNode.Assign(file, this);
                        fileNodes.Add(fileNode);
                    }

                    extendedInfo.isAccessDenied = false;
                }
                catch (UnauthorizedAccessException)
                {
                    Debug.LogWarning("SystemNode cannot be expanded, access denied.");
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

        /*private void OnMouseDown()
        {
            if (isShowingInternal)
            {
                Depopulate();
            }
            else
            {
                Populate(PrimitiveType.Capsule, PrimitiveType.Cube);
            }
            return;
        }*/

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
