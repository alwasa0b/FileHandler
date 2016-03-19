using System;
using System.Collections;
using System.Collections.Generic;

namespace FileHandler
{
    public abstract class FileHandlerBase<TProcess>: IFileHandler where TProcess : PreHandler, new()
    {
        protected readonly ProcessDelegate Process;
        protected readonly DirectoryHelperBase<TProcess> SourceDirectory;
        protected List<DirectoryHelperBase<TProcess>> DestDirectories;
        protected List<object> Audits = new List<object>(); 
        protected bool Disposed;


        public delegate IList ProcessDelegate(string targetFile, PreHandler compress);

        public static IFileHandler CreateFileHandler(string type, string sourcePath, List<string> destPaths, ProcessDelegate process = null)
        {
            switch (type)
            {
                case "SKIP":
                    return new FileSkipper<TProcess>(sourcePath, destPaths, process);
                case "MERGE":
                    return new FileMerger<TProcess>(sourcePath, destPaths, process);
                case "OVERWRITE":
                    return new FileOverwriter<TProcess>(sourcePath, destPaths, process);
                default:
                    throw new NotImplementedException();
            }
        }

        protected FileHandlerBase(string sourcePath, ProcessDelegate process)
        {
            Process = process;
            SourceDirectory = new DirectoryHelper<TProcess>(sourcePath);
        }

        public abstract void CopyFiles();

        public virtual void MoveFiles()
        {
            CopyFiles();
            SourceDirectory.Directory.Delete(true);
        }

        public void DeleteTemps()
        {
            try
            {
                SourceDirectory.TempStorage.Delete(true);
                foreach (var directoryHelpers in DestDirectories)
                {
                    directoryHelpers.TempStorage.Delete(true);
                }
            }
            catch (Exception e)
            {
                //Failed to delete tempfolders
            }
        }
    }
}