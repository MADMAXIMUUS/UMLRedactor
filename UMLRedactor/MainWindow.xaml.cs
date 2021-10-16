using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UMLRedactor.Elements;

namespace UMLRedactor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public bool IsSizing;
        public int SizingEdgeType;
        public double SizingOffsetX;
        public double SizingOffsetY;
        public UserControl SizingPanel;

        public Border SizingEdge;

        enum EdgeTypes
        {
            TopMove = 0,
            TopLeft = 1,
            TopRight = 2,
            BottomLeft = 3,
            BottomRight = 4
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e) { }

        private void set_Sizing(object sender, MouseEventArgs e)
        {
            if (IsSizing == false) return;
            if (SizingEdgeType < 0) return;
            if (SizingPanel == null) return;
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                IsSizing = false;
            }
            else
            {
                Point mousePoint = e.GetPosition(this);
                double mouseX = mousePoint.X;
                double mouseY = mousePoint.Y;
                Point position = TranslatePoint(new Point(0, 0), SizingPanel);
                double posX = -position.X;
                double posY = -position.Y;
                double diffX = (posX - mouseX);
                double diffY = (posY - mouseY);
                if (SizingEdgeType == (int)EdgeTypes.TopMove)
                {
                    double newLeft = mouseX - SizingOffsetX;
                    double newTop = mouseY - SizingOffsetY;
                    if (newLeft > 0) Canvas.SetLeft(SizingPanel, newLeft);
                    if (newTop > 0) Canvas.SetTop(SizingPanel, newTop);
                }
                else
                {
                    if ((SizingEdgeType == (int)EdgeTypes.TopLeft) || (SizingEdgeType == (int)EdgeTypes.BottomLeft))
                    {
                        double new_Left = mouseX;
                        double newWidth = (SizingPanel.ActualWidth) + diffX;
                        if (new_Left > 0) Canvas.SetLeft(SizingPanel, new_Left);
                        if (newWidth > 0) SizingPanel.Width = newWidth;
                    }

                    if ((SizingEdgeType == (int)EdgeTypes.TopLeft) || (SizingEdgeType == (int)EdgeTypes.TopRight))
                    {
                        double new_Top = mouseY;
                        double newHeight = (SizingPanel.ActualHeight) + diffY;
                        if (new_Top > 0) Canvas.SetTop(SizingPanel, new_Top);
                        if (newHeight > 0) SizingPanel.Height = newHeight;
                    }

                    if ((SizingEdgeType == (int)EdgeTypes.TopRight) || (SizingEdgeType == (int)EdgeTypes.BottomRight))
                    {
                        double newWidth = mouseX - posX;
                        if (newWidth > 0) SizingPanel.Width = newWidth;
                    }

                    if ((SizingEdgeType == (int)EdgeTypes.BottomLeft) || (SizingEdgeType == (int)EdgeTypes.BottomRight))
                    {
                        double newHeight = mouseY - posY;
                        if (newHeight > 0) SizingPanel.Height = newHeight;
                    }
                }
            }
        }

        private void DrawCanvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (IsSizing) set_Sizing(sender, e);
        }

        private void DrawCanvas_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IsSizing)
            {
                IsSizing = false;
                SizingEdgeType = -1;
                SizingOffsetX = 0;
                SizingOffsetY = 0;
                SizingPanel = null;
                SizingEdge = null;
            }
        }
    }
}