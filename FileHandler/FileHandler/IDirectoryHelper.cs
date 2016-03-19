using System.IO;

namespace FileHandler
{
    public interface IDirectoryHelper<TProcess> where TProcess : PreHandler, new()
    {
        DirectoryInfo Directory { get; set; }
        string[] Files { get; set; }
        TProcess Process { get; set; }
        DirectoryInfo TempStorage { get; set; }
    }
}