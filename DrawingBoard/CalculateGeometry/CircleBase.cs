using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingBoard.CalculateGeometry
{
    public abstract class CircleBase:Figure
    {
        public virtual Vector2 C { get; set; }
        public virtual double R { get; set; }
        public Color drawColor = Color.DarkCyan;
        public float drawWidth = 3f;
        public sealed override void Draw(Graphics g)
        {
            RectangleF rect = new RectangleF((float)(C.X - R), (float)(C.Y - R), (float)R * 2, (float)R * 2);
            if (Selected)
                g.DrawEllipse(new Pen(Color.Black, drawWidth+2), rect);
            g.DrawEllipse(new Pen(drawColor, drawWidth), rect);
        }
        public sealed override double GetDistance(double x, double y)
        {
            Vector2 p = new Vector2(x, y);
            return Math.Abs((Vector2.Distance(C, p) - R));
        }
        public override string ToString()
        {
            return base.ToString() + string.Format("{0},R={1:0.00}", C.ToString(), R);
        }
    }
}
