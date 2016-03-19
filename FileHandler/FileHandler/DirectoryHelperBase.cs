using System.IO;

namespace FileHandler
{
    public abstract class DirectoryHelperBase<TProcess> : IDirectoryHelper<TProcess> where TProcess : PreHandler, new()
    {
        public DirectoryInfo TempStorage { get; set; }
        public DirectoryInfo Directory { get; set; }
        public string[] Files { get; set; }
        public TProcess Process { get; set; }
    }
}