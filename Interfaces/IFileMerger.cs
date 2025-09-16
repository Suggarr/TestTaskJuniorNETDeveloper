namespace TestTaskJuniorNETDeveloper.Interfaces
{
    public interface IFileMerger
    {
        void MergeFiles(int numFiles, string deleteString, string outputFile);
    }
}