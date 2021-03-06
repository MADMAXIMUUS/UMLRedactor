using System.Windows;
using UMLRedactor.Controllers;
using UMLRedactor.Models;
using UMLRedactor.View;

namespace UMLRedactor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Model model = new Model();
            Controller controller = new Controller(model);
            MainWindow mw = new MainWindow(controller);
            mw.Show();
        }
    }
}