using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;


namespace AOC24
{
    public class Equation
    { 
        public enum Operation
        {
            Add,
            Multiply,
            Concatenate
        }

        public List<Operation> valid_operations;
        public long test_value;
        public List<int> inputs;
        public List<List<Operation>> operator_paths;

        public bool valid;

        public Equation(string equation)
        {
            var parts = equation.Split(":");

            this.test_value = long.Parse(parts[0].Trim());

            this.inputs = new List<int>();
            foreach (var part in parts[1].Trim().Split(" "))
            {
                this.inputs.Add(int.Parse(part));
            }

            this.valid_operations = new List<Operation>([Operation.Add, Operation.Multiply]);
            this.operator_paths = new List<List<Operation>>();
            this.valid = false;

        }

        private int resolve_recursive(long partial, int location, Operation op, List<Operation> op_chain)
        {

            if (op == Operation.Add)
            {
                partial = checked (partial + this.inputs[location]);
            }
            else if (op == Operation.Multiply)
            {
                partial = checked (partial * this.inputs[location]);
            }
            else if (op == Operation.Concatenate)
            {
                var multiplier = (long)Math.Pow(10, Math.Floor(Math.Log10(this.inputs[location])) + 1);
                
                partial = checked (partial * multiplier);
                partial += this.inputs[location];
            }

            if (partial > this.test_value)
            {
                return -1;
            }
            
            if (location == this.inputs.Count - 1)
            {
                if (partial == this.test_value)
                {
                    this.operator_paths.Add(op_chain);
                    this.valid = true;
                    return 1;
                }
                else
                {
                    return -1;
                }
            }

            int count = 0;
            bool valid = false;

            foreach (var operation in this.valid_operations)
            {
                var op_chain_temp = new List<Operation>(op_chain)
                {
                    operation
                };

                var valid_count_temp = this.resolve_recursive(partial, location + 1, operation, op_chain_temp);

                if (valid_count_temp > 0)
                {
                    valid = true;
                    count += valid_count_temp;
                }

            }
            if (valid)
            {
                return count;
            }
            else
            {
                return -1;
            }
        }

        public int resolve_operators()
        {
            return this.resolve_recursive(0, 0, Operation.Add, new List<Operation>());
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(this.test_value);
            sb.Append(":");
            foreach (var input in this.inputs)
            {
                sb.Append(" ");
                sb.Append(input);
            }

            return sb.ToString();
        }

        public void print_chains()
        {
            Console.WriteLine("\n\t{0}", this.ToString());
            
            if (!this.valid)
            {
                Console.WriteLine("\tNot Valid");
                return;
            }

            foreach (var chain in this.operator_paths)
            {
                var sb = new StringBuilder();
                sb.Append("\t");
                sb.Append(this.inputs[0]);

                for (int i = 0; i < chain.Count; i++)
                {
                    
                    switch (chain[i])
                    {
                        case Operation.Add:
                            sb.Append(" + ");
                            break;
                        case Operation.Multiply:
                            sb.Append(" * ");
                            break;
                        case Operation.Concatenate:
                            sb.Append(" || ");
                            break;
                        default:
                            sb.Append(" ???? "); 
                            break;
                    }

                    sb.Append(this.inputs[i + 1]);
                }
                
                Console.WriteLine(sb.ToString());
            }
        }
    }
    public class Day7(bool test) : Day(7, test)
    {
        public override void Part1()
        {
            Console.Write("\tPart 1: ");
            
            var lines = this.Load();

            List<Equation> equations = new List<Equation>();

            long calibration_result = 0;
            int valid_count = 0;
            int invalid_count = 0;
            if (this.test)
            {
                Console.WriteLine();
            }
            for (int i = 0; i < lines.Count; i++)
            {
                equations.Add(new Equation(lines[i]));
                equations[i].resolve_operators();

                if(equations[i].valid)
                {
                    calibration_result += equations[i].test_value;

                    valid_count++;
                }
                else
                {
                    invalid_count++;
                }
                if (this.test)
                {
                    equations[i].print_chains();
                }
            }

            Console.WriteLine(calibration_result);
            Console.WriteLine(String.Format("\tTotal: {0}, Valid: {1}, Invalid: {2}", lines.Count, valid_count, invalid_count));

        }
        public override void Part2()
        {
            Console.Write("\tPart 2: ");
            var lines = this.Load();
            List<Equation> equations = new List<Equation>();

            if (this.test)
            {
                Console.WriteLine();
            }
            long calibration_result = 0;
            int valid_count = 0;
            int invalid_count = 0;
            for (int i = 0; i < lines.Count; i++)
            {
                equations.Add(new Equation(lines[i]));
                equations[i].valid_operations.Add(Equation.Operation.Concatenate);
                equations[i].resolve_operators();

                if (equations[i].valid)
                {
                    calibration_result = checked(equations[i].test_value + calibration_result);
                    valid_count++;
                }
                else
                {
                    invalid_count++;
                }
                if (this.test)
                {
                    equations[i].print_chains();
                }
            }

            Console.WriteLine(calibration_result);
            Console.WriteLine(String.Format("\tTotal: {0}, Valid: {1}, Invalid: {2}", lines.Count, valid_count, invalid_count));

        }
    }
}
