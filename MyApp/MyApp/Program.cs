using System;

namespace MyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Wrong arguments.");
                return;
            }
            Worker worker = new Worker(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=users;Integrated Security=True");
            switch(args[0])
            {
                case "1":
                    worker.CreateTable();
                    break;
                case "2":
                    if(args.Length < 6)
                    {
                        Console.WriteLine("Error. Wrong arguments count for adding record.");
                        return;
                    } else
                    {
                        int sex = -1;
                        if (args[4] == "m")
                            sex = 0;
                        else if (args[4] == "w")
                            sex = 1;
                        else
                        {
                            Console.WriteLine("Unrecognized user gender.");
                        }
                        DateTime date;
                        try
                        {
                            date = Convert.ToDateTime(args[5]);
                        } catch (Exception ex)
                        {
                            Console.WriteLine("Cannot parse user date.");
                            return;
                        }
                        worker.AddRecord(args[1], args[2], args[3], sex, date);
                    }
                    break;
                case "3":
                    worker.DisplayRecords();
                    break;
                case "4":
                    worker.GenerateRandomRecords(100000);
                    break;
                case "5":
                    worker.SelectRecords();
                    break;
                default:
                    Console.WriteLine("Error. Unrecognized function code.");
                    break;
            }
        }
    }
}
