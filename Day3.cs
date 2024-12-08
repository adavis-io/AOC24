using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace AOC24
{
    public class Day3(bool test) : Day(3, test)
    {
        public override void Part1()
        {
            Console.Write("\tPart 1: ");
            var lines = this.Load();

            var mul_re = new Regex(@"mul\((\d+),(\d+)\)");

            string input = string.Join("/n", lines);

            var muls = mul_re.Matches(input);

            int total = 0;
            foreach (Match m in muls)
            {
                total += int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value);
            }

            Console.WriteLine("Total: " + total);
        }
        public override void Part2()
        {
            Console.Write("\tPart 2: ");
            var lines = this.Load();
            string input = string.Join("/n", lines);

            bool mul_enabled = true;
            bool in_arg = false;

            string arg_buf = "";

            var mul_args = new int[2];

            int total = 0;

            for(int start = 0; start < input.Length; start++)
            {
                if (mul_enabled)
                {
                    if (start + 7 < input.Length && input.Substring(start, 7) == "don't()")
                    {
                        mul_enabled = false;
                        start += 6;
                    }
                    else if (start + 4 < input.Length && input.Substring(start, 4) == "mul(")
                    {
                        start += 3;
                        arg_buf = "";
                        in_arg = true;
                    }
                    else if (in_arg)
                    {
                        switch(input[start])
                        {
                            
                            case ')':
                                mul_args[1] = int.Parse(arg_buf);

                                total += mul_args[0] * mul_args[1];
                                in_arg = false;
                                break;
                            
                            case ',':
                                mul_args[0] = int.Parse(arg_buf);
                                arg_buf = "";
                                break;
                            
                            case char n when (n >= '0' && n <= '9'):
                                arg_buf += input[start];
                                break;

                            default:
                                in_arg = false;
                                break;
                        }
                    }
                }
                else
                {
                    if (start + 4 < input.Length && input.Substring(start, 4) == "do()")
                    {
                        start += 3;
                        mul_enabled = true;
                    }
                }
            }
            Console.WriteLine("Total: " + total);
        }
    }
}
