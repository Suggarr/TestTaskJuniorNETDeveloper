using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TestTaskJuniorNETDeveloper.Interfaces;

namespace TestTaskJuniorNETDeveloper.Services
{
    public class FileMerger : IFileMerger
    {
        public void MergeFiles(int numFiles, string deleteString, string outputFile)
        {
            int deletedStrings = 0;
            using (StreamWriter writer = new StreamWriter(outputFile))
            {
                for (int i = 0; i < numFiles; i++)
                {
                    string filename = $"file_{i}.txt";
                    if (File.Exists(filename))
                    {
                        string[] lines = File.ReadAllLines(filename);
                        foreach (string line in lines)
                        {
                            if (!line.Contains(deleteString))
                            {
                                writer.WriteLine(line);
                            }
                            else
                            {
                                deletedStrings++;
                            }
                        }
                    }
                }
            }
            Console.WriteLine($"Всего удалено: {deletedStrings}");
        }
    }
}
