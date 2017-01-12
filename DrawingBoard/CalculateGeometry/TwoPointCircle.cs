using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingBoard.CalculateGeometry
{
    public class TwoPointCircle:CircleBase
    {
        public PointBase PC { get; set; }
        public PointBase PR { get; set; }
        public override Vector2 C
        {
            get
            {
                return PC.P;
            }
        }
        public override double R
        {
            get
            {
                return Vector2.Distance(PR.P, PC.P);
            }
        }
        public TwoPointCircle(PointBase CenterPoint,PointBase RadiusPoint)
        {
            PC = CenterPoint;
            PC.Children.Add(this);Parents.Add(PC);
            PR = RadiusPoint;
            PR.Children.Add(this); Parents.Add(PR);
            Exist = R > Vector2.Eps;
        }
        public override void RecalculateParameters()
        {
            Exist = R > Vector2.Eps;
        }
    }
}
