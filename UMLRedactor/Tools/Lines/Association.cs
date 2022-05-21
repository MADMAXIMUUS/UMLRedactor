using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using UMLRedactor.Models;

namespace UMLRedactor.Tools.Lines
{
    public class Association: Shape, ILine
    {
        private readonly ModelNodeLine _modelNodeLine;
        public double X1;
        public double Y1;
        public double X2;
        public double Y2;

        public Association(ModelNodeLine line)
        {
            _modelNodeLine = line;
            Stroke = Brushes.Black;
            StrokeThickness = 6;
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                LineGeometry line =new LineGeometry(
                    new Point(X1, Y1), 
                    new Point(X2, Y2));
                return line;
            }
        }

        public ModelNodeLine GetModelElement()
        {
            return _modelNodeLine;
        }
    }
}