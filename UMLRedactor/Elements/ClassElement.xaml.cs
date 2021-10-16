using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace UMLRedactor.Elements
{
    public partial class ClassElement : UserControl
    {
        private readonly List<TextBox> _tbs = new List<TextBox>();
        private int _edgeType;

        enum EdgeTypes
        {
            TopLeft = 1,
            TopRight = 2,
            BottomLeft = 3,
            BottomRight = 4
        }

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
        
        private void Mt_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point offset = e.GetPosition(Mt);
            double offsetX = offset.X;
            double offsetY = offset.Y;
            if (Parent is Canvas canvasMain && canvasMain.Parent is MainWindow main)
            {
                main.IsSizing = true;
                main.SizingEdgeType = 0;
                main.SizingPanel = this;
                main.SizingEdge = sender as Border;
                main.SizingOffsetX = offsetX;
                main.SizingOffsetY = offsetY;
            }
        }

        private void ClassElement_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Lt.Visibility = Visibility.Visible;
            Rt.Visibility = Visibility.Visible;
            Lb.Visibility = Visibility.Visible;
            Rb.Visibility = Visibility.Visible;
            Mt.Visibility = Visibility.Visible;
            Mb.Visibility = Visibility.Visible;
        }

        private void ClassElement_OnLostFocus(object sender, RoutedEventArgs e)
        {
            Lt.Visibility = Visibility.Hidden;
            Rt.Visibility = Visibility.Hidden;
            Lb.Visibility = Visibility.Hidden;
            Rb.Visibility = Visibility.Hidden;
            Mt.Visibility = Visibility.Hidden;
            Mb.Visibility = Visibility.Hidden;
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
            _edgeType = (int)EdgeTypes.TopLeft;
            set_Sizing(sender, e);
        }

        private void Rt_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _edgeType = (int)EdgeTypes.TopRight;
            set_Sizing(sender, e);
        }

        private void Lb_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _edgeType = (int)EdgeTypes.BottomLeft;
            set_Sizing(sender, e);
        }

        private void Rb_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _edgeType = (int)EdgeTypes.BottomRight;
            set_Sizing(sender, e);
        }

        private void set_Sizing(object sender, MouseButtonEventArgs e)
        {
            Canvas canvasMain = Parent as Canvas;
            if (canvasMain?.Parent is MainWindow main)
            {
                main.IsSizing = true;
                main.SizingEdgeType = _edgeType;
                main.SizingPanel = this;
                main.SizingEdge = sender as Border;
            }
        }

        private void Mt_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}