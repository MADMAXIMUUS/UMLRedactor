using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            ButtonOpen.Click += _controller.OpenFile;
        }
    }
}