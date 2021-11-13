using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using UMLRedactor.Additions;

namespace UMLRedactor.Tools.Lines
{
    public class NoteLink : Shape, ILine
    {
        public Enums.LineTypes Type { get; set; }
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        private LineGeometry _line = new LineGeometry();

        public NoteLink(Point startPoint, Point endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
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