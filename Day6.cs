using System;
using System.Collections.Generic;
using System.Net;
using System.Text;


namespace AOC24
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    public class Map
    {
        public int[,] MapState { 
            get {
                return this._map.Clone() as int[,];
            } 
        }
        
        public int[] Guard_pos
        {
            get
            {
                int[] guard_pos_copy = [0, 0];
                this._guard_pos.CopyTo(guard_pos_copy, 0);
                return guard_pos_copy;
            }
            set
            {
                if (value[0] < 0 || value[1] < 0 || value[1] >= this._y_size || value[0] >= this._x_size)
                {
                    throw new Exception("Guard position out of map bounds");
                }
                value.CopyTo(this._guard_pos, 0);
            }
        }
        public int[] Guard_start { get => _guard_start; }
        public Direction Guard_facing { get; set; }
        public List<Tuple<Direction, int[]>> Turns { get { return new List<Tuple<Direction, int[]>>(_turns); } }

        private int _x_size;
        private int _y_size;
        private int[,] _map;

        private int[] _guard_pos = [0, 0];
        private int[] _guard_start = [0, 0];

        private List<Tuple<Direction, int[]>> _turns = new List<Tuple<Direction, int[]>>();

        public Map(Map m)
        {
            this._x_size = m.MapState.GetLength(0);
            this._y_size = m.MapState.GetLength(1);
            this._map = m.MapState;
            this._guard_pos = m.Guard_pos;
            this._guard_start = m.Guard_start;
            this.Guard_facing = m.Guard_facing;
            this._turns = m.Turns;
        }

        public Map(List<string> lines)
        {
            this._map = new int[lines[0].Length, lines.Count];

            for (int j = 0; j < lines.Count; j++)
            {
                for (int i = 0; i < lines[j].Length; i++)
                {
                    if (lines[j][i] == '^')
                    {
                        this._guard_pos = [i, j];
                        this._guard_start = [i, j];
                        this._map[i, j] = 1;
                    }
                    else if (lines[j][i] == '#')
                    {
                        this._map[i, j] = -1;
                    }
                }
            }
            
            this._x_size = this._map.GetLength(0);
            this._y_size = this._map.GetLength(1);


        }
        public void clear_map()
        {
            for (int j = 0; j < this._y_size; j++)
            {
                for (int i = 0; i < this._x_size; i++)
                {
                    if (this._map[i, j] >= 1)
                    {
                        this._map[i, j] = 0;
                    }
                }
            }
        }
        public void reset()
        {
            this._guard_pos = this._guard_start;
            this.Guard_facing = Direction.Up;
            this.clear_map();
            this._turns.Clear();
        }
        public bool in_bounds(int x, int y)
        {
            if (x < 0 || y < 0 || y >= this._y_size || x >= this._x_size)
            {
                return false;
            }
            return true;
        }
        private bool open_space(int x, int y)
        {
            if (!in_bounds(x, y))
            {
                return true;
            }
            if (this._map[x, y] < 0)
            {
                return false;
            }
            return true;
        }

        private int[] direction_to_coords(Direction d)
        {
            switch (d)
            {
                case Direction.Up:
                    return [0, -1];
                case Direction.Down:
                    return [0, 1];
                case Direction.Left:
                    return [-1, 0];
                case Direction.Right:
                    return [1, 0];
                default:
                    throw new Exception("Invalid direction. How did you even get here?!");
            }
        }
        public int move_guard()
        {
            var move_dir = this.direction_to_coords(this.Guard_facing);
            var target_x = this._guard_pos[0] + move_dir[0];
            var target_y = this._guard_pos[1] + move_dir[1];


            if (target_x < 0 || target_y < 0 || target_y >= this._y_size || target_x >= this._x_size)
            {
                this._turns.Add(new Tuple<Direction, int[]>(this.Guard_facing, this.Guard_pos));
                return -1;
            }

            var last_map_value = this._map[target_x, target_y];

            if (!this.open_space(target_x, target_y))
            {
                switch (this.Guard_facing)
                {
                    case Direction.Up:
                        this.Guard_facing = Direction.Right;
                        break;
                    case Direction.Right:
                        this.Guard_facing = Direction.Down;
                        break;
                    case Direction.Down:
                        this.Guard_facing = Direction.Left;
                        break;
                    case Direction.Left:
                        this.Guard_facing = Direction.Up;
                        break;
                    default:
                        throw new Exception("Invalid guard facing direction. How did you even get here?!");
                }
                var turn_loc = new int[2];
                this._guard_pos.CopyTo(turn_loc, 0);
                this._turns.Add(new Tuple<Direction, int[]>(this.Guard_facing, turn_loc));
                return 1;
            }
            else
            {
                this._guard_pos = [target_x, target_y];
                this._map[target_x, target_y] = (int)this.Guard_facing + 1;

                if (last_map_value == (int)this.Guard_facing + 1) //loop detected
                {
                    return -2;
                }
                else
                {
                    return 0;
                }


            }

        }
        public void add_obstacle(int x, int y)
        {
            if (!in_bounds(x, y))
            {
                throw new Exception("Obstacle out of map bounds");
            }
            this._map[x, y] = -2;
        }
        public int count_visited()
        {
            int count = 0;
            foreach (var c in this._map)
            {
                if (c >= 1)
                {
                    count++;
                }
            }
            return count;
        }

        public int loop_count_old()
        {
            int count = 0;
            
            int[] delta = [0, 0];
            int[] endpoint = [0, 0];
            int[] obstacle = [0, 0];

            var loop_locs = new Dictionary<int, List<int>>();

            for (int i = 0; i < this._turns.Count - 1; i++)
            {
                endpoint = this._turns[i + 1].Item2;

                switch (this._turns[i].Item1)
                {
                    case Direction.Up:
                        delta = [0, -1];
                        break;
                    case Direction.Right:
                        delta = [1, 0];
                        break;
                    case Direction.Down:
                        delta = [0, 1];
                        break;
                    case Direction.Left:
                        delta = [-1, 0];
                        break;
                    default:
                        throw new Exception("Invalid direction. How did you even get here?!");
                }

                this._turns[i].Item2.CopyTo(obstacle, 0);

                Map working_map;
                while (true) //step obstacle location along line between adjacent turns
                {
                    obstacle[0] += delta[0];
                    obstacle[1] += delta[1];
                    if (!in_bounds(obstacle[0], obstacle[1]))
                    {
                        break;
                    }   
                    if (this._map[obstacle[0], obstacle[1]] == -1)
                    {
                        break;
                    }

                    working_map = new Map(this);
                    working_map.clear_map();
                    working_map.add_obstacle(obstacle[0], obstacle[1]);
                    working_map.Guard_facing = this._turns[i].Item1;
                    working_map.Guard_pos = this._turns[i].Item2;

                    var move_result = 0;
                    while (move_result >= 0)
                    {
                        move_result = working_map.move_guard();
                    }
                    if (move_result == -2)
                    {
                        if (!loop_locs.ContainsKey(obstacle[0]))
                        {
                            loop_locs[obstacle[0]] = new List<int>();
                        }
                        if (!loop_locs[obstacle[0]].Contains(obstacle[1]))
                        {
                            loop_locs[obstacle[0]].Add(obstacle[1]);
                            count++;
                        }
                    }

                }


            }

            return count;

        }
        public int loop_count()
        {
            Map working_map;
            int count = 0;
            for (int i = 0; i < this._x_size; i++)
            {
                for (int j = 0; j < this._y_size; j++)
                {

                    working_map = new Map(this);
                    working_map.reset();
                    working_map.add_obstacle(i, j);

                    var move_result = 0;
                    while (move_result >= 0)
                    {
                        move_result = working_map.move_guard();
                    }
                    if (move_result == -2)
                    {
                        count += 1;
                    }
                }
            }
                return count;
        }


        public void draw()
        {
            for (int j = 0; j < this._y_size; j++)
            {
                for (int i = 0; i < this._x_size; i++)
                {
                    if (this._guard_pos[0] == i && this._guard_pos[1] == j)
                    {
                        switch(Guard_facing)
                        {
                            case Direction.Up:
                                Console.Write("^");
                                break;
                            case Direction.Right:
                                Console.Write(">");
                                break;
                            case Direction.Down:
                                Console.Write("v");
                                break;
                            case Direction.Left:
                                Console.Write("<");
                                break;
                            default:
                                Console.Write("*");
                                break;
                        }
                    }
                    else
                    {
                        
                        switch (this._map[i, j])
                        {
                            case 0:
                                Console.Write(".");
                                break;
                            case 1:
                                Console.Write("|");
                                break;
                            case 2:
                                Console.Write("-");
                                break;
                            case 3:
                                Console.Write("!");
                                break;
                            case 4:
                                Console.Write("~");
                                break;
                            case -1:
                                Console.Write("#");
                                break;
                            case -2:
                                Console.Write("O");
                                break;
                            default:
                                Console.Write("?");
                                break;
                        }
                    }
                }
                Console.WriteLine();
            }
        }
    }
    public class Day6(bool test) : Day(6, test)
    {
        public override void Part1()
        {
            Console.Write("\tPart 1: ");
            var lines = this.Load();
            var map = new Map(lines);

            while (true)
            {
                if (map.move_guard() < 0)
                {
                    break;
                }
            }
            Console.WriteLine(map.count_visited());
            }
        public override void Part2()
        {
            Console.Write("\tPart 2: ");
            var lines = this.Load();
            var map = new Map(lines);

            while (true)
            {
                var result = map.move_guard();
                if (map.move_guard() < 0)
                {
                    break;
                }

            }
            Console.WriteLine(map.loop_count());
            
            
        }
    }
}
