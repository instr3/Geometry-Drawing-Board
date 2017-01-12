using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingBoard.CalculateGeometry
{
    public abstract class IntersectPoint:PointBase
    {
        public override Color FillColor
        {
            get
            {
                return Color.LightGray;
            }
        }
        public static IntersectPoint CreateIntersection(Figure figure1,Figure figure2,double X,double Y)
        {
            if (figure1 is LineBase && figure2 is LineBase)
                return new LLIntersectPoint(figure1 as LineBase, figure2 as LineBase);
            else if (figure1 is LineBase && figure2 is CircleBase)
                return new LCIntersectPoint(figure1 as LineBase, figure2 as CircleBase, X, Y);
            else if (figure1 is CircleBase && figure2 is LineBase)
                return new LCIntersectPoint(figure2 as LineBase, figure1 as CircleBase, X, Y);
            else if (figure1 is CircleBase && figure2 is CircleBase)
                return new CCIntersectPoint(figure1 as CircleBase, figure2 as CircleBase, X, Y);
            else throw new NotImplementedException();
        }
        public sealed override void TryDragTo(double x, double y)
        {
            // Can't drag
        }
    }
}
