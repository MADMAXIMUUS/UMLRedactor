using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UMLRedactor.Additions.AdditionElements
{
    public class DecisionRhombus : Shape
    {
        private PathGeometry _pathGeometry;
        public double Size { get; set; }

        protected override Geometry DefiningGeometry
        {
            get
            {
                Point a = new Point(ActualWidth / 2, 0);
                Point b = new Point(ActualWidth, ActualHeight / 2);
                Point c = new Point(ActualWidth / 2, ActualHeight);
                Point d = new Point(0, ActualHeight / 2);
                List<PathSegment> segments = new List<PathSegment>(4)
                {
                    new LineSegment(a, true),
                    new LineSegment(b, true),
                    new LineSegment(c, true),
                    new LineSegment(d, true)
                };
                List<PathFigure> figures = new List<PathFigure>(1)
                {
                    new PathFigure(a, segments, true)
                };
                _pathGeometry = new PathGeometry(figures, FillRule.EvenOdd, null);
                return _pathGeometry;
            }
        }
    }
}