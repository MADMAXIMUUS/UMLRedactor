using System.Windows;
using UMLRedactor.View;
using UMLRedactor.Controller;
using UMLRedactor.Models;

namespace UMLRedactor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App: Application {
        

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            DomModel model = new DomModel();
            Controller.Controller controller = new Controller.Controller(model);
            MainWindow mw = new MainWindow(controller);
            mw.Show();
        }

    }
}