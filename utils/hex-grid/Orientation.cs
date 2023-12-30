using System;

namespace HexGrid
{
    public readonly struct Orientation
    {
        public readonly float F0;
        public readonly float F1;
        public readonly float F2;
        public readonly float F3;
        public readonly float B0;
        public readonly float B1;
        public readonly float B2;
        public readonly float B3;

        public readonly float StartAngle;

        public Orientation(
            float F0,
            float F1,
            float F2,
            float F3,
            float B0,
            float B1,
            float B2,
            float B3,
            float StartAngle
        )
        {
            this.F0 = F0;
            this.F1 = F1;
            this.F2 = F2;
            this.F3 = F3;
            this.B0 = B0;
            this.B1 = B1;
            this.B2 = B2;
            this.B3 = B3;
            this.StartAngle = StartAngle;
        }

        public static readonly Orientation FLAT =
            new(
                F0: 3.0f / 2.0f,
                F1: 0.0f,
                F2: (float)Math.Sqrt(3.0) / 2.0f,
                F3: (float)Math.Sqrt(3.0),
                B0: 2.0f / 3.0f,
                B1: 0.0f,
                B2: -1.0f / 3.0f,
                B3: (float)Math.Sqrt(3.0) / 3.0f,
                StartAngle: 0.0f
            );

        public static readonly Orientation POINTY =
            new(
                F0: (float)Math.Sqrt(3.0),
                F1: (float)Math.Sqrt(3.0) / 2.0f,
                F2: 0.0f,
                F3: 3.0f / 2.0f,
                B0: (float)Math.Sqrt(3.0) / 3.0f,
                B1: -1.0f / 3.0f,
                B2: 0.0f,
                B3: 2.0f / 3.0f,
                StartAngle: 0.5f
            );
    }
}
