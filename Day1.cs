using System;
using System.Collections.Generic;
using System.Text;


namespace AOC23
{
    public class Day1 : Day
    {
        private List<int> lhs;
        private List<int> rhs;

        public Day1(bool test) : base(1, test)
        {
        }

        private void load(List<string> lines)
        {
            List<int> lhs = new();
            List<int> rhs = new();

            foreach (var line in lines)
            {
                var elems = line.Split(" ");

                lhs.Add(int.Parse(elems[0]));
                rhs.Add(int.Parse(elems[3]));
            }
            
            if (lhs.Count != rhs.Count)
            {
                throw new ArgumentException("After parse, LHS and RHS list lengths are not equal!");
            }

            this.rhs = rhs;
            this.lhs = lhs;
        }

        public override void Part1()
        {
            Console.Write("\tPart 1: ");
            var lines = this.Load();

            this.load(lines);

            lhs.Sort();
            rhs.Sort();

            int dist = 0;
            int total_score = 0;
            
            for(int i = 0; i < lhs.Count; i++)
            {
                dist = Math.Abs(lhs[i] - rhs[i]);
                total_score += dist;
            }
            Console.WriteLine("Similarity Score: " + total_score);  
        }

        public override void Part2()
        {
            Console.Write("\tPart 2: ");
            var lines = this.Load();

            this.load(lines);

            int sim_score = 0;
            int total_score = 0;
            foreach(var left_num in this.lhs)
            {
                sim_score = left_num * rhs.FindAll(x => x == left_num).Count;
                total_score += sim_score;
            }

            Console.WriteLine("Similarity Score: " + total_score);
        }
    }
}
