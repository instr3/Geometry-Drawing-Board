using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingBoard.CalculateGeometry
{
    public class Vector2
    {
        public double X { get; set; }
        public double Y { get; set; }
        public const double Eps = 1e-10;

        public Vector2(double x,double y)
        {
            X = x;Y = y;
        }
        public static Vector2 operator + (Vector2 lhs,Vector2 rhs)
        {
            return new Vector2(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }
        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }
        public static Vector2 operator -(Vector2 op)
        {
            return new Vector2(-op.X, -op.Y);
        }
        public static Vector2 operator *(Vector2 lhs, double rhs)
        {
            return new Vector2(lhs.X * rhs, lhs.Y * rhs);
        }
        public static Vector2 operator *(double lhs, Vector2 rhs)
        {
            return new Vector2(lhs * rhs.X, lhs * rhs.Y);
        }

        public static Vector2 operator /(Vector2 lhs, double rhs)
        {
            return new Vector2(lhs.X / rhs, lhs.Y / rhs);
        }

        internal double Abs2()
        {
            return X * X + Y * Y;
        }

        public static double Distance(Vector2 a,Vector2 b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }
        public double Abs()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        public double Angle()
        {
            return Math.Atan2(Y, X);
        }
        public Vector2 Norm()
        {
            return this / Abs();
        }
        public static double operator *(Vector2 lhs, Vector2 rhs)// Det
        {
            return lhs.X * rhs.Y - lhs.Y * rhs.X;
        }
        public static double operator ^(Vector2 lhs, Vector2 rhs)// Dot
        {
            return lhs.X * rhs.X + lhs.Y * rhs.Y;
        }

        public override string ToString()
        {
            return string.Format("({0:0.00},{1:0.00})", X, Y);
        }
    }
}
