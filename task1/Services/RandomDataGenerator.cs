using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTaskJuniorNETDeveloper.Interfaces;

namespace TestTaskJuniorNETDeveloper.Services
{
    // Класс для генерации случайных данных 
    public class RandomDataGenerator : IRandomDataGenerator
    {
        private readonly Random _random;
        public RandomDataGenerator(Random random)
        {
            _random = random;
        }

        // Генерирует случайную дату за последние 5 лет
        public DateOnly RandomDay()
        {
            DateTime endDate = DateTime.Now;
            DateTime startDate = endDate.AddYears(-5);
            int daysDifference = (endDate - startDate).Days;
            int randomDays = _random.Next(daysDifference + 1);
            return DateOnly.FromDateTime(startDate.AddDays(randomDays)); // Возвращаем случайную дату преобразованную в DateOnly
        }

        // Генерирует случайную строку из указанного набора символов заданной длины
        public string RandomChars(string chars, int length)
        {
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[_random.Next(chars.Length)]; // Выбираем случайный символ из строки chars и заносим его в result
            }
            return new string(result);
        }

        // Генерирует случайное целое число в диапазоне от min по max
        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max + 1);
        }

        // Генерирует случайное дробное число в диапазоне от min до max с указанным количеством знаков после запятой 
        public double RandomDouble(double min, double max, int decimalPlaces)
        {
            double value = _random.NextDouble() * (max - min) + min;
            return Math.Round(value, decimalPlaces);
        }
    }
}
