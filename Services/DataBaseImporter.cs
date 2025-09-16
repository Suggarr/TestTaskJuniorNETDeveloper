using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; 

namespace TestTaskJuniorNETDeveloper.Services
{
    public class DataBaseImporter : IDataBaseImporter
    {
        private readonly string _connectionString;
        public DataBaseImporter(string connnectionString)
        {
            _connectionString = connnectionString;
        }

        public void ImportFilesToDatabase(int numFiles)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                conn.Open();
                string checkTableQuery = "Select 1 From sys.tables WHERE name = 'data'";
                using (SqlCommand Checkcmd = new SqlCommand(checkTableQuery, conn))
                {
                    if (Checkcmd.ExecuteScalar() == null)
                    {
                        string createTableQuery = @"
                            Create Table data (
                                recordDate Date,
                                latinText NVARCHAR(50),
                                russianText NVARCHAR(50),
                                number INT,
                                float_number FLOAT)";
                        using (SqlCommand createCmd = new SqlCommand(createTableQuery, conn))
                        {
                            createCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string clearTableQuery = "TRUNCATE TABLE data";
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
                            int totalLines = lines.Length;
                            int importedLines = 0;
                            using SqlTransaction transaction = conn.BeginTransaction();
                            foreach (string line in lines)
                            {
                                string[] parts = line.Split("||");
                                if (parts.Length == 6)
                                {
                                    try
                                    {
                                        string insertQuery = @"
                                            INSERT INTO data(recordDate, latinText, russianText, number, float_number)
                                            VALUES (@date, @latin, @russian, @number, @float_number)";
                                        using (SqlCommand insertDataQuery = new SqlCommand(insertQuery, conn))
                                        {
                                            insertDataQuery.Parameters.AddWithValue("@date", DateOnly.Parse(parts[0]));
                                            insertDataQuery.Parameters.AddWithValue("@latin", parts[1]);
                                            insertDataQuery.Parameters.AddWithValue("@russian", parts[2]);
                                            insertDataQuery.Parameters.AddWithValue("@number", parts[3]);
                                            insertDataQuery.Parameters.AddWithValue("@float_number", parts[4]);

                                            insertDataQuery.ExecuteNonQuery();
                                            importedLines++;
                                            Console.WriteLine($"Из файла {filename} импортировано {importedLines} строк");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.ToString());
                                    }
                                }
                            }
                            transaction.Commit();
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
