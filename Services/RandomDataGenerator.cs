using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTaskJuniorNETDeveloper.Interfaces;

namespace TestTaskJuniorNETDeveloper.Services
{
    public class RandomDataGenerator : IRandomDataGenerator
    {
        private readonly Random _random;
        public RandomDataGenerator(Random random)
        {
            _random = random;
        }

        public DateOnly RandomDay()
        {
            DateTime endDate = DateTime.Now;
            DateTime startDate = endDate.AddYears(-5);
            int daysDifference = (endDate - startDate).Days;
            int randomDays = _random.Next(daysDifference + 1);
            return DateOnly.FromDateTime(startDate.AddDays(randomDays));
        }

        public string RandomChars(string chars, int length)
        {
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[_random.Next(chars.Length)];
            }
            return new string(result);
        }

        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max + 1);
        }

        public double RandomDouble(double min, double max, int decimalPlaces)
        {
            double value = _random.NextDouble() * (max - min) + min;
            return Math.Round(value, decimalPlaces);
        }
    }
}
