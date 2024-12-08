using System;
using System.Collections.Generic;
using System.Text;


namespace AOC24
{
    
    public class Day5(bool test) : Day(5, test)
    {
        private Dictionary<int, List<int>> page_requirements = new Dictionary<int, List<int>>();
        private List<List<int>> updates = new List<List<int>>();

        private bool is_valid(List<int> update)
        {
            for (int i = 0; i < update.Count; i++)
            {
                for (int j = i + 1; j < update.Count; j++)
                {
                    if (this.page_requirements.ContainsKey(update[i]) && this.page_requirements[update[i]].Contains(update[j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private List<int> reorder(List<int> update)
        {

            var dependent_pages = new Dictionary<int, List<int>>();
            foreach(int page in update)
            {
                if (!this.page_requirements.ContainsKey(page))
                {
                    dependent_pages[page] = new List<int>();
                }
                else
                {
                    dependent_pages[page] = this.page_requirements[page].Intersect(update).ToList();
                }
            }
            

            var ordered = new List<int>(update);
            ordered.Sort((a, b) => dependent_pages[a].Count - dependent_pages[b].Count);

            return ordered;
        }

        private void parse_input(List<string> lines)
        {
            var page_requirements = new Dictionary<int, List<int>>();
            var updates = new List<List<int>>();

            foreach (var line in lines)
            {
                if (line.Contains("|"))
                {
                    var rule_str = line.Split("|");
                    var second_page = int.Parse(rule_str[1]);
                    if (!page_requirements.ContainsKey(second_page))
                    {
                        page_requirements[second_page] = new List<int>();
                    }
                    page_requirements[second_page].Add(int.Parse(rule_str[0]));
                }
                else if (line.Contains(","))
                {

                    var update = new List<int>();

                    foreach (var u in line.Split(','))
                    {
                        update.Add(int.Parse(u));
                    }
                    updates.Add(update);
                }

            }
            this.page_requirements = page_requirements;
            this.updates = updates;
        }
        public override void Part1()
        {
            Console.Write("\tPart 1: ");
            var lines = this.Load();
            this.parse_input(lines);
            
            int total = 0;
            foreach (var update in updates)
            {
                if (is_valid(update))
                {
                    total += update[update.Count / 2];
                }
            }
            Console.WriteLine(total);
        }
        public override void Part2()
        {
            Console.Write("\tPart 2: ");
            var lines = this.Load();
            this.parse_input(lines);

            int total = 0;
            foreach (var update in updates)
            {
                if (!is_valid(update))
                {
                    
                    total += reorder(update)[update.Count / 2];
                    
                }
            }
            Console.WriteLine(total);
        }
    }
}
