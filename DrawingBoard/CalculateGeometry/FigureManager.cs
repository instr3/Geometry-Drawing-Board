using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawingBoard.CalculateGeometry
{
    public class FigureManager
    {
        public CreateMode CurrentCreateMode { get; set; } = CreateMode.Point;

        public enum CreateMode
        {
            Nothing = 0,
            Point = 1,
            Line = 2,
            Circle = 3,
        }
        public enum HoverStatus
        {
            NothingAround = 0,
            NearExistPoint = 1,
            NearExistLine = 2,
            NearExistCircle = 3,
            NearExistIntersection = 4,

        }
        public List<Figure> Figures { get; private set; }
        Figure dragingFigure;
        Figure[] drawingFigures;
        BufferedGraphics bg;
        Graphics target;
        public FigureManager(PictureBox pictureBox)
        {
            Figures = new List<Figure>();
            target = pictureBox.CreateGraphics();
            bg = BufferedGraphicsManager.Current.Allocate(target, new Rectangle(0, 0, pictureBox.Width, pictureBox.Height));
            bg.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            bg.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        }
        public void Redraw()
        {
            bg.Graphics.Clear(Color.White);
            // First draw segment,circle etc
            foreach (Figure figure in Figures)
                if (!(figure is PointBase))
                    if (figure.Exist)
                        figure.Draw(bg.Graphics);
            // Then draw drawing figures
            // Objects in drawing figures should be first segments, then points
            if (drawingFigures != null)
                foreach (Figure figure in drawingFigures)
                    if (figure.Exist)
                        figure.Draw(bg.Graphics);
            // Then draw points
            foreach (Figure figure in Figures)
                if(figure is PointBase)
                    if(figure.Exist)
                        figure.Draw(bg.Graphics);
            bg.Render(target);
        }
        public void PushFigure(Figure figure)
        {
            Figures.Add(figure);
        }
        public void DestroyFigure(Figure figure)
        {
            figure.Destroy();
            Figures.RemoveAll(i => i.Destroyed);
        }
        bool isMouseDown;
        const double POINT_ATTRACT_DISTANCE = 7;
        const double LINE_ATTRACT_DISTANCE = 4;
        Figure HoverFigure;
        Figure HoverIntersectionA, HoverIntersectionB;
        HoverStatus GetHoverStatus(int X, int Y)
        {
            double pointDistance = POINT_ATTRACT_DISTANCE;
            Figure nearestFigure = null;
            foreach (Figure figure in Figures)
                figure.Selected = false;
            foreach (Figure figure in Figures)
            {
                if (!figure.Exist) continue;
                if(figure is PointBase)
                {
                    if(figure.GetDistance(X,Y)< pointDistance)
                    {
                        pointDistance = figure.GetDistance(X, Y);
                        nearestFigure = figure;
                    }
                }
            }
            if(nearestFigure!=null)
            {
                HoverFigure = nearestFigure;
                nearestFigure.Selected = true;
                return HoverStatus.NearExistPoint;
            }
            // Get two most nearing line
            // Test if they are likely to be intersect
            double lineDistance = LINE_ATTRACT_DISTANCE;
            double lineDistance2 = LINE_ATTRACT_DISTANCE;
            Figure nearestFigure2 = null;

            foreach (Figure figure in Figures)
            {
                if (!figure.Exist) continue;
                if (!(figure is PointBase))
                {
                    double distance = figure.GetDistance(X, Y);
                    if (distance < lineDistance)
                    {
                        lineDistance2 = lineDistance;
                        nearestFigure2 = nearestFigure;
                        lineDistance = distance;
                        nearestFigure = figure;
                    }
                    else if(distance < lineDistance2)
                    {
                        lineDistance2 = distance;
                        nearestFigure2 = figure;
                    }
                }
            }
            if (nearestFigure != null)
            {
                HoverFigure = nearestFigure;
                if (nearestFigure2!=null)
                {
                    HoverIntersectionA = nearestFigure;
                    HoverIntersectionB = nearestFigure2;
                    HoverIntersectionA.Selected = true;
                    HoverIntersectionB.Selected = true;
                    return HoverStatus.NearExistIntersection;

                }
                else
                {
                    nearestFigure.Selected = true;
                    if (HoverFigure is LineBase)
                        return HoverStatus.NearExistLine;
                    else if (HoverFigure is CircleBase)
                        return HoverStatus.NearExistCircle;
                    else throw new NotImplementedException();
                }
            }
            return HoverStatus.NothingAround;
        }
        Figure CreateTwoPointFigure(CreateMode createMode,PointBase start,PointBase end)
        {
            if (createMode == CreateMode.Line)return new TwoPointSegment(start, end);
            if (createMode == CreateMode.Circle) return new TwoPointCircle(start, end);
            throw new NotImplementedException();
        }
        public void MouseDown(int X,int Y)
        {
            isMouseDown = true;
            drawingFigures = null;
            HoverStatus hoverStatus = GetHoverStatus(X, Y);
            if (CurrentCreateMode == CreateMode.Point)
            {
                if (hoverStatus == HoverStatus.NothingAround)
                {
                    drawingFigures = new Figure[] { new FreePoint(X, Y) };
                }
                else if (hoverStatus == HoverStatus.NearExistPoint)
                {
                    dragingFigure = HoverFigure;
                }
                else if (hoverStatus==HoverStatus.NearExistLine)
                {
                    drawingFigures = new Figure[] { new PointOnLine(HoverFigure as LineBase, X, Y) };
                }
                else if (hoverStatus == HoverStatus.NearExistCircle)
                {
                    drawingFigures = new Figure[] { new PointOnCircle(HoverFigure as CircleBase, X, Y) };
                }
                else if(hoverStatus==HoverStatus.NearExistIntersection)
                {
                    drawingFigures = new Figure[] {
                        IntersectPoint.CreateIntersection(HoverIntersectionA,HoverIntersectionB,X,Y)
                    };
                }
                Redraw();
            }
            else if(CurrentCreateMode==CreateMode.Nothing)
            {
                if (hoverStatus == HoverStatus.NearExistPoint)
                {
                    dragingFigure = HoverFigure;
                }
            }
            else // Circle or line
            {
                PointBase startPoint = null;
                if (hoverStatus == HoverStatus.NothingAround)
                {
                    startPoint = new FreePoint(X, Y);
                    Figures.Add(startPoint);
                }
                else if (hoverStatus == HoverStatus.NearExistPoint)
                {
                    startPoint = HoverFigure as PointBase;
                }
                else if (hoverStatus == HoverStatus.NearExistLine)
                {
                    startPoint = new PointOnLine(HoverFigure as LineBase, X, Y);
                    Figures.Add(startPoint);
                }
                else if (hoverStatus == HoverStatus.NearExistCircle)
                {
                    startPoint = new PointOnCircle(HoverFigure as CircleBase, X, Y);
                    Figures.Add(startPoint);
                }
                else if (hoverStatus == HoverStatus.NearExistIntersection)
                {
                    startPoint = IntersectPoint.CreateIntersection(HoverIntersectionA, HoverIntersectionB, X, Y);
                    Figures.Add(startPoint);
                }
                if (startPoint != null)
                {
                    PointBase endPoint = new FreePoint(X, Y);
                    Figure twoPointFigure = CreateTwoPointFigure(CurrentCreateMode, startPoint, endPoint);
                    drawingFigures = new Figure[] { twoPointFigure, endPoint };
                }
            }
        }
        public void MouseMove(int X,int Y)
        {
            if (dragingFigure != null)
            {
                dragingFigure.TryDragTo(X, Y);
                Redraw();
                return;
            }
            HoverStatus hoverStatus = GetHoverStatus(X, Y);
            if (isMouseDown&&drawingFigures!=null)
            {
                if (CurrentCreateMode == CreateMode.Point)
                {
                    drawingFigures[0].TryDragTo(X, Y);
                }
                else
                {
                    PointBase startPoint = null;// Save start point
                    if (CurrentCreateMode == CreateMode.Line)
                        startPoint = (drawingFigures[0] as TwoPointSegment).PS;
                    else if (CurrentCreateMode == CreateMode.Circle)
                        startPoint = (drawingFigures[0] as TwoPointCircle).PC;
                    // Then destroy everything when creating circles and lines
                    foreach (Figure figure in drawingFigures)
                    {
                        figure.Destroy();
                    }
                    drawingFigures = null;
                    // Then recreate everything
                    if (CurrentCreateMode == CreateMode.Line|| CurrentCreateMode == CreateMode.Circle)
                    {
                        PointBase endPoint = null;
                        Figure twoPointFigure = null;
                        if (hoverStatus == HoverStatus.NearExistPoint)
                        {
                            endPoint = HoverFigure as PointBase;
                            twoPointFigure = CreateTwoPointFigure(CurrentCreateMode, startPoint, endPoint);
                            drawingFigures = new Figure[] { twoPointFigure };
                        }
                        else
                        {
                            if (hoverStatus == HoverStatus.NearExistLine)
                                endPoint = new PointOnLine(HoverFigure as LineBase, X, Y);
                            else if (hoverStatus == HoverStatus.NearExistCircle)
                                endPoint = new PointOnCircle(HoverFigure as CircleBase, X, Y);
                            else if (hoverStatus == HoverStatus.NearExistIntersection)
                                endPoint = IntersectPoint.CreateIntersection(HoverIntersectionA, HoverIntersectionB, X, Y);
                            else
                                endPoint = new FreePoint(X, Y);
                            twoPointFigure = CreateTwoPointFigure(CurrentCreateMode, startPoint, endPoint);
                            drawingFigures = new Figure[] { twoPointFigure, endPoint };
                        }
                    }
                }
            }
            Redraw();
        }
        public void MouseUp(int X,int Y)
        {
            dragingFigure = null;
            if(isMouseDown&&drawingFigures != null)
            {
                foreach(Figure figure in drawingFigures)
                {
                    Figures.Add(figure);
                }
                drawingFigures = null;
            }
            Redraw();
            isMouseDown = false;
        }
    }
}
