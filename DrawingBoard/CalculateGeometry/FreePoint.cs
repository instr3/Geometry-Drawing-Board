using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingBoard.CalculateGeometry
{
    public class FreePoint : PointBase
    {
        public FreePoint(double x, double y)
        {
            P = new Vector2(x,y);
            Exist = true;
        }
        public override void TryDragTo(double x, double y)
        {
            P.X = x;
            P.Y = y;
            Exist = true;
            OnPositionUpdate();
        }
    }
}
