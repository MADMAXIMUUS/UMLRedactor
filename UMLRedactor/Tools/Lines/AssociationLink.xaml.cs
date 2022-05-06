using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using UMLRedactor.Models;

namespace UMLRedactor.Tools.Lines
{
    public partial class AssociationLink : ILine
    {
        private readonly ModelNodeLine _modelLine;
        private Point _startPosition, _endPosition;

        public AssociationLink(ModelNodeLine line, Point startPosition, Point endPosition)
        {
            InitializeComponent();
            _modelLine = line;
            _startPosition = startPosition;
            _endPosition = endPosition;
        }

        public ModelNodeLine GetModelElement()
        {
            return _modelLine;
        }

        private void AssociationLink_OnLoaded(object sender, RoutedEventArgs e)
        {
            Polyline polyline = new Polyline
            {
                Stroke = Brushes.Black,
                StrokeThickness = 6,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeLineJoin = PenLineJoin.Round
            };
            if (_startPosition.X > _endPosition.X)
            {
                if (_startPosition.Y > _endPosition.Y)
                {
                    polyline.Points = new PointCollection
                    {
                        new Point(0, 0),
                        new Point(ActualWidth / 2, 0),
                        new Point(ActualWidth / 2, 0),
                        new Point(ActualWidth / 2, ActualHeight),
                        new Point(ActualWidth / 2, ActualHeight),
                        new Point(ActualWidth, ActualHeight)
                    };
                }
                else
                {
                    polyline.Points = new PointCollection
                    {
                        new Point(0, ActualHeight),
                        new Point(ActualWidth / 2, ActualHeight),
                        new Point(ActualWidth / 2, ActualHeight),
                        new Point(ActualWidth / 2, 0),
                        new Point(ActualWidth / 2, 0),
                        new Point(ActualWidth, 0)
                    };
                }
            }
            else
            {
                if (_startPosition.Y > _endPosition.Y)
                {
                    polyline.Points = new PointCollection
                    {
                        new Point(ActualWidth, 0),
                        new Point(ActualWidth / 2, 0),
                        new Point(ActualWidth / 2, 0),
                        new Point(ActualWidth / 2, ActualHeight),
                        new Point(ActualWidth / 2, ActualHeight),
                        new Point(0, ActualHeight)
                    };
                }
                else
                {
                    polyline.Points = new PointCollection
                    {
                        new Point(ActualWidth, ActualHeight),
                        new Point(ActualWidth / 2, ActualHeight),
                        new Point(ActualWidth / 2, ActualHeight),
                        new Point(ActualWidth / 2, 0),
                        new Point(ActualWidth / 2, 0),
                        new Point(0, 0)
                    };
                }
            }
            MainCanvas.Children.Add(polyline);
        }
    }
}