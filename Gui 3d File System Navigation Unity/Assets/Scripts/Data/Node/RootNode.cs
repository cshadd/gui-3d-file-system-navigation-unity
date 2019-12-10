using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class RootNode : DirectoryNode
    {
        public List<DriveNode> driveNodes;

        private RootNode() : base() { return; }

        public override ISystemNode<DirectoryInfo> Assign(DirectoryInfo container,
            DirectoryNode parent = null)
        {
            throw new NotSupportedException("This method is not supported for a RootNode.");
        }
        public override ISystemNode<DirectoryInfo> Depopulate()
        {
            throw new NotSupportedException("This method is not supported for a RootNode.");
        }
        public override ISystemNode<DirectoryInfo> Grab(string path)
        {
            throw new NotSupportedException("This method is not supported for a RootNode.");
        }
        public override ISystemNode<DirectoryInfo> Populate()
        {
            var sample = new GameObject();
            Populate(sample);
            Destroy(sample);
            return this;
        }
        [Obsolete("This method is obsolete.")]
        public ISystemNode<DirectoryInfo> Populate(PrimitiveType drivePrimitiveType)
        {
            var sample = GameObject.CreatePrimitive(drivePrimitiveType);
            Populate(sample);
            Destroy(sample);
            return this;
        }
        [Obsolete("This method is obsolete.")]
        public override ISystemNode<DirectoryInfo> Populate(PrimitiveType directoryPrimitiveType,
            PrimitiveType filePrimitiveType)
        {
            throw new NotSupportedException("This method is not supported for a RootNode.");
        }
        public ISystemNode<DirectoryInfo> Populate(GameObject driveTemplate)
        {
            directoryNodes = new List<DirectoryNode>();
            driveNodes = new List<DriveNode>();
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                var driveGameObject = Instantiate(driveTemplate);
                driveGameObject.transform.parent = transform;
                var driveNode = driveGameObject.AddComponent<DriveNode>();
                driveNode.iconDatabase = iconDatabase;
                driveNode.Assign(drive, this);
                directoryNodes.Add(driveNode);
                driveNodes.Add(driveNode);
            }
            return this;
        }
        public override ISystemNode<DirectoryInfo> Populate(GameObject directoryTemplate,
            GameObject fileTemplate)
        {
            throw new NotSupportedException("This method is not supported for a RootNode.");
        }
        public override ISystemNode<DirectoryInfo> Unassign()
        {
            throw new NotSupportedException("This method is not supported for a RootNode.");
        }
    }
}
