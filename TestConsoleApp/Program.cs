using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TestSomthing();
            TestCinfiguration();
            TestPostgresql();
            TestPostgresqlBulkCopy();
            TestPostgresqlEF();
        }

        private static void TestPostgresql()
        {
            var connectoinString = "Host=localhost;User ID=testuser;Password=123654;Database=test;";
            using (var connetion = new NpgsqlConnection(connectoinString))
            {
                var cmd = connetion.CreateCommand();
                cmd.CommandText = "SELECT * FROM business.persion";
                connetion.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader.GetGuid(0)},HireDate Type:{reader[5].GetType()}");
                }
                
                connetion.Close();
            }
        }        

        private static void TestPostgresqlBulkCopy()
        {
            //http://www.npgsql.org/doc/copy.html?q=bulk
            var connectoinString = "Host=localhost;User ID=testuser;Password=123654;Database=test;";
            Random random = new Random();
            var nameIndex = 0L;
            
            using (var connetion = new NpgsqlConnection(connectoinString))
            {
                var cmd = connetion.CreateCommand();
                //cmd.CommandText = "SELECT COALESCE(name,'name0') AS name FROM business.persion ORDER BY id DESC LIMIT 1;";
                //cmd.CommandText = "SELECT coalesce(max(seq),0) AS seq FROM business.persion;";
                cmd.CommandText = "SELECT CASE WHEN is_called=TRUE THEN last_value ELSE last_value-1 END AS cvalue FROM business.persion_seq_seq;";
                connetion.Open();
                nameIndex = (long)cmd.ExecuteScalar();

                // Import columns to table
                using (var writer = connetion.BeginBinaryImport("COPY business.persion(name,age,wage,hiredate) FROM STDIN (FORMAT BINARY)"))
                {
                    var name = new string(new[]
                    {
                        (char) random.Next(65, 90), (char) random.Next(97, 120), (char) random.Next(97, 120),
                        (char) random.Next(97, 120)
                    });
                    writer.StartRow();
                    writer.Write($"{name}{++nameIndex}");
                    writer.Write(random.Next(18,28), NpgsqlDbType.Integer);
                    writer.Write(123456.78+random.Next(20,500), NpgsqlDbType.Money);
                    writer.Write(DateTime.Now.AddDays(random.Next(-1000,-30)).Date, NpgsqlDbType.Date);
                    
                    name = new string(new[]
                    {
                        (char) random.Next(65, 90), (char) random.Next(97, 120), (char) random.Next(97, 120),
                        (char) random.Next(97, 120)
                    });
                    writer.StartRow();
                    writer.Write($"{name}{++nameIndex}");
                    writer.Write(random.Next(18,28), NpgsqlDbType.Integer);
                    writer.Write(123456.78+random.Next(20,500), NpgsqlDbType.Money);
                    writer.Write(DateTime.Now.AddDays(random.Next(-1000,-30)).Date, NpgsqlDbType.Date);
                }

                // Export columns from table
                using (var reader = connetion.BeginBinaryExport("COPY business.persion (name,age,wage,hiredate) TO STDOUT (FORMAT BINARY)"))
                {
                    reader.StartRow();
                    Console.WriteLine(reader.Read<string>());
                    Console.WriteLine(reader.Read<int>(NpgsqlDbType.Integer));
                    Console.WriteLine(reader.Read<decimal>(NpgsqlDbType.Money));
                    Console.WriteLine(reader.Read<DateTime>(NpgsqlDbType.Date));

                    reader.StartRow();
                    reader.Skip();
                    Console.WriteLine(reader.IsNull);   // Null check doesn't consume the column
                    Console.WriteLine(reader.Read<int>());
                    Console.WriteLine(reader.Read<decimal>(NpgsqlDbType.Money));
                    Console.WriteLine(reader.Read<DateTime>(NpgsqlDbType.Date));

                    reader.StartRow();    // Last StartRow() returns -1 to indicate end of data
                }
                connetion.Close();
            }
        }

        private static void TestPostgresqlEF()
        {
            //http://www.npgsql.org/efcore/index.html
            //dotnet add package Microsoft.EntityFrameworkCore.Design
            //dotnet ef dbcontext scaffold "Host=localhost;Database=test;Username=testuser;Password=123654" Npgsql.EntityFrameworkCore.PostgreSQL
            var contex=new testContext();
            var persons = contex.Persion;
            foreach (var person in persons)
            {
                Console.WriteLine($"{person.Id},{person.Name}");
            }
        }

        static void TestSomthing()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var random=new Random();
            Console.WriteLine(new string(new[]{(char)random.Next(65,90),(char)random.Next(97,120),(char)random.Next(97,120),(char)random.Next(97,120)}));
            Console.WriteLine("Hello World!");
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Console.WriteLine(Directory.GetCurrentDirectory());
            Console.WriteLine($"{stopwatch.ElapsedMilliseconds}ms");
            var list = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                list.Add(i);
            }

            var d3 = from i in list where i % 3 == 0 select i;
            var sum = 0;
            foreach (var item in d3)
            {
                sum += item;
            }

            Console.WriteLine($@"{sum}{Environment.NewLine}");
        }

        static void TestCinfiguration()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
            var applicationName = configuration["ApplicationName"];
            Console.WriteLine(applicationName);
        }
    }

    #region person
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public string GetPersonInfo()
        {
            return $"Name={Name},Age={Age},PhoneNumber={PhoneNumber}";
        }
    }
    #endregion
}