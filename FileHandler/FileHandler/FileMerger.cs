using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileHandler
{
    public class FileMerger<TProcess>: FileHandlerBase<TProcess> where TProcess : PreHandler, new()
    {
        public List<string> MergedFiles { get; } = new List<string>();

        public FileMerger(string sourcePath, IEnumerable<string> destPath, ProcessDelegate process) : base(sourcePath, process)
        {
            DestDirectories = new List<DirectoryHelperBase<TProcess>>(destPath.Select(d => new DirectoryHelper<TProcess>(d)).ToList());
        }

        public override void CopyFiles()
        {
            foreach (var destDirectories in DestDirectories)
            {
                foreach (string f in SourceDirectory.Files.Where(s => destDirectories.Files.All(d => d.Split('\\').Last() != s.Split('\\').Last())))
                {
                    var fi2 = new FileInfo(f);
                    var targetFile = Path.Combine(destDirectories.Directory.FullName, fi2.Name);
                    fi2.CopyTo(targetFile);
                    Process(targetFile, destDirectories.Process);
                    MergedFiles.Add(fi2.Name);
                }
            }  
        }

        public override void MoveFiles()
        {
            foreach (var destDirectories in DestDirectories)
            {
                foreach (string f in SourceDirectory.Files.Where(s => destDirectories.Files.All(d => d.Split('\\').Last() != s.Split('\\').Last())))
                {
                    FileInfo fi2 = new FileInfo(f);
                    string targetFile = Path.Combine(destDirectories.Directory.FullName, fi2.Name);
                    fi2.MoveTo(targetFile);
                    Process(targetFile, destDirectories.Process);
                    MergedFiles.Add(fi2.Name);
                }
            }
            SourceDirectory.Directory.Delete(true);
        }
    }
}
