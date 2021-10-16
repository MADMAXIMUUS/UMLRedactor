using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace UMLRedactor.Elements
{
    public partial class ClassElement : UserControl
    {
        private Point _anchorPoint, _currentPoint;
        private bool _isInDrag;
        private TranslateTransform _translateTransform = new TranslateTransform();

        private readonly List<TextBox> _tbs = new List<TextBox>();

        public ClassElement()
        {
            InitializeComponent();
            _tbs.Add(Line1);
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            MainGrid.RowDefinitions.Add(new RowDefinition());
            TextBox tb = new TextBox()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                FontSize = 16,
                Height = 30,
                BorderThickness = new Thickness(0, 0, 0, 2),
                BorderBrush = Brushes.Black,
                Padding = new Thickness(5, 0, 5, 0)
            };
            Grid.SetRow(tb, MainGrid.RowDefinitions.Count - 2);
            Grid.SetRow(AddButton, MainGrid.RowDefinitions.Count - 1);
            MainGrid.Children.Add(tb);
            _tbs.Add(tb);
        }

        private void ClassElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            _anchorPoint = e.GetPosition(null);
            if (element != null) element.CaptureMouse();
            _isInDrag = true;
            e.Handled = true;
        }

        private void ClassElement_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isInDrag)
            {
                _currentPoint = e.GetPosition(null);

                _translateTransform.X += _currentPoint.X - _anchorPoint.X;
                _translateTransform.Y += (_currentPoint.Y - _anchorPoint.Y);
                if (_currentPoint.X < Application.Current.MainWindow.RenderSize.Width &&
                    _currentPoint.Y < Application.Current.MainWindow.RenderSize.Height
                    && _currentPoint.X > 0 && _currentPoint.Y > 0)
                {
                    RenderTransform = _translateTransform;
                    _anchorPoint = _currentPoint;
                }
                else
                {
                    _translateTransform = new TranslateTransform();
                    RenderTransform = _translateTransform;
                }
            }
        }

        private void ClassElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isInDrag)
            {
                if (sender is FrameworkElement element) element.ReleaseMouseCapture();
                _isInDrag = false;
                e.Handled = true;
            }
        }
    }
}