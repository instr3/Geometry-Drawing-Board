using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingBoard.CalculateGeometry
{
    public abstract class Figure
    {

        public abstract void Draw(Graphics g);
        public abstract double GetDistance(double x, double y);
        public bool Exist { get; protected set; }
        public bool Selected { get; set; }
        public bool Destroyed { get; private set; }
        private int updateTimeStamp;
        private static Random random = new Random();

        public virtual void TryDragTo(double x, double y)
        {
            // Default: Can't drag
        }
        public virtual void RecalculateParameters()
        {
            // Default: Set myself to exist
            Exist = true;
        }
        public virtual void AfterChildrenUpdated()
        {
            // Default: No need to collect information
        }
        private void Hidehierarchy()
        {
            Exist = false;
            foreach (Figure figure in Children)
            {
                if(figure.Exist)
                {
                    figure.Hidehierarchy();
                }
            }
        }
        private void OnPositionUpdate_Inner(int timeStamp)
        {
            if (timeStamp == updateTimeStamp)
                return;
            updateTimeStamp = timeStamp;
            RecalculateParameters();
            if (!Exist)
                Hidehierarchy();
            else
            {
                foreach (Figure figure in Children)
                    figure.OnPositionUpdate_Inner(timeStamp);
                AfterChildrenUpdated();
                foreach (Figure figure in Parents)
                    if(figure.updateTimeStamp!=timeStamp)
                        figure.AfterChildrenUpdated();
            }
        }
        /// <summary>
        /// Call when the position of an originator is changed.
        /// </summary>
        public void OnPositionUpdate()
        {
            if (!Exist) return;
            int timeStamp;
            lock (random)
                timeStamp = random.Next();
            OnPositionUpdate_Inner(timeStamp);
        }

        public List<Figure> Children = new List<Figure>();
        public List<Figure> Parents = new List<Figure>();

        public virtual void OnDestroy() { }
        public void Destroy()
        {
            if (Destroyed) return;
            OnDestroy();
            while (Children.Count>0)
            {
                Children[0].Destroy();
            }
            foreach(Figure figure in Parents)
            {
                figure.Children.Remove(this);
            }
            Destroyed = true;
        }

        public override string ToString()
        {
            string res = "<" +
                    GetType().Name + ">[";
            if (Exist) res += "E";
            if (Selected) res += "S";
            if (Destroyed) res += "D";
            return res + "]";
        }
    }
}
