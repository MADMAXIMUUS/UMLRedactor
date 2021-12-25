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

        public MainWindow()
        {
            InitializeComponent();
            _controller = new Controller.Controller(this);
            InitFunction();
        }

        private void InitFunction()
        {
            DrawCanvas.MouseMove += _controller.OnMouseMove;
        }
    }
}