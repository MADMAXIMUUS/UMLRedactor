using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using UMLRedactor.Controllers;

namespace UMLRedactor.Additions
{
    public class ResizeThumb : Thumb
    {
        public ResizeThumb()
        {
            DragDelta += ResizeThumb_DragDelta;
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (DataContext is UserControl designerItem)
            {
                designerItem.Height = designerItem.ActualHeight;
                designerItem.Width = designerItem.ActualWidth;
                double deltaVertical, deltaHorizontal;

                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-e.VerticalChange, 
                            designerItem.ActualHeight - designerItem.MinHeight);
                        designerItem.Height -= deltaVertical;
                        break;
                    case VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange, 
                            designerItem.ActualHeight - designerItem.MinHeight);
                        Canvas.SetTop(designerItem, 
                            Canvas.GetTop(designerItem) + deltaVertical);
                        designerItem.Height -= deltaVertical;
                        break;
                }

                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(e.HorizontalChange, 
                            designerItem.ActualWidth - designerItem.MinWidth);
                        Canvas.SetLeft(designerItem, 
                            Canvas.GetLeft(designerItem) + deltaHorizontal);
                        designerItem.Width -= deltaHorizontal;
                        break;
                    case HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-e.HorizontalChange,
                            designerItem.ActualWidth - designerItem.MinWidth);
                        designerItem.Width -= deltaHorizontal;
                        break;
                }
            }

            e.Handled = true;
        }
    }
}





