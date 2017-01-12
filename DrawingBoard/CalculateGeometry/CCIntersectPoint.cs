using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingBoard.CalculateGeometry
{
    public class CCIntersectPoint:IntersectPoint
    {
        public CircleBase C1 { get; private set; }
        public CircleBase C2 { get; private set; }
        public bool RootID { get; private set; }
        public CCIntersectPoint(CircleBase circle1, CircleBase circle2, double X, double Y)
        {
            C1 = circle1;
            C1.Children.Add(this); Parents.Add(C1);
            C2 = circle2;
            C2.Children.Add(this); Parents.Add(C2);
            Vector2 root1 = GetRoots(false);
            Vector2 root2 = GetRoots(true);
            Vector2 pos = new Vector2(X, Y);
            if (Vector2.Distance(root1, pos) <= Vector2.Distance(root2, pos))
                RootID = false;
            else
                RootID = true;
            RecalculateParameters();
        }
        Vector2 Rotate(Vector2 p,double cost,double sint)
        {
            return new Vector2(p.X * cost - p.Y * sint, p.X * sint + p.Y * cost);
        }
        private Vector2 GetRoots(bool rootID)
        {
            Vector2 ap = C1.C, bp = C2.C;
            double ar = C1.R, br = C2.R;
            double d = (ap - bp).Abs();
            double cost = (ar * ar + d * d - br * br) / (2 * ar * d);
            if(1 - cost * cost>=-Vector2.Eps)
            {
                double sint = 1 - cost * cost >= Vector2.Eps ? Math.Sqrt(1 - cost * cost) : 0;
                Vector2 v = (bp - ap).Norm() * ar;
                return rootID ?
                    ap + Rotate(v, cost, sint) :
                    ap + Rotate(v, cost, -sint);
            }
            return null;
        }
        public override void RecalculateParameters()
        {
            Vector2 result = GetRoots(RootID);
            if (result == null)
            {
                Exist = false;
            }
            else
            {
                Exist = true;
                P = result;
            }
        }
    }
}
