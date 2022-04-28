using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using UMLRedactor.Models;

namespace UMLRedactor.Tools.Lines
{
    public partial class AssociationLink : ILine
    {
        private readonly ModelNodeLine _modelLine;

        public AssociationLink(/*ModelNodeLine line*/)
        {
            InitializeComponent();
            //_modelLine = line;
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
                StrokeLineJoin = PenLineJoin.Round,
                Points =
                {
                    new Point(0,0),
                    new Point(ActualWidth / 2,0),
                    new Point(ActualWidth / 2,0),
                    new Point(ActualWidth / 2,ActualHeight),
                    new Point(ActualWidth / 2,ActualHeight),
                    new Point(ActualWidth,ActualHeight)
                }
            };
            MainCanvas.Children.Add(polyline);
        }
    }
}