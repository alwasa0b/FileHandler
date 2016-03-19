using System;
using System.Collections.Generic;
using System.IO;

namespace FileHandler
{
    public class FileSkipper<TProcess> : FileHandlerBase<TProcess> where TProcess : PreHandler, new()
    {
        

        public FileSkipper(string sourcePath, IEnumerable<string> destPath, ProcessDelegate process) : base(sourcePath, process)
        {
            DestDirectories = new List<DirectoryHelperBase<TProcess>>();
            foreach (string s in destPath)
            {
                try
                {
                    var directorySkipperHelper = new DirectorySkipperHelper<TProcess>(s);
                    DestDirectories.Add(directorySkipperHelper);
                }
                catch (Exception e)
                {
                    //log skipped
                }
            }
        }

        public override void CopyFiles()
        {
            foreach (var destDirectoriesClass in DestDirectories)
            {
                destDirectoriesClass.Directory.Create();
                foreach (string f in SourceDirectory.Files)
                {
                    var fi2 = new FileInfo(f);
                    string targetFile = $"{destDirectoriesClass.Directory.FullName}\\{fi2.Name}";
                    fi2.CopyTo(targetFile);
                    Process(targetFile, destDirectoriesClass.Process);
                }
            }   
        }
    }
}
