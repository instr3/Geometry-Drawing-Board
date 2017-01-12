using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingBoard.CalculateGeometry
{
    public abstract class LineSegmentBase : LineBase
    {
        double leftMostClamp = 0, rightMostClamp = 1;
        float[] dashPattern = new float[] { 3, 2 };
        Pen[] pens;
        public sealed override void Draw(Graphics g)
        {
            if(pens==null)
            {
                pens = new Pen[4];
                pens[0] = new Pen(drawColor, drawWidth);
                pens[1] = new Pen(Color.Black, drawWidth + 2);
                pens[2] = new Pen(drawColor, drawWidth);
                pens[2].DashPattern = dashPattern;
                pens[3] = new Pen(Color.Black, drawWidth + 2);
                // Adjust the dash pattern for width is changed
                pens[3].DashPattern = 
                    dashPattern.Select(x => x / (drawWidth + 2) * drawWidth).ToArray();
            }
            Vector2 L = leftMostClamp * (T - S) + S;
            Vector2 R = rightMostClamp * (T - S) + S;
            if (Selected)
            {
                g.DrawLine(pens[1], new Point((int)S.X, (int)S.Y), new Point((int)T.X, (int)T.Y));
                g.DrawLine(pens[3], new Point((int)S.X, (int)S.Y), new Point((int)L.X, (int)L.Y));
                g.DrawLine(pens[3], new Point((int)T.X, (int)T.Y), new Point((int)R.X, (int)R.Y));
            }
                
            g.DrawLine(pens[0], new Point((int)S.X, (int)S.Y), new Point((int)T.X, (int)T.Y));
            g.DrawLine(pens[2], new Point((int)S.X, (int)S.Y), new Point((int)L.X, (int)L.Y));
            g.DrawLine(pens[2], new Point((int)T.X, (int)T.Y), new Point((int)R.X, (int)R.Y));

        }
        public sealed override void AfterChildrenUpdated()
        {
            leftMostClamp = 0;
            rightMostClamp = (T - S) ^ (T - S);
            foreach (Figure child in Children)
            {
                if(child is PointBase)
                {
                    PointBase p = child as PointBase;
                    leftMostClamp = Math.Min(leftMostClamp, (p.P - S) ^ (T - S));
                    rightMostClamp = Math.Max(rightMostClamp, (p.P - S) ^ (T - S));
                }
            }
            leftMostClamp /= (T - S) ^ (T - S);
            rightMostClamp /= (T - S) ^ (T - S);
        }
        public sealed override double GetDistance(double x, double y)
        {
            Vector2 p = new Vector2(x, y);
            Vector2 L = leftMostClamp * (T - S) + S;
            Vector2 R = rightMostClamp * (T - S) + S;
            if (((p - L) ^ (R - L)) < 0) return (p - L).Abs();
            if (((p - R) ^ (L - R)) < 0) return (p - R).Abs();
            return LineDistanceFromPoint(p);
        }
        /*public sealed override double ClampPosition(double pos)
        {
            return pos < leftMostClamp ? leftMostClamp : pos > rightMostClamp ? rightMostClamp : pos;
        }*/
    }
}
