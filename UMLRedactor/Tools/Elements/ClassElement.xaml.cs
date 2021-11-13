using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UMLRedactor.Additions;
using UMLRedactor.View;

namespace UMLRedactor.Tools.Elements
{
    public partial class ClassElement: IElement
    {
        //private readonly List<TextBox> _tbs = new List<TextBox>();
        private int _edgeType;

        public Enums.ElementTypes Type { get; set; }

        public ClassElement()
        {
            InitializeComponent();
            MinWidth = 200;
            MinHeight = 150;
            Type = Enums.ElementTypes.ElementClass;
            //_tbs.Add(Line1);
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            RowDefinition rw = new RowDefinition { Height = GridLength.Auto };
            MainGrid.RowDefinitions.Add(rw);
            TextBox tb = new TextBox()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                FontSize = 16,
                MinHeight = 30,
                TextWrapping = TextWrapping.Wrap,
                BorderThickness = new Thickness(0, 0, 0, 2),
                BorderBrush = Brushes.Black,
                Margin = new Thickness(1, 0, 1, 0),
                Padding = new Thickness(5, 0, 5, 0)
            };
            Grid.SetRow(tb, MainGrid.RowDefinitions.Count - 2);
            Grid.SetRow(AddButton, MainGrid.RowDefinitions.Count - 1);
            MinHeight += 30;
            MainGrid.Children.Add(tb);
            //_tbs.Add(tb);
        }

        private void Mt_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point offset = e.GetPosition(this);
            Canvas canvas = Parent as Canvas;
            ScrollViewer sv = canvas?.Parent as ScrollViewer;
            Grid grid = sv?.Parent as Grid;
            if (grid?.Parent is MainWindow main)
            {
                main.IsSizing = true;
                main.BorderEdge = (int)Enums.EdgeTypes.MiddleTop;
                main.SelectedElement = this;
                main.SizingOffsetX = offset.X;
                main.SizingOffsetY = offset.Y;
            }
        }

        private void ClassElement_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Lt.Visibility = Visibility.Visible;
            Rt.Visibility = Visibility.Visible;
            Lb.Visibility = Visibility.Visible;
            Rb.Visibility = Visibility.Visible;
            Mt.Visibility = Visibility.Visible;
        }

        private void ClassElement_OnLostFocus(object sender, RoutedEventArgs e)
        {
            Lt.Visibility = Visibility.Hidden;
            Rt.Visibility = Visibility.Hidden;
            Lb.Visibility = Visibility.Hidden;
            Rb.Visibility = Visibility.Hidden;
            Mt.Visibility = Visibility.Hidden;
        }

        private void Lt_OnMouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.SizeNWSE;
        }

        private void Rt_OnMouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.SizeNESW;
        }

        private void Lb_OnMouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.SizeNESW;
        }

        private void Rb_OnMouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.SizeNWSE;
        }

        private void Mt_OnMouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void Lt_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _edgeType = (int)Enums.EdgeTypes.LeftTop;
            Setting_Resizing();
        }

        private void Rt_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _edgeType = (int)Enums.EdgeTypes.RightTop;
            Setting_Resizing();
        }

        private void Lb_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _edgeType = (int)Enums.EdgeTypes.LeftBottom;
            Setting_Resizing();
        }

        private void Rb_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _edgeType = (int)Enums.EdgeTypes.RightBottom;
            Setting_Resizing();
        }

        private void Setting_Resizing()
        {
            Canvas canvas = Parent as Canvas;
            ScrollViewer sv = canvas?.Parent as ScrollViewer;
            Grid grid = sv?.Parent as Grid;
            if (grid?.Parent is MainWindow main)
            {
                main.IsSizing = true;
                main.BorderEdge = _edgeType;
                main.SelectedElement = this;
            }
        }
    }
}