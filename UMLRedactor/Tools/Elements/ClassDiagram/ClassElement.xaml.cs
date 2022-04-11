using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UMLRedactor.Additions;

namespace UMLRedactor.Tools.Elements.ClassDiagram
{
    public partial class ClassElement : IElement
    {
        public ClassElement()
        {
            InitializeComponent();
            MinWidth = 200;
            MinHeight = 150;
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
    }
}