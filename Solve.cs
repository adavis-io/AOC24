// See https://aka.ms/new-console-template for more information


class AOC
{
    static void Main(string[] args)
    {
        List<AOC24.Day> days = new();

        bool test = args.Contains("--test");

        if (test)
        {
            Console.WriteLine("Running with test data!");
        }
        else
        {
            Console.WriteLine("Running with full data"); 
        }

        //days.Add(new AOC24.Day1(test));
        days.Add(new AOC24.Day2(test));

        foreach (var day in days)
        {

            Console.WriteLine(day.name);

            long part1millis, part2millis;

            var solvetimer = System.Diagnostics.Stopwatch.StartNew();
            
            day.Part1();
            
            solvetimer.Stop();
            
            part1millis = solvetimer.ElapsedMilliseconds;

            solvetimer.Restart();
            
            day.Part2();

            solvetimer.Stop();
            part2millis = solvetimer.ElapsedMilliseconds;

            Console.WriteLine(String.Format("\tSolved Part 1 in {0}ms, Part 2 in {1}ms", part1millis, part2millis));
        }
    }
}