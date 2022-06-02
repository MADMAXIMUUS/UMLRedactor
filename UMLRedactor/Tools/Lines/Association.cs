using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using UMLRedactor.Additions;
using UMLRedactor.Models;

namespace UMLRedactor.Tools.Lines
{
    public class Association : Shape, ILine
    {
        private readonly LineGeometry _lineGeometry;
        private readonly ModelNodeLine _modelNodeLine;

        
        public static readonly DependencyProperty X1Property =
            DependencyProperty.Register("X1",
                typeof(double), typeof(Association),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        
        public double X1
        {
            set => SetValue(X1Property, value);
            get => (double)GetValue(X1Property);
        }

        public double Y1
        {
            set => SetValue(Y1Property, value);
            get => (double)GetValue(Y1Property);
        }

        public double X2
        {
            set => SetValue(X2Property, value);
            get => (double)GetValue(X2Property);
        }

        public Association(ModelNodeLine line)
        {
            _lineGeometry = new LineGeometry();
            _modelNodeLine = line;
            Stroke = Brushes.Black;
            StrokeThickness = 6;
        }

        public static readonly DependencyProperty Y1Property =
            DependencyProperty.Register("Y1",
                typeof(double), typeof(Association),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        
        public static readonly DependencyProperty X2Property =
            DependencyProperty.Register("X2",
                typeof(double), typeof(Association),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));
        
        public double Y2
        {
            set => SetValue(Y2Property, value);
            get => (double)GetValue(Y2Property);
        }
        
        public static readonly DependencyProperty Y2Property =
            DependencyProperty.Register("Y2",
                typeof(double), typeof(Association),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.AffectsMeasure));

        public void MoveLine(object sender, MoveLineEventArgs eventArgs)
        {
            if ((sender as ModelNodeElement)?.Id == _modelNodeLine.Source)
            {
                X1 = eventArgs.NewX + eventArgs.Width / 2;
                Y1 = eventArgs.NewY + eventArgs.Height / 2;
            }
            else if ((sender as ModelNodeElement)?.Id == _modelNodeLine.Target)
            {
                X2 = eventArgs.NewX + eventArgs.Width / 2;
                Y2 = eventArgs.NewY + eventArgs.Height / 2;
            }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                _lineGeometry.StartPoint = new Point(X1, Y1);
                _lineGeometry.EndPoint = new Point(X2, Y2);
                return _lineGeometry;
            }
        }

        public ModelNodeLine GetModelElement()
        {
            return _modelNodeLine;
        }
    }
}