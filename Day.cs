

namespace AOC23
{
    public class Day
    {
        public string filename = "";
        public string name = "";
        public bool test = false;
        public Day(int daynum, bool test)
        {
            this.filename = String.Format("day{0}{1}.txt", daynum, test ? "_test" : "");
            this.name = String.Format("Day {0}", daynum);
            this.test = test;
        }
        public List<string> Load()
        {
            var filename = this.filename;
            var inputfile = String.Format("../../../inputs/{0}", filename);
            var input_path = Path.GetFullPath(inputfile);

            List<string> lines = new List<string>();
            if (!File.Exists(input_path))
            {
                Console.WriteLine(String.Format("File not found: {0}", input_path));
                return lines;
            }

            using (StreamReader sr = File.OpenText(input_path))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    lines.Add(s);
                }
            }
            return lines;
        }
        public virtual void Part1()
        {

        }

        public virtual void Part2()
        {

        }
    }
}
