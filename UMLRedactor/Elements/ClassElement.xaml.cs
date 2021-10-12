using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UMLRedactor.Elements
{
    public partial class ClassElement : UserControl
    {
        public ClassElement()
        {
            InitializeComponent();
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            MainGrid.RowDefinitions.Add(new RowDefinition());
            TextBox tb = new TextBox()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                FontSize = 16,
                Height = 30,
                BorderThickness = new Thickness(0, 0, 0, 2),
                BorderBrush = Brushes.Black
            };
            Grid.SetRow(tb, MainGrid.RowDefinitions.Count - 2);
            Grid.SetRow(AddButton, MainGrid.RowDefinitions.Count - 1);
            MainGrid.Children.Add(tb);
        }
    }
}