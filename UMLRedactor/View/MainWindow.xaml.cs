using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UMLRedactor.Additions;

namespace UMLRedactor.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Controllers.Controller _controller;

        public MainWindow(Controllers.Controller controller)
        {
            InitializeComponent();
            _controller = controller;
            InitFunction();
        }

        private void InitFunction()
        {
            ButtonFileOpen.Click += _controller.OpenFile;
            ButtonFileNew.Click += _controller.CreateNew;
            ButtonFileSave.Click += _controller.Save;
            ButtonFileSaveAs.Click += _controller.SaveAs;
            ButtonFileExport.Click += _controller.Export;
            ButtonEditRedo.Click += _controller.Redo;
            ButtonEditUndo.Click += _controller.Undo;
            ButtonViewNew.Click += _controller.NewDiagram;
            ButtonViewClose.Click += _controller.CloseDiagram;
            ButtonViewNext.Click += _controller.NextDiagram;
            ButtonViewPrev.Click += _controller.PrevDiagram;
            ButtonViewOpen.Click += _controller.OpenDiagram;
            ButtonViewSave.Click += _controller.SaveDiagram;
        }

        private void SystemButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = ((Border)sender)?.Name == "SystemButtonClose" 
                ? Brushes.Red 
                : Brushes.LightBlue;
        }

        private void SystemButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = Brushes.LightGray;
        }

        private void SystemButtonTray_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainView.WindowState = WindowState.Minimized;
        }

        private void SystemButtonMaximize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainView.WindowState = MainView.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void SystemButtonClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TitleBar_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            if (e.ClickCount == 2)
                MainView.WindowState = MainView.WindowState == WindowState.Normal 
                    ? WindowState.Maximized 
                    : WindowState.Normal;
        }
    }
}