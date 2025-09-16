using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTaskJuniorNETDeveloper.Interfaces;

namespace TestTaskJuniorNETDeveloper.Services
{
    public class FileGenerator : IFileGenerator
    {
        private readonly IRandomDataGenerator _randomDataGenerator;
        private readonly string _latinChars;
        private readonly string _russianChars;

        public FileGenerator(IRandomDataGenerator randomDataGenerator, string latinChars, string russianChars)
        {
            _randomDataGenerator = randomDataGenerator;
            _latinChars = latinChars;
            _russianChars = russianChars;
        }

        public void GenerateFiles(int numFiles, int linesPerFile, int minNum, int maxNum)
        {
            for (int i = 0; i < numFiles; i++)
            {
                string filename = $"file_{i}.txt";
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    for (int j = 0; j < linesPerFile; j++)
                    {
                        writer.WriteLine($"{_randomDataGenerator.RandomDay()}||{_randomDataGenerator.RandomChars(_latinChars, 10)}||{_randomDataGenerator.RandomChars(_russianChars, 10)}" +
                            $"||{_randomDataGenerator.RandomNumber(minNum, maxNum)}||{_randomDataGenerator.RandomDouble(1, 20, 8)}||");
                    }
                }

            }
        }
    }
}
