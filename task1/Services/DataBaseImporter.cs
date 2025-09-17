using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using TestTaskJuniorNETDeveloper.Interfaces;

namespace TestTaskJuniorNETDeveloper.Services
{
    // Класс для импорта данных из текстовых файлов в базу данных
    public class DataBaseImporter : IDataBaseImporter
    {
        private readonly string _connectionString;
        public DataBaseImporter(string connnectionString)
        {
            _connectionString = connnectionString;
        }

        // Импортирует данные из файлов в таблицу
        public void ImportFilesToDatabase(int numFiles, int totalLines)
        {
            int importedLines = 0; // Счетчик импортированных строк
            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString); // Создаем подключение к базе данных
                conn.Open(); // Открываем подключение

                string checkTableQuery = "Select 1 From sys.tables WHERE name = 'data'"; // Проверяем наличие таблицы data
                using (SqlCommand Checkcmd = new SqlCommand(checkTableQuery, conn))
                {
                    if (Checkcmd.ExecuteScalar() == null) // Создаем таблицу, если она не существует
                    {
                        string createTableQuery = @"
                            CREATE TABLE dbo.data (
                                recordDate Date,
                                latinText NVARCHAR(50),
                                russianText NVARCHAR(50),
                                number BIGINT,
                                float_number FLOAT)";
                        using (SqlCommand createCmd = new SqlCommand(createTableQuery, conn))
                        {
                            createCmd.ExecuteNonQuery(); 
                        }
                    }
                    else // Очищаем таблицу, если она существует
                    {
                        string clearTableQuery = "TRUNCATE TABLE dbo.data";
                        using (SqlCommand clearCmd = new SqlCommand(clearTableQuery, conn))
                        {
                            clearCmd.ExecuteNonQuery();
                        }
                    }

                    for (int i = 0; i < numFiles; i++)
                    {
                        string filename = $"file_{i}.txt";
                        if (File.Exists(filename))
                        {
                            string[] lines = File.ReadAllLines(filename);
                            using SqlTransaction transaction = conn.BeginTransaction(); // Начинаем транзакцию
                            try
                            {
                                foreach (string line in lines)
                                {
                                    string[] parts = line.Split("||");
                                    if (parts.Length == 6) // Проверяем, что строка содержит 6 частей (включая пустую в конце)
                                    {
                                        try
                                        {
                                            // Вставляем данные в таблицу ис
                                            string insertQuery = @"
                                            INSERT INTO dbo.data(recordDate, latinText, russianText, number, float_number)
                                            VALUES (@date, @latin, @russian, @number, @float_number)";
                                            using (SqlCommand insertDataQuery = new SqlCommand(insertQuery, conn, transaction))
                                            {
                                                // Добавлеяем значения параметров и выполняем запрос
                                                insertDataQuery.Parameters.AddWithValue("@date", DateTime.Parse(parts[0]).Date);
                                                insertDataQuery.Parameters.AddWithValue("@latin", parts[1]);
                                                insertDataQuery.Parameters.AddWithValue("@russian", parts[2]);
                                                insertDataQuery.Parameters.AddWithValue("@number", int.Parse(parts[3]));
                                                insertDataQuery.Parameters.AddWithValue("@float_number", double.Parse(parts[4]));

                                                insertDataQuery.ExecuteNonQuery();
                                                importedLines++; // Увеличиваем счетчик импортированных строк

                                                Console.WriteLine($"Из файла {filename} импортировано {importedLines} строк, осталось {totalLines - importedLines}");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.ToString());
                                        }
                                    }
                                }
                                transaction.Commit(); // Фиксируем транзакцию
                            }
                            catch(Exception ex)
                            {
                                transaction.Rollback(); // Откатываем транзакцию в случае ошибки
                                Console.WriteLine(ex.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
