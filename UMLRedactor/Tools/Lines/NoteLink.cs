using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using UMLRedactor.Additions;
using UMLRedactor.Tools.Elements;

namespace UMLRedactor.Tools.Lines
{
    public class NoteLink : Shape, ILine
    {
        public int LocalId { get; set; }
        public Enums.LineTypes Type { get; set; }
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public bool SrcIsOrdered { get; set; }
        public bool DstIsOrdered { get; set; }
        public IElement Source { get; set; }
        public IElement Target { get; set; }
        
        private readonly LineGeometry _line = new LineGeometry();

        public NoteLink(Point startPoint, Point endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            Stroke=Brushes.Black;
            StrokeThickness = 6;
        }

        public NoteLink()
        {
            Stroke=Brushes.Black;
            StrokeThickness = 6;
            StrokeDashArray= DoubleCollection.Parse("3 2");
        }

        protected override Geometry DefiningGeometry {
            get
            {
                _line.StartPoint = StartPoint;
                _line.EndPoint = EndPoint;
                return _line;
            }
        }

        public void Move(Point newStartPoint, Point newEndPoint)
        {
            StartPoint = newStartPoint;
            EndPoint = newEndPoint;
        }
}
}