using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingBoard.CalculateGeometry
{
    public abstract class LineBase : Figure
    {
        public float drawWidth = 3f;
        public Color drawColor = Color.DarkBlue;
        public virtual Vector2 S { get; set; }
        public virtual Vector2 T { get; set; }
        public Vector2 GetDir()
        {
            return (T - S).Norm();
        }
        public virtual double ClampPosition(double pos)
        {
            return pos;
        }
        protected double LineDistanceFromPoint(Vector2 p)
        {
            return Math.Abs((S - p) * (T - p) / Vector2.Distance(S, T));
        }
        public override string ToString()
        {
            return base.ToString() + string.Format("{0}-{1}", S.ToString(), T.ToString());
        }
    }
}
