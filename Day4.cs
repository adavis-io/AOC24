using System;
using System.Collections.Generic;
using System.Text;


namespace AOC24
{
    public class Day4(bool test) : Day(4, test)
    {
        private List<string> puzzle = new List<string>();

        private char[,] get_region(int x, int y, int width, int height)
        {
            char[,] region = new char[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    region[i,j] = this.puzzle[y + i][x + j];
                }
            }
            return region;
        }
        private bool search_at(string search, int x, int y, int dir_x, int dir_y)
        {
            if (dir_x == 0 && dir_y == 0)
            {
                return false;
            }

            // no match if search will run off the edge
            int ll = search.Length - 1;
            if (x + ll * dir_x < 0 || x + ll * dir_x > this.puzzle[y].Length - 1)
            {
                return false;
            }
            if (y + ll * dir_y < 0 || y + ll * dir_y > this.puzzle.Count - 1)
            {
                return false;
            }

            for (int i = 0; i < search.Length; i++)
            {
                if (this.puzzle[y + i * dir_y][x + i * dir_x] != search[i])
                {
                    return false;
                }
            }
            return true;
        }
        private bool test_part2_at(int x, int y)
        {
            if (this.puzzle[y][x] != 'A')
            {
                return false;
            }
            // no match if search will run off the edge
            if (x - 1 < 0 || x + 1 > this.puzzle[y].Length - 1)
            {
                return false;
            }
            if (y - 1 < 0 || y + 1 > this.puzzle.Count - 1)
            {
                return false;
            }

            if (search_at("MAS", x - 1, y - 1, 1, 1))
            {
                if (search_at("MAS", x - 1, y + 1, 1, -1))
                {
                    return true;
                }
                else if (search_at("MAS", x + 1, y - 1, -1, 1))
                {
                    return true;
                }

            }
            else if (search_at("MAS", x + 1, y + 1, -1, -1))
            {
                if (search_at("MAS", x - 1, y + 1, 1, -1))
                {
                    return true;
                }
                else if (search_at("MAS", x + 1, y - 1, -1, 1))
                {
                    return true;
                }
            }
            
            return false;
        }

        private int count_part1_from(int x, int y)
        {
            int[] directions = [-1, 0, 1];
            int count = 0;
            foreach(int dir_x in directions)
            {
                foreach (int dir_y in directions)
                {
                    if (dir_x == 0 && dir_y == 0)
                    {
                        continue;
                    }
                    if (search_at("XMAS", x, y, dir_x, dir_y))
                    {
                        count += 1;
                    }
                }
            }

            return count;
        }
        private int count_part2_at(int x, int y)
        {
            int[] directions = [-1, 0, 1];
            int count = 0;

            var region = get_region(x - 1, y - 1, 3, 3);

            if (test_part2_at(x, y))
            {
                count += 1;
            }
           
            return count;
        }
        public override void Part1()
        {
            Console.Write("\tPart 1: ");
            this.puzzle = this.Load();
            
            int count = 0;

            for (int y = 0; y < this.puzzle.Count; y++)
            {
                for (int x = 0; x < this.puzzle[y].Length; x++)
                {
                    if (this.puzzle[y][x] == 'X')
                    {
                        count += count_part1_from(x, y);
                    }
                }
            }
            Console.WriteLine(count);
        }
        public override void Part2()
        {
            Console.Write("\tPart 2: ");
            this.puzzle = this.Load();

            int count = 0;

            for (int y = 1; y < this.puzzle.Count - 1; y++)
            {
                for (int x = 1; x < this.puzzle[y].Length - 1; x++)
                {
                    count += count_part2_at(x, y);    
                }
            }
            Console.WriteLine(count);
        }
    }
}
