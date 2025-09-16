namespace TestTaskJuniorNETDeveloper.Interfaces
{
    public interface IDataBaseImporter
    {
        void ImportFilesToDatabase(int numFiles, int totalLines);
    }
}