using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingBoard.CalculateGeometry
{
    public abstract class PointBase : Figure
    {
        public string Letter { get; private set; }
        public virtual Vector2 P { get; set; }
        public float BorderWidth = 1, DrawRadius = 3.5f;
        private Font font = new Font("宋体", 12, FontStyle.Bold);

        public virtual Color FillColor
        {
            get { return Color.YellowGreen; }
        }
        public sealed override double GetDistance(double x, double y)
        {
            return Vector2.Distance(P, new Vector2(x, y));
        }
        public sealed override void Draw(Graphics g)
        {
            if(Letter==null)
            {
                Letter = LetterPool.Instance.Request();
            }
            RectangleF rect = new RectangleF((float)P.X - DrawRadius, (float)P.Y - DrawRadius, DrawRadius * 2, DrawRadius * 2);
            g.DrawString(Letter, font, Brushes.Black, new PointF((float)P.X, (float)P.Y));
            g.FillEllipse(new SolidBrush(FillColor), rect);
            g.DrawEllipse(new Pen(Color.Black, BorderWidth), rect);
            if (Selected)
                g.DrawEllipse(new Pen(Color.Black, BorderWidth + 1), rect);
        }
        public override string ToString()
        {
            return base.ToString() + P.ToString();
        }
        public override void OnDestroy()
        {
            if(Letter!=null)
            {
                LetterPool.Instance.Recycle(Letter);
                Letter = null;
            }
        }
    }
}
