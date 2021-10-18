using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UMLRedactor.Additions;

namespace UMLRedactor
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
        public UserControl SizingPanel;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ResizeAndTranslate(MouseEventArgs e)
        {
            if (!IsSizing) return;
            if (BorderEdge < 0) return;
            if (SizingPanel == null) return;

            if (e.LeftButton != MouseButtonState.Pressed)
            {
                IsSizing = false;
            }
            else
            {
                Point cursorPosition = e.GetPosition(DrawCanvas);
                Point position = new Point(Canvas.GetLeft(SizingPanel), Canvas.GetTop(SizingPanel));
                double diffX = position.X - cursorPosition.X;
                double diffY = position.Y - cursorPosition.Y;
                if (BorderEdge == (int)Enums.EdgeTypes.MiddleTop)
                {
                    double newLeft = cursorPosition.X - SizingOffsetX;
                    double newTop = cursorPosition.Y - SizingOffsetY;
                    if (newLeft > 0) Canvas.SetLeft(SizingPanel, newLeft);
                    if (newTop > 0) Canvas.SetTop(SizingPanel, newTop);
                }
                else
                {
                    if (BorderEdge == (int)Enums.EdgeTypes.LeftTop || BorderEdge == (int)Enums.EdgeTypes.LeftBottom)
                    {
                        double newLeft = cursorPosition.X;
                        double newWidth = SizingPanel.Width + diffX;
                        if (newLeft > 0 && newWidth > SizingPanel.MinWidth) Canvas.SetLeft(SizingPanel, newLeft);
                        if (newWidth > SizingPanel.MinWidth) SizingPanel.Width = newWidth;
                    }

                    if (BorderEdge == (int)Enums.EdgeTypes.LeftTop || BorderEdge == (int)Enums.EdgeTypes.RightTop)
                    {
                        double newTop = cursorPosition.Y;
                        double newHeight = SizingPanel.Height + diffY;
                        if (newTop > 0 && newHeight > SizingPanel.MinHeight) Canvas.SetTop(SizingPanel, newTop);
                        if (newHeight > SizingPanel.MinHeight) SizingPanel.Height = newHeight;
                    }

                    if (BorderEdge == (int)Enums.EdgeTypes.RightTop || BorderEdge == (int)Enums.EdgeTypes.RightBottom)
                    {
                        double newWidth = cursorPosition.X + position.X;
                        if (newWidth > SizingPanel.MinWidth) SizingPanel.Width = newWidth;
                    }

                    if (BorderEdge == (int)Enums.EdgeTypes.LeftBottom || BorderEdge == (int)Enums.EdgeTypes.RightBottom)
                    {
                        double newHeight = cursorPosition.Y + position.Y;
                        if (newHeight > SizingPanel.MinHeight) SizingPanel.Height = newHeight;
                    }
                }
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
                SizingPanel = null;
            }
        }
    }
}