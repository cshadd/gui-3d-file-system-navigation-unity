using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class DirectoryNode : AbstractSystemNode<DirectoryInfo>
    {
        public List<DirectoryNode> directoryNodes;
        public List<FileNode> fileNodes;
        public DirectoryNode parentDirectory;

        protected DirectoryNode(string path = null) : base(path) { return; }

        public virtual ISystemNode<DirectoryInfo> Assign(DirectoryInfo container,
            DirectoryNode parent = null)
        {
            var assignment = base.Assign(container);
            parentDirectory = parent;
            if (iconDatabase != null)
            {
                extendedInfo.icon = iconDatabase.GrabIcon("Default Directory");
            }

            DirectoryInfo[] directories = null;
            FileInfo[] files = null;

            try
            {
                directories = Container.GetDirectories();
                files = Container.GetFiles();
            }
            catch (IOException ex)
            {
                extendedInfo.isAccessDenied = true;
                Debug.LogWarning("SystemNode had IOException (caught): " + ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                extendedInfo.isAccessDenied = true;
                Debug.LogWarning("SystemNode had UnauthorizedAccessException (caught): " + ex);
            }
            if (parentDirectory != null && parentDirectory.Container != null)
            {
                extendedInfo.location = parentDirectory.Container.FullName;
            }
            if (directories != null && files != null)
            {
                extendedInfo.size = directories.Length + files.Length;
            }
            return assignment;
        }
        public virtual ISystemNode<DirectoryInfo> Depopulate()
        {
            if (Container.Exists && extendedInfo.isShowingInternal)
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

                extendedInfo.isShowingInternal = false;
            }
            return this;
        }
        public override ISystemNode<DirectoryInfo> Grab(string path)
        {
            return Assign(new DirectoryInfo(path));
        }
        public virtual ISystemNode<DirectoryInfo> Populate()
        {
            var sample = new GameObject();
            Populate(sample, sample);
            Destroy(sample);
            return this;
        }
        [Obsolete("This method is obsolete.")]
        public virtual ISystemNode<DirectoryInfo> Populate(PrimitiveType directoryPrimitiveType,
            PrimitiveType filePrimitiveType)
        {
            var sample1 = GameObject.CreatePrimitive(directoryPrimitiveType);
            var sample2 = GameObject.CreatePrimitive(filePrimitiveType);
            Populate(sample1, sample2);
            Destroy(sample1);
            Destroy(sample2);
            return this;
        }
        public virtual ISystemNode<DirectoryInfo> Populate(GameObject directoryTemplate,
            GameObject fileTemplate)
        {
            if (Container.Exists && !extendedInfo.isAccessDenied && !extendedInfo.isShowingInternal)
            {
                directoryNodes = new List<DirectoryNode>();
                fileNodes = new List<FileNode>();
                try
                {
                    foreach (DirectoryInfo directory in Container.GetDirectories())
                    {
                        var directoryGameObject = Instantiate(directoryTemplate);
                        directoryGameObject.transform.parent = transform;
                        var directoryNode = directoryGameObject.AddComponent<DirectoryNode>();
                        directoryNode.iconDatabase = iconDatabase;
                        directoryNode.Assign(directory, this);
                        directoryNodes.Add(directoryNode);
                    }

                    foreach (FileInfo file in Container.GetFiles())
                    {
                        var fileGameObject = Instantiate(fileTemplate);
                        fileGameObject.transform.parent = transform;
                        var fileNode = fileGameObject.AddComponent<FileNode>();
                        fileNode.iconDatabase = iconDatabase;
                        fileNode.Assign(file, this);
                        fileNodes.Add(fileNode);
                    }

                    extendedInfo.isAccessDenied = false;
                    extendedInfo.isShowingInternal = true;
                }
                catch (IOException ex)
                {
                    extendedInfo.isAccessDenied = true;
                    Debug.LogWarning("SystemNode had IOException (caught): " + ex);
                }
                catch (UnauthorizedAccessException ex)
                {
                    extendedInfo.isAccessDenied = true;
                    Debug.LogWarning("SystemNode had UnauthorizedAccessException (caught): " + ex);
                }
            }
            return this;
        }
        public override ISystemNode<DirectoryInfo> Unassign()
        {
            Depopulate();
            directoryNodes = null;
            fileNodes = null;
            return base.Unassign();
        }
    }
}
