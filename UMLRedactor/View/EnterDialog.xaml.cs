using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace UMLRedactor.View
{
    public partial class EnterDialog
    {
        
        public EnterDialog(string title, string caption)
        {
            InitializeComponent();
            Title.Text = title;
            Caption.Text = caption;
        }

        private void SystemButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = Brushes.Red;
        }

        private void SystemButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = Brushes.LightGray;
        }

        private void SystemButtonClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void TitleBar_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            if (e.ClickCount == 2)
                MainView.WindowState = MainView.WindowState == WindowState.Normal
                    ? WindowState.Maximized
                    : WindowState.Normal;
        }

        private void ButtonOk_Click(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
        }
    }
}