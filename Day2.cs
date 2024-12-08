using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace AOC24
{
    public class Report
    {
        private List<int> levels;

        public Report(string report_string)
        {
            this.levels = report_string.Split(" ").ToList().ConvertAll(int.Parse);
        }

        private List<bool> check_report(List<int> levels)
        {
            var deltas = levels.Zip(levels.Skip(1), (a, b) => b - a).ToList();

            int increasing = 0;
            int decreasing = 0;
            foreach (var delta in deltas)
            {
                if (delta > 0)
                {
                    increasing++;
                }
                else if (delta < 0)
                {
                    decreasing++;
                }
            }

            if (increasing < decreasing)
            {
                for (int i = 0; i < deltas.Count; i++)
                {
                    deltas[i] = -deltas[i];
                }
            }

            var check = new List<bool>();

            foreach (var delta in deltas)
            {
                check.Add(delta > 0 && delta <= 3);
            }

            return check;
        }
        public bool is_safe_simple()
        {
            var check = this.check_report(this.levels);
            return check.All(x => x == true);
        }

        public bool is_safe()
        {
            var check = this.check_report(this.levels);

            if (check.All(x => x == true))
            {
                return true;
            }

            else
            {
                for (int i = 0; i < this.levels.Count; i++)
                {
                    var level_copy = new List<int>(this.levels);
                    level_copy.RemoveAt(i);
                    check = this.check_report(level_copy);
                    if (check.All(x => x == true))
                    {   
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public class Day2(bool test) : Day(2, test)
    {
        public override void Part1()
        {
            var lines = this.Load();
            Console.WriteLine("\t{0} lines loaded", lines.Count());

            Console.Write("\tPart 1: ");
            var reports = lines.ConvertAll(x => new Report(x));

            var safe_count = reports.Where(x => x.is_safe_simple()).Count();

            Console.WriteLine(safe_count);
        }
        public override void Part2()
        {
            Console.Write("\tPart 2: ");
            var lines = this.Load();
            var reports = lines.ConvertAll(x => new Report(x));

            var safe_count = reports.Where(x => x.is_safe()).Count();

            Console.WriteLine(safe_count);

        }
    }
}
