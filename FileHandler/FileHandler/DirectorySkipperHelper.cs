using System;
using System.IO;

namespace FileHandler
{
    internal class DirectorySkipperHelper<TProcess> : DirectoryHelperBase<TProcess> where TProcess : PreHandler, new()
    {
        public DirectorySkipperHelper(string destDirectory)
        {
            Directory = new DirectoryInfo(destDirectory);
            Process = new TProcess();
            if (Directory.Exists)
                throw new Exception("Directory already exists");
        }
    }
}
