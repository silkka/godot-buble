using System;
using Godot;

namespace HexGrid
{
    public class Hex
    {
        private Vector3I vector3;
        public int Q => vector3.X;
        public int R => vector3.Y;
        public int S => vector3.Z;

        public static readonly Hex[] Directions = new Hex[]
        {
            new(+1, 0),
            new(+1, -1),
            new(0, -1),
            new(-1, 0),
            new(-1, +1),
            new(0, +1)
        };

        public Hex(int q, int r)
        {
            vector3 = new Vector3I(q, r, -q - r);

            if (vector3.X + vector3.Y + vector3.Z != 0)
                throw new ArgumentException("q + r + s must be 0");
        }

        public Hex(Vector3I vector3)
        {
            this.vector3 = vector3;

            if (vector3.X + vector3.Y + vector3.Z != 0)
                throw new ArgumentException("q + r + s must be 0");
        }

        /************************ OPERATORS *********************************/

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Hex other = (Hex)obj;
            return this == other;
        }

        public static bool operator ==(Hex a, Hex b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }
            if (a is null || b is null)
            {
                return false;
            }

            return a.vector3 == b.vector3;
        }

        public static bool operator !=(Hex a, Hex b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Q, R, S);
        }

        public static Hex operator +(Hex a, Hex b)
        {
            return new Hex(a.vector3 + b.vector3);
        }

        public static Hex operator -(Hex a, Hex b)
        {
            return new Hex(a.vector3 - b.vector3);
        }

        public static Hex operator *(Hex a, int k)
        {
            return new Hex(k * a.vector3);
        }

        public int Length()
        {
            return (Math.Abs(Q) + Math.Abs(R) + Math.Abs(S)) / 2;
        }

        public int Distance(Hex other)
        {
            return (this - other).Length();
        }

        public static Hex Direction(int direction)
        {
            if (direction < 0 || direction > 5)
            {
                throw new ArgumentException("direction must be between 0 and 5");
            }

            return Directions[direction];
        }

        public Hex Neighbor(int direction)
        {
            return this + Direction(direction);
        }
    }

    public class FractionalHex
    {
        public readonly float Q;
        public readonly float R;
        public readonly float S;

        public FractionalHex(float q, float r)
        {
            Q = q;
            R = r;
            S = -q - r;
        }

        public static Hex HexRound(FractionalHex h)
        {
            int qi = (int)Math.Round(h.Q);
            int ri = (int)Math.Round(h.R);
            int si = (int)Math.Round(h.S);
            float q_diff = Math.Abs(qi - h.Q);
            float r_diff = Math.Abs(ri - h.R);
            float s_diff = Math.Abs(si - h.S);
            if (q_diff > r_diff && q_diff > s_diff)
            {
                qi = -ri - si;
            }
            else if (r_diff > s_diff)
            {
                ri = -qi - si;
            }

            return new Hex(qi, ri);
        }

        public static FractionalHex HexLerp(Hex a, Hex b, float t)
        {
            return new FractionalHex(a.Q * (1.0f - t) + b.Q * t, a.R * (1.0f - t) + b.R * t);
        }

        public static Hex[] HexLinedraw(Hex a, Hex b)
        {
            int N = a.Distance(b);
            Hex[] results = new Hex[N + 1];
            float step = 1.0f / Math.Max(N, 1);
            for (int i = 0; i <= N; i++)
            {
                results[i] = HexRound(HexLerp(a, b, step * i));
            }
            return results;
        }
    }
}
