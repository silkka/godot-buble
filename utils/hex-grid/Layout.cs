using System;
using Godot;

namespace HexGrid
{
    public class Layout
    {
        public readonly Orientation Orientation;
        public readonly Vector2 Size;
        public readonly Vector2 Origin;

        public Layout(Orientation orientation, Vector2 size, Vector2 origin)
        {
            Orientation = orientation;
            Size = size;
            Origin = origin;
        }

        public static Vector2 HexToPixel(Layout layout, Hex h)
        {
            Orientation M = layout.Orientation;
            float x = (M.F0 * h.Q + M.F1 * h.R) * layout.Size.X;
            float y = (M.F2 * h.Q + M.F3 * h.R) * layout.Size.Y;
            return new Vector2(x + layout.Origin.X, y + layout.Origin.Y);
        }

        public static FractionalHex PixelToHex(Layout layout, Vector2 p)
        {
            Orientation M = layout.Orientation;
            Vector2 pt = new Vector2(
                (p.X - layout.Origin.X) / layout.Size.X,
                (p.Y - layout.Origin.Y) / layout.Size.Y
            );
            float q = M.B0 * pt.X + M.B1 * pt.Y;
            float r = M.B2 * pt.X + M.B3 * pt.Y;
            return new FractionalHex(q, r);
        }
    }
}
