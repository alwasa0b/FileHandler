using System;
using System.Collections.Generic;

namespace FileHandler
{
    public class FileOverwriter<TProcess> : FileHandlerBase<TProcess> where TProcess : PreHandler, new()
    {
        public FileOverwriter(string sourcePath, IEnumerable<string> destPath, ProcessDelegate process) : base(sourcePath, process)
        {
            DestDirectories = new List<DirectoryHelperBase<TProcess>>();
            foreach (string s in destPath)
            {
                try
                {
                    var directoryHelper = new DirectoryHelper<TProcess>(s);
                    DestDirectories.Add(directoryHelper);
                }
                catch (Exception e)
                {
                    //log skipped
                }
            }
        }

        public override void CopyFiles()
        {
            foreach (var destDirectory in DestDirectories)
            {
                destDirectory.Directory.Delete(true);
                destDirectory.Directory.Create();
                CopyFilesTo(destDirectory);
            }
        }

        public void MoveDestFilesBack()
        {
            foreach (var destDirectoriesClass in DestDirectories)
            {
                var destPath = destDirectoriesClass.Directory.FullName;
                destDirectoriesClass.TempStorage.MoveTo(destPath);
            }   
        }

        protected void CopyFilesTo(DirectoryHelperBase<TProcess> destDirectory)
        {
            var files = SourceDirectory.Directory.GetFiles();
           
            foreach (var fileInfo in files)
            {
                string targetFile = $"{destDirectory.Directory.FullName}\\{fileInfo.Name}";
                fileInfo.CopyTo(targetFile);
                Process(targetFile, destDirectory.Process);
            }
        }
    }
}
