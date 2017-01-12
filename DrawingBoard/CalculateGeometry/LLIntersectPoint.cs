using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingBoard.CalculateGeometry
{
    public class LLIntersectPoint:IntersectPoint
    {
        public LineBase L1 { get; set; }
        public LineBase L2 { get; set; }
        public LLIntersectPoint(LineBase firstLine,LineBase secondLine)
        {
            L1 = firstLine;
            L1.Children.Add(this);Parents.Add(L1);
            L2 = secondLine;
            L2.Children.Add(this);Parents.Add(L2);
            RecalculateParameters();
        }
        public override void RecalculateParameters()
        {
            double det = (L1.T - L1.S) * (L2.T - L2.S);
            if(Math.Abs(det)<Vector2.Eps)
            {
                Exist = false;
                P = null;
            }
            else
            {
                double s1 = (L1.S - L2.S) * (L2.T - L2.S);
                double s2 = (L1.T - L2.S) * (L2.T - L2.S);
                Exist = true;
                P = (s1 * L1.T - s2 * L1.S) / (s1 - s2);
            }
        }
        public override string ToString()
        {
            return base.ToString() + "(I of " + L1.GetType().Name + " & " + L2.GetType().Name + ")";
        }
    }
}
