using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingBoard.CalculateGeometry
{
    public class TwoPointSegment:LineSegmentBase
    {
        public PointBase PS { get; protected set; }
        public PointBase PT { get; protected set; }
        public override Vector2 S
        {
            get
            {
                return PS.P;
            }
        }
        public override Vector2 T
        {
            get
            {
                return PT.P;
            }
        }
        public TwoPointSegment(PointBase StartPoint,PointBase EndPoint)
        {
            PS = StartPoint;
            PT = EndPoint;
            PS.Children.Add(this); Parents.Add(PS);
            PT.Children.Add(this); Parents.Add(PT);

            Exist = Vector2.Distance(PS.P, PT.P) > Vector2.Eps;
        }
        public override void RecalculateParameters()
        {
            Exist = Vector2.Distance(PS.P, PT.P) > Vector2.Eps;
        }
    }
}
