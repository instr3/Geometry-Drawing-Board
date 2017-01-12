using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingBoard.CalculateGeometry
{
    public class PointOnLine:PointBase
    {
        public LineBase Line { get; private set; }
        public double Position { get; private set; }
        public override Vector2 P
        {
            get
            {
                return Line.S + (Line.T - Line.S) * Position;
            }
        }
        public override Color FillColor
        {
            get
            {
                return Color.Orange;
            }
        }
        public PointOnLine(LineBase line,double x,double y)
        {
            Line = line;
            Line.Children.Add(this);
            Parents.Add(Line);
            Position = 0;
            TryDragTo(x, y);
            Exist = true;
        }
        public override void TryDragTo(double x, double y)
        {
            Vector2 p = new Vector2(x, y);
            Position = Line.ClampPosition(((p - Line.S) ^ (Line.T-Line.S))/(Line.T-Line.S).Abs2());
            OnPositionUpdate();
        }
        public override string ToString()
        {
            return base.ToString() + "(On " + Line.GetType().Name + ")";
        }
    }
}
