using System;
using System.IO;
using UnityEngine;

namespace Gui3dFileSystemNavigationUnity.Data
{
    public class FileNode : AbstractSystemNode<FileInfo>
    {
        public DirectoryNode parentDirectory;

        private FileNode(string path = null) : base(path) { return; }

        public ISystemNode<FileInfo> Assign(FileInfo container,
            DirectoryNode parent = null)
        {
            var assignment = base.Assign(container);
            parentDirectory = parent;
            try
            {
                var fileStream = container.OpenRead();
                fileStream.Close();
                extendedInfo.isAccessDenied = false;
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
            if (fileIconDatabase != null)
            {
                var icon = fileIconDatabase.GrabIcon(container.Extension.ToLower());
                if (icon == null)
                {
                    icon = fileIconDatabase.GrabIcon("Default File");
                }
                extendedInfo.fileIcon = icon;
            }
            return assignment;
        }
        public override ISystemNode<FileInfo> Grab(string path)
        {
            return Assign(new FileInfo(path));
        }
        public override ISystemNode<FileInfo> Unassign()
        {
            parentDirectory = null;
            return base.Unassign();
        }
    }
}
