using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UMLRedactor.Additions;
using UMLRedactor.Tools.Elements;

namespace UMLRedactor.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public bool IsSizing;
        public int BorderEdge;
        public double SizingOffsetX;
        public double SizingOffsetY;
        public UserControl SelectedElement;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ResizeAndTranslate(MouseEventArgs e)
        {
            if (!IsSizing) return;
            if (BorderEdge < 0) return;
            if (SelectedElement == null) return;

            if (e.LeftButton != MouseButtonState.Pressed)
            {
                IsSizing = false;
            }
            else
            {
                Point cursorPosition = e.GetPosition(DrawCanvas);
                Point position = new Point(Canvas.GetLeft(SelectedElement), Canvas.GetTop(SelectedElement));
                double diffX = position.X - cursorPosition.X;
                double diffY = position.Y - cursorPosition.Y;
                if (BorderEdge == (int)Enums.EdgeTypes.MiddleTop)
                {
                    double newLeft = cursorPosition.X - SizingOffsetX;
                    double newTop = cursorPosition.Y - SizingOffsetY;
                    if (newLeft > 0) Canvas.SetLeft(SelectedElement, newLeft);
                    if (newTop > 0) Canvas.SetTop(SelectedElement, newTop);
                    SelectedElement.UpdateLayout();
                }
                else
                {
                    if (BorderEdge == (int)Enums.EdgeTypes.LeftTop || BorderEdge == (int)Enums.EdgeTypes.LeftBottom)
                    {
                        double newLeft = cursorPosition.X;
                        double newWidth = SelectedElement.Width + diffX;
                        if (newLeft > 0 && newWidth > SelectedElement.MinWidth)
                            Canvas.SetLeft(SelectedElement, newLeft);
                        if (newWidth > SelectedElement.MinWidth) SelectedElement.Width = newWidth;
                        SelectedElement.UpdateLayout();
                    }

                    if (BorderEdge == (int)Enums.EdgeTypes.LeftTop || BorderEdge == (int)Enums.EdgeTypes.RightTop)
                    {
                        double newTop = cursorPosition.Y;
                        double newHeight = SelectedElement.Height + diffY;
                        if (newTop > 0 && newHeight > SelectedElement.MinHeight) Canvas.SetTop(SelectedElement, newTop);
                        if (newHeight > SelectedElement.MinHeight) SelectedElement.Height = newHeight;
                        SelectedElement.UpdateLayout();
                    }

                    if (BorderEdge == (int)Enums.EdgeTypes.RightTop || BorderEdge == (int)Enums.EdgeTypes.RightBottom)
                    {
                        double newWidth = cursorPosition.X + position.X;
                        if (newWidth > SelectedElement.MinWidth) SelectedElement.Width = newWidth;
                        SelectedElement.UpdateLayout();
                    }

                    if (BorderEdge == (int)Enums.EdgeTypes.LeftBottom || BorderEdge == (int)Enums.EdgeTypes.RightBottom)
                    {
                        double newHeight = cursorPosition.Y + position.Y;
                        if (newHeight > SelectedElement.MinHeight) SelectedElement.Height = newHeight;
                        SelectedElement.UpdateLayout();
                    }
                }

                ViewActualHeight.Text =
                    "ActualHeight = " + SelectedElement.ActualHeight.ToString(CultureInfo.CurrentCulture);
                ViewActualWidth.Text =
                    "ActualWidth = " + SelectedElement.ActualWidth.ToString(CultureInfo.CurrentCulture);
            }
        }

        private void DrawCanvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (IsSizing) ResizeAndTranslate(e);
        }

        private void DrawCanvas_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IsSizing)
            {
                IsSizing = false;
                BorderEdge = -1;
                SizingOffsetX = 0;
                SizingOffsetY = 0;
                SelectedElement = null;
            }
        }
    }
}