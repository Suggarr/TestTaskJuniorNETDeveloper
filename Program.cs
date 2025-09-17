using TestTaskJuniorNETDeveloper.Interfaces;
using TestTaskJuniorNETDeveloper.Services;

class Program
{
    static void Main()
    {
        const int numFiles = 100; // Количество текстовых файлов
        const int linesPerFile = 1000; // Количество линий в каждом файле - поставил на 1000 т.к. объединенный файл во втором задании бы не открылся
        const int numMin = 1; // Минимальное значение для случайных целых чисел
        const int numMax = 100000000; // Максимальное значение для случайных целых чисел
        const string latinChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"; // Набор латинских символов
        const string russiaChars = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдежзийклмнопрстуфхцчшщъыьэюя"; // Набор русских символов
        const string connectionString = "Data Source=localhost;Initial Catalog=testingDb;Integrated Security=True;"; // Строка подключения к БД

        /* Создание экземпляров сервисов с внедрением зависимостей. Также создал несколько классов и для них интерфейсы чтобы для каждого была только одна ответственность
        , а также чтобы каждый класс должен отвечать за одну задачу*/

        IRandomDataGenerator randomDataGenerator = new RandomDataGenerator(new Random()); // Генератор случайных данных
        IFileGenerator fileGenerator = new FileGenerator(randomDataGenerator, latinChars, russiaChars);// Генератор файлов
        IFileMerger fileMerger = new FileMerger();// Объединитель файлов
        IDataBaseImporter dataBaseImporter = new DataBaseImporter(connectionString); // Импортер данных в БД

        fileGenerator.GenerateFiles(numFiles, linesPerFile, numMin, numMax); // Вызываем метод генерации файлов

        Console.WriteLine("Введите сочетания символов для удаления: ");
        string deleteString = Console.ReadLine();

        Console.WriteLine("Введите название файла для вывода(без формата напишите): ");
        string outputFile = Console.ReadLine() + ".txt";

        fileMerger.MergeFiles(numFiles, deleteString, outputFile); // Вызываем метод объединения файлов с удалением строк, в которых содержится deletestring

        dataBaseImporter.ImportFilesToDatabase(numFiles, numFiles*linesPerFile); // Вызываем метод который импортирует данные из файлов в БД
    }
}