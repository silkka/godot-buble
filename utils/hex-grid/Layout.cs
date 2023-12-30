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

        public static Vector2 HexCornerOffset(Layout layout, int corner)
        {
            Orientation M = layout.Orientation;
            float angle = 2.0f * (float)Math.PI * (M.StartAngle + corner) / 6.0f;
            return new Vector2(
                layout.Size.X * (float)Math.Cos(angle),
                layout.Size.Y * (float)Math.Sin(angle)
            );
        }

        public static Vector2[] PolygonCorners(Layout layout, Hex h)
        {
            Vector2[] corners = new Vector2[6];
            Vector2 center = HexToPixel(layout, h);
            for (int i = 0; i < 6; i++)
            {
                Vector2 offset = HexCornerOffset(layout, i);
                corners[i] = new Vector2(center.X + offset.X, center.Y + offset.Y);
            }
            return corners;
        }
    }
}
