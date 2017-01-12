using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingBoard.CalculateGeometry
{
    class MidPoint:PointBase
    {
        public override Color FillColor
        {
            get
            {
                return Color.LightGray;
            }
        }
        public override Vector2 P
        {
            get
            {
                return (P1.P + P2.P) / 2;
            }
        }
        public PointBase P1, P2;
        public MidPoint(PointBase firstPoint,PointBase secondPoint)
        {
            P1 = firstPoint;
            P1.Children.Add(this);Parents.Add(P1);
            P2 = secondPoint;
            P2.Children.Add(this);Parents.Add(P2);
            Exist = true;
        }
        public override string ToString()
        {
            return base.ToString() + "(M of "+P1.P.ToString()+ "," +P2.P.ToString()+")";
        }
    }
}
