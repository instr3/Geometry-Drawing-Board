using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingBoard.CalculateGeometry
{
    class PointOnCircle:PointBase
    {
        public CircleBase Circle { get; private set; }
        public double Angle { get; private set; }
        public override Vector2 P
        {
            get
            {
                return Circle.C + new Vector2(Math.Cos(Angle), Math.Sin(Angle)) * Circle.R;
            }
        }
        public override Color FillColor
        {
            get
            {
                return Color.Orange;
            }
        }
        public PointOnCircle(CircleBase circle, double x, double y)
        {
            Circle = circle;
            Circle.Children.Add(this);
            Parents.Add(Circle);
            Angle = 0;
            TryDragTo(x, y);
            Exist = true;
        }
        public override void TryDragTo(double x, double y)
        {
            Vector2 p = new Vector2(x, y)-Circle.C;
            if (p.Abs() < Vector2.Eps) return;
            Angle = Math.Atan2(p.Y, p.X);
            OnPositionUpdate();
        }
        public override string ToString()
        {
            return base.ToString() + "(On " + Circle.GetType().Name + ")";
        }

    }
}
