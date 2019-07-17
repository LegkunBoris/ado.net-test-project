using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MyApp
{
    class Worker
    {
        private string _connectionString;
        private SqlConnection _connection;
        public Worker(string connectionString)
        {
            _connectionString = connectionString;
        }
        //Function 1
        public void CreateTable()
        {
            OpenConnection();

            string query = @"CREATE TABLE Users (
                    ID int IDENTITY(1,1) NOT NULL,
                    Name nvarchar(50) NOT NULL,
                    Surname nvarchar(50) NOT NULL,
                    Patronymic nvarchar(50),
                    Sex int NOT NULL,
                    Date datetime NOT NULL,
                    CONSTRAINT pk_id PRIMARY KEY (Name)
                );";
            try
            {
                SqlCommand command = new SqlCommand(query, _connection);
                int nums = command.ExecuteNonQuery();

                Console.WriteLine("Users table created.");
            } catch (SqlException ex)
            {
                Console.WriteLine("Error creating table.\nMessage:" + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }
        //Function 2
        public void AddRecord(string firstname, string surname, string patronymic, int sex, DateTime date)
        {
            OpenConnection();

            string query = "INSERT INTO Users (Name, Surname, Patronymic, Sex, Date) VALUES (@Name, @Surname, @Patronymic, @Sex, @Date)";
            try
            {
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.Add(new SqlParameter("@Name", firstname));
                command.Parameters.Add(new SqlParameter("@Surname", surname));
                command.Parameters.Add(new SqlParameter("@Patronymic", patronymic));
                command.Parameters.Add(new SqlParameter("@Sex", sex));
                command.Parameters.Add(new SqlParameter("@Date", date));
                int nums = command.ExecuteNonQuery();

                Console.WriteLine("User created.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error adding record.\nMessage:" + ex.Message);
            }
            finally
            {
                _connection.Close();
            }

        }
        //Function 3
        public void DisplayRecords()
        {
            OpenConnection();

            string query = "SELECT * FROM Users;";

            try
            {
                SqlCommand command = new SqlCommand(query, _connection);
                SqlDataReader reader = command.ExecuteReader();

                DisplayRecords(reader);

                reader.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error reading records from database.\nMessage:" + ex.Message);
            }
            finally
            {
                _connection.Close();
            }

        }
        //Function 4
        public void GenerateRandomRecords(int number)
        {
            string query = @"Declare @Id int 
                declare @FromDate date = '1950-01-01'
                declare @ToDate date = '2018-12-31'
                Set @Id = 1

                While @Id <= @number
                Begin 
                   Insert Into Users values ((select right(NEWID(),12)),
                              (select right(NEWID(),12)),
			                  (select right(NEWID(),12)),
			                  CAST(RAND() * 2 AS INT),
			                  (select dateadd(day, 
                               rand(checksum(newid()))*(1+datediff(day, @FromDate, @ToDate)), 
                               @FromDate)))
                   Set @Id = @Id + 1
                End";

            OpenConnection();

            try
            {
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.Add(new SqlParameter("@number", number));
                int nums = command.ExecuteNonQuery();

                Console.WriteLine($"[{nums}] users created.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error creating table.\nMessage:" + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }
        //Function 5
        public void SelectRecords()
        {
            string query = @"select top 100 * from Users
                where Name like 'F%' and Sex = 0
                order by Name, Surname;";

            OpenConnection();

            try
            {
                SqlCommand command = new SqlCommand(query, _connection);
                SqlDataReader reader = command.ExecuteReader();

                DisplayRecords(reader);

                reader.Close();

                var stats = _connection.RetrieveStatistics();
                var firstCommandExecutionTimeInMs = (long)stats["ExecutionTime"];
                Console.WriteLine($"Execution time: {firstCommandExecutionTimeInMs}ms.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error reading records from database.\nMessage:" + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }
        private void DisplayExecutionTime()
        {

        }
        private void DisplayRecords(SqlDataReader reader)
        {
            Console.WriteLine("{0,-15}|{1,-15}|{2,-15}|{3,-10}|{4,5}","Firstname","Surname","Patronymic","Age","Sex");
            Console.WriteLine("--------------------------------------------------------------------------------");
            while (reader.Read())
            {
                Console.WriteLine("{0,-15}|{1,-15}|{2,-15}|{3,-10}|{4,5}",
                reader[1], reader[2], reader[3], reader.GetInt32(4) == 0 ? "man" : "woman", DateTime.Today.Year - reader.GetDateTime(5).Year);
            }
        }
        private void OpenConnection()
        {
            try
            {
                _connection = new SqlConnection(_connectionString);
                _connection.StatisticsEnabled = true;
                _connection.Open();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error creating connection.\nMessage:" + ex.Message);
            }
        }
    }
}
