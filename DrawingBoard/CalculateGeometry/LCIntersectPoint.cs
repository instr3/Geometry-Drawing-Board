using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingBoard.CalculateGeometry
{
    public class LCIntersectPoint:IntersectPoint
    {
        public LineBase L { get; private set; }
        public CircleBase C { get; private set; }
        public bool RootID { get; private set; }
        public LCIntersectPoint(LineBase line,CircleBase circle,double X,double Y)
        {
            L = line;
            L.Children.Add(this); Parents.Add(L);
            C = circle;
            C.Children.Add(this); Parents.Add(C);
            Vector2 root1 = GetRoots(false);
            Vector2 root2 = GetRoots(true);
            Vector2 pos = new Vector2(X, Y);
            if (Vector2.Distance(root1, pos) <= Vector2.Distance(root2, pos))
                RootID = false;
            else
                RootID = true;
            RecalculateParameters();
        }
        private Vector2 GetRoots(bool rootID)
        {
            Vector2 a = L.S;
            Vector2 b = L.T;
            Vector2 o = C.C;
            double r = C.R;
            double dx = b.X - a.X, dy = b.Y - a.Y;
            double ta = dx * dx + dy * dy;
            double tb = 2 * dx * (a.X - o.X) + 2 * dy * (a.Y - o.Y);
            double tc = (a.X - o.X) * (a.X - o.X) + (a.Y - o.Y) * (a.Y - o.Y) - r * r;
            double delta = tb * tb - 4 * ta * tc;
            if(delta>=-Vector2.Eps)
            {
                double sqrt = delta <= Vector2.Eps ? 0 : Math.Sqrt(delta);
                double t1 = (-tb - sqrt) / (2 * ta);
                double t2 = (-tb + sqrt) / (2 * ta);
                return rootID ?
                    new Vector2(a.X + t2 * dx, a.Y + t2 * dy) :
                    new Vector2(a.X + t1 * dx, a.Y + t1 * dy);
            }
            return null;
        }
        public override void RecalculateParameters()
        {
            Vector2 result = GetRoots(RootID);
            if(result==null)
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
