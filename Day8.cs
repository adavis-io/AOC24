using System;
using System.Collections.Generic;
using System.Text;


namespace AOC24
{
    public class MapEntry
    {
        public List<char> Antinodes = new();
        public char Tower = '.';
        public int[] Location = [-1, -1];

        public MapEntry(int[] position) 
        {
            this.Location = position;
        }
        public override string ToString()
        {
            if (this.Tower != '.')
            {
                return Tower.ToString();
            }
            else if (Antinodes.Count == 0)
            {
                return ".";
            }
            else
            {
                return Antinodes.Count.ToString();
            }
        }
    }
    public class RadioTowers
    {
        private MapEntry[,] map;
        private Dictionary<char, List<int[]>> frequencies;       

        public RadioTowers(List<string> map_def)
        {

            this.map = new MapEntry[map_def[0].Length, map_def.Count];

            this.frequencies = new Dictionary<char, List<int[]>>();

            for (int j = 0; j < map_def.Count; j++)
            {
                var line = map_def[j];
                for (int i = 0; i < line.Length; i++)
                {
                    this.map[i, j] = new MapEntry([i, j]);
                    if (line[i] != '.')
                    {
                        this.map[i, j].Tower = line[i];
                        if (!this.frequencies.ContainsKey(line[i]))
                        {
                            this.frequencies[line[i]] = new List<int[]>();
                        }
                        this.frequencies[line[i]].Add([i, j]);
                    }
                }
            }
        }
        private bool TestPointInMap(int[] point)
        {
            if (point[0] < 0 || point[1] < 0)
            { 
                return false; 
            }
            if (point[0] >= this.map.GetLength(0) || point[1] >= this.map.GetLength(1))
            {
                return false;
            }
            return true;
        }

        private List<int[]> GetAntennaPairAntinodes1(int[] antenna1, int[] antenna2)
        {
            if (antenna1.Length != 2 || antenna2.Length != 2)
            {
                throw new ArgumentException("Antennas must be 2D coordinates");
            }
            var antinodes = new List<int[]>();

            int[] distance = [0, 0];

            distance[0] = (antenna1[0] - antenna2[0]);
            distance[1] = (antenna1[1] - antenna2[1]);

            int[] node_0 = [antenna1[0] + distance[0], antenna1[1] + distance[1]];
            int[] node_1 = [antenna2[0] - distance[0], antenna2[1] - distance[1]];

            if (TestPointInMap(node_0))
            {
                antinodes.Add(node_0);
            }
            if (TestPointInMap(node_1))
            {
                antinodes.Add(node_1);
            }

            return antinodes;
        }


        private List<int[]> GetAntennaPairAntinodes2(int[] antenna1, int[] antenna2)
        {
            if (antenna1.Length != 2 || antenna2.Length != 2)
            {
                throw new ArgumentException("Antennas must be 2D coordinates");
            }
            var antinodes = new List<int[]>();

            antinodes.Add((int[])antenna1.Clone());
            antinodes.Add((int[])antenna2.Clone());

            int[] distance = [0, 0];

            distance[0] = (antenna1[0] - antenna2[0]);
            distance[1] = (antenna1[1] - antenna2[1]);

            int[] loc1 = [0, 0];
            int[] loc2 = [0, 0];
            antenna1.CopyTo(loc1, 0);
            antenna2.CopyTo(loc2, 0);

            while (true)
            {
                loc1[0] += distance[0];
                loc1[1] += distance[1];
                loc2[0] -= distance[0];
                loc2[1] -= distance[1];
                if (!TestPointInMap(loc1) && !TestPointInMap(loc2))
                {
                    break;
                }
                if (TestPointInMap(loc1))
                {
                    antinodes.Add((int[])loc1.Clone());
                }
                if (TestPointInMap(loc2))
                {
                    antinodes.Add((int[])loc2.Clone());
                }

            }
            return antinodes;
        }
        public int FindAntinodes(bool limit)
        {
            foreach (var frequency in frequencies.Keys)
            {
                for (int i = 0; i < frequencies[frequency].Count - 1; i++)
                {
                    for (int j = i + 1; j < frequencies[frequency].Count; j++)
                    {
                        List<int[]> cur_antinodes;
                        if (limit)
                        {
                            cur_antinodes = GetAntennaPairAntinodes1(frequencies[frequency][i], frequencies[frequency][j]);
                        }
                        else
                        {
                            cur_antinodes = GetAntennaPairAntinodes2(frequencies[frequency][i], frequencies[frequency][j]);
                        }
                        foreach (var antinode in cur_antinodes)
                        {
                            map[antinode[0], antinode[1]].Antinodes.Add(frequency);
                            
                        }
                    }

                }
            }

            int antinodes = 0;
            foreach (var loc in map)
            {
                if (loc.Antinodes.Count != 0)
                {
                    antinodes += 1;
                }
            }
            return antinodes;

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < map.GetLength(1); j++)
            {
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    sb.Append(map[i, j].ToString());
                }
                sb.Append('\n');
            }
            return sb.ToString();
        }
    }
    public class Day8(bool test) : Day(8, test)
    {
        

        public override void Part1()
        {
            Console.Write("\tPart 1: ");
            var lines = this.Load();

            RadioTowers rt = new RadioTowers(lines);

            //Console.WriteLine();

            Console.WriteLine(rt.FindAntinodes(true));
            //Console.WriteLine(rt.ToString());
        }
        public override void Part2()
        {
            Console.Write("\tPart 2: ");
            var lines = this.Load();
            RadioTowers rt = new RadioTowers(lines);

            //Console.WriteLine();

            Console.WriteLine(rt.FindAntinodes(false));
            //Console.WriteLine(rt.ToString());

        }
    }
}
