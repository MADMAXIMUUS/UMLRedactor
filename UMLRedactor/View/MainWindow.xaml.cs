using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UMLRedactor.Additions;
using UMLRedactor.Controller;

namespace UMLRedactor.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Controller.Controller _controller;

        public MainWindow(Controller.Controller controller)
        {
            InitializeComponent();
            _controller = controller;
            InitFunction();
        }

        private void InitFunction()
        {
            DrawCanvas.MouseMove += _controller.OnMouseMove;
            ButtonFileOpen.Click += _controller.OpenFile;
        }

        private void SytemButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if ((sender as Border).Name == "SystemButtonClose")
                (sender as Border).Background = Brushes.Red;
            else
                (sender as Border).Background = Brushes.LightBlue;
        }

        private void SytemButton_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Border).Background = Brushes.WhiteSmoke;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainView.WindowState = WindowState.Minimized;
        }

        private void SystemButtonMaximize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MainView.WindowState == WindowState.Normal)
                MainView.WindowState = WindowState.Maximized;
            else
                MainView.WindowState = WindowState.Normal;
        }

        private void SystemButtonClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}