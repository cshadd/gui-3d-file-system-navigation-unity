using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class DirectoryNode : SystemNode<DirectoryInfo>
    {
        [SerializeField]
        private List<DirectoryNode> directoryNodes;
        [SerializeField]
        private List<FileNode> fileNodes;
        [SerializeField]
        private bool isShowingInternal;

        public DirectoryNode() : base()
        {
            return;
        }

        public new void OnMouseDown()
        {
            base.OnMouseDown();
            if (Container.Exists)
            {
                var directoryPosition = 5;
                var filePosition = 5;
                if (!isShowingInternal)
                {
                    foreach (DirectoryInfo directory in Container.GetDirectories())
                    {
                        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        sphere.transform.parent = this.transform;
                        sphere.transform.position = new Vector3(this.transform.position.x + directoryPosition, this.transform.position.y, this.transform.position.z + directoryPosition);
                        var directoryNode = sphere.AddComponent<DirectoryNode>();
                        directoryNode.Assign(directory, this);
                        directoryNodes.Add(directoryNode);
                        directoryPosition += 5;
                    }

                    foreach (FileInfo file in Container.GetFiles())
                    {
                        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.parent = this.transform;
                        cube.transform.position = new Vector3(this.transform.position.x - filePosition, this.transform.position.y, this.transform.position.z - filePosition);
                        var fileNode = cube.AddComponent<FileNode>();
                        fileNode.Assign(file, this);
                        fileNodes.Add(fileNode);
                        filePosition += 5;
                    }

                    isShowingInternal = true;
                }
            }
            return;
        }

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
    }
}
