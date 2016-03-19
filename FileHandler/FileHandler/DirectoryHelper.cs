using System.IO;

namespace FileHandler
{
    internal class DirectoryHelper<TProcess> : DirectoryHelperBase<TProcess> where TProcess: PreHandler, new()
    {
        public DirectoryHelper(string destDirectory)
        {
            Directory = new DirectoryInfo(destDirectory);

            if (!Directory.Exists)
            {
                Directory.Create();
                Process = new TProcess();
                Files = new string[0];
                return;
            }

            Files = System.IO.Directory.GetFiles(Directory.FullName);
            TempStorage = Directory.Parent?.CreateSubdirectory(Path.GetRandomFileName());
            CopyFiles();

        }

        private void CopyFiles()
        {
            if (string.IsNullOrEmpty(TempStorage?.FullName)) return;

            foreach (string f in Files)
            {
                FileInfo fi2 = new FileInfo(f);
                string targetFile = Path.Combine(TempStorage?.FullName, fi2.Name);
                fi2.CopyTo(targetFile);
            }
        }
    }
}