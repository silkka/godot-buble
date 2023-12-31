using System.Collections.Generic;
using Godot;

namespace HexGrid
{
    class Grid
    {
        private readonly Dictionary<Hex, Ball> grid;
        public readonly Layout layout;

        public Grid(Vector2 size, Vector2 origin)
        {
            grid = new Dictionary<Hex, Ball>();
            layout = new Layout(orientation: Orientation.POINTY, size, origin);
        }

        public void Add(Hex hex, Ball ball)
        {
            grid.Add(hex, ball);
        }

        public void Remove(Hex hex)
        {
            grid.Remove(hex);
        }

        public bool Contains(Hex hex)
        {
            return grid.ContainsKey(hex);
        }

        public Ball Get(Hex hex)
        {
            return grid[hex];
        }

        public void Set(Hex hex, Ball ball)
        {
            grid[hex] = ball;
        }

        public void CreateRectangular(int width, int height)
        {
            for (int r = 0; r < height; r++)
            {
                int r_offset = r >> 1;
                for (int q = -r_offset; q < width - r_offset - (r % 2); q++)
                {
                    GD.Print(q, r);
                    Hex hex = new(q, r);
                    grid.Add(hex, null);
                }
            }
        }

        public List<Hex> GetHexes()
        {
            return new List<Hex>(grid.Keys);
        }
    }
}
