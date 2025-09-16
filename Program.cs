using TestTaskJuniorNETDeveloper.Interfaces;
using TestTaskJuniorNETDeveloper.Services;

class Program
{
    static void Main()
    {
        const int numFiles = 100;
        const int linesPerFile = 1000;
        const int numMin = 1;
        const int numMax = 100000000;
        const string latinChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        const string russiaChars = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдежзийклмнопрстуфхцчшщъыьэюя";
        const string connectionString = "Data Source=localhost;Initial Catalog=testingDb;Integrated Security=True;";

        IRandomDataGenerator randomDataGenerator = new RandomDataGenerator(new Random());
        IFileGenerator fileGenerator = new FileGenerator(randomDataGenerator, latinChars, russiaChars);
        IFileMerger fileMerger = new FileMerger();
        IDataBaseImporter dataBaseImporter = new DataBaseImporter(connectionString);

        fileGenerator.GenerateFiles(numFiles, linesPerFile, numMin, numMax);

        Console.WriteLine("Введите сочетания символов для удаления: ");
        string deleteString = Console.ReadLine();

        Console.WriteLine("Введите название файла для вывода(без формата напишите): ");
        string outputFile = Console.ReadLine() + ".txt";

        fileMerger.MergeFiles(numFiles, deleteString, outputFile);

        dataBaseImporter.ImportFilesToDatabase(numFiles, numFiles*linesPerFile);
    }
}