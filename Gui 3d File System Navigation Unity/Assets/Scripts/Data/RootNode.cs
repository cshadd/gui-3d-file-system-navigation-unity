using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class RootNode : DirectoryNode
    {
        public List<DriveNode> driveNodes;

        public new ISystemNode<DirectoryInfo> Assign(DirectoryInfo container,
            DirectoryNode parent = null)
        {
            throw new NotSupportedException("This method is not supported for a RootNode.");
        }
        public override ISystemNode<DirectoryInfo> Grab(string path)
        {
            throw new NotSupportedException("This method is not supported for a RootNode.");
        }
        public new ISystemNode<DirectoryInfo> Populate()
        {
            var sample = new GameObject();
            Populate(sample, sample);
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
        public new ISystemNode<DirectoryInfo> Populate(PrimitiveType drivePrimitiveType,
            PrimitiveType notUsed)
        {
            return Populate(drivePrimitiveType);
        }
        public new ISystemNode<DirectoryInfo> Populate(GameObject driveTemplate,
            GameObject notUsed = null)
        {
            driveNodes = new List<DriveNode>();
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                var driveGameObject = Instantiate(driveTemplate);
                driveGameObject.transform.parent = transform;
                var driveNode = driveGameObject.AddComponent<DriveNode>();
                driveNode.Assign(drive, this);
                driveNodes.Add(driveNode);
            }
            return this;
        }
    }
}
