using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TestTaskJuniorNETDeveloper.Interfaces;

namespace TestTaskJuniorNETDeveloper.Services
{
    // Класс для объединения файлов с удалением строк, содержащих указанную подстроку
    public class FileMerger : IFileMerger
    {
        // Объединяет файлы, удаляя строки с заданной подстрокой, и записывает результат в выходной файл
        public void MergeFiles(int numFiles, string deleteString, string outputFile)
        {
            int deletedStrings = 0; // Счетчиекк удаленных строк
            using (StreamWriter writer = new StreamWriter(outputFile)) // Создаем выходной файл
            {
                for (int i = 0; i < numFiles; i++)
                {
                    string filename = $"file_{i}.txt"; // Название нового файла
                    if (File.Exists(filename)) //Проверка его существования
                    {
                        string[] lines = File.ReadAllLines(filename); // Читаем все строки файла
                        foreach (string line in lines)
                        {
                            if (!line.Contains(deleteString))
                            {
                                writer.WriteLine(line); // Запись строки, если не содержит подстроку
                            }
                            else
                            {
                                deletedStrings++; // Увеличение счетчика удаленных строк, если все так есть сочетание
                            }
                        }
                    }
                }
            }
            Console.WriteLine($"Всего удалено: {deletedStrings}");
        }
    }
}
