namespace FileHandler
{
    public interface IFileHandler
    {
        void CopyFiles();
        void MoveFiles();
        void DeleteTemps();
    }
}