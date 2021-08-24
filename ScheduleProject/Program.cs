using System;
using ScheduleProject.schedule;

namespace ScheduleProject
{
    class Program
    {
        static Schedule schedule;

        static void PrintResult(DateTime input)
        {
            Console.WriteLine();
            Console.WriteLine($"  Prev = " + schedule.NextPrev(input).ToString("yyyy.MM.dd HH:mm:ss.fff"));
            Console.WriteLine($" Preve = " + schedule.NearestPrev(input).ToString("yyyy.MM.dd HH:mm:ss.fff"));
            Console.WriteLine($"Input  = " + input.ToString("yyyy.MM.dd HH:mm:ss.fff"));
            Console.WriteLine($" Neare = " + schedule.Nearest(input).ToString("yyyy.MM.dd HH:mm:ss.fff"));
            Console.WriteLine($"  Next = " + schedule.Next(input).ToString("yyyy.MM.dd HH:mm:ss.fff"));
        }

        static string InputPattern()
        {
            string pattern;
            string readed;

            while (true)
            {
                Console.Write("Please, input pattern : ");

                readed = Console.ReadLine();

                if (readed.Contains("def"))
                {
                    schedule = new Schedule();
                    return "dafault";
                }

                try
                {
                    schedule = new Schedule(readed);
                    pattern = readed;
                    return pattern;
                }
                catch
                {
                    Console.WriteLine();
                    continue;
                }
            }
        }

        static void CustomCheck()
        {
            string pattern = InputPattern();
            string readed;
            DateTime input;

            while (true)
            {
                Console.WriteLine($"\nPattn = {pattern}");
                Console.Write("Please, input datetime : ");

                readed = Console.ReadLine();

                if (readed.Contains("exit"))
                {
                    break;
                }

                if (readed.Contains("patt"))
                {
                    Console.WriteLine();
                    pattern = InputPattern();
                    continue;
                }

                try
                {
                    input = DateTime.Parse(readed);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
                PrintResult(input);
            }
        }

        static void SpecificCheck()
        {
            // не робит для custom
            //string pattern = "2100.12.31 23:59:59.999";
            //schedule = new Schedule(pattern);
            //DateTime input = DateTime.Parse("2000-01-01 00:00:00.000");

            // 7 nearest
            //string pattern = "*.*.* * *:*:*.1,2,3-5,10-20/3";
            //schedule = new Schedule(pattern);
            //DateTime input = DateTime.Parse("2020-12-31 23:59:59.020");

            // 9 nearest
            //string pattern = "*.*.* * */4:*:*";
            //schedule = new Schedule(pattern);
            //DateTime input = DateTime.Parse("2020-12-31 21:00:00.000");

            // 11 nearest - exception
            //string pattern = "*.9.*/2 1-5 10:00:00.000";
            //schedule = new Schedule(pattern);
            //DateTime input = DateTime.Parse("2020-09-30 12:00:00.000");

            // 13 nearest
            //string pattern = "*:00:00";
            //schedule = new Schedule(pattern);
            //DateTime input = DateTime.Parse("2020-12-31 23:59:59.999");

            // 15 nearest
            //string pattern = "*.*.01 01:30:00";
            //schedule = new Schedule(pattern);
            //DateTime input = DateTime.Parse("2020-12-31 01:30:00.001");

            // 17 nearest
            string pattern = "*.*.32 12:00:00";
            schedule = new Schedule(pattern);
            DateTime input = DateTime.Parse("2020-01-31 12:00:00.001");

            Console.WriteLine($"Patte = {pattern}");
            PrintResult(input);
        }

        static void Main(string[] args)
        {

            //CustomCheck();
            SpecificCheck();
        }
    }
}
