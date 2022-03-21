using System.Windows;
using UMLRedactor.View;
using UMLRedactor.Models;

namespace UMLRedactor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App: Application {
        

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Model model = new Model();
            Controllers.Controller controller = new Controllers.Controller(model);
            MainWindow mw = new MainWindow(controller);
            mw.Show();
        }

    }
}