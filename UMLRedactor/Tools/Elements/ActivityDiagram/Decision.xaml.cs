using System.Windows;
using System.Windows.Input;
using UMLRedactor.Additions;
using UMLRedactor.Models;

namespace UMLRedactor.Tools.Elements.ActivityDiagram
{
    public partial class Decision : IElement
    {
        public Decision()
        {
            InitializeComponent();
            MinHeight = 200;
            MinWidth = 200;
            DataContext = this;
        }
        public ModelNodeElement GetModelElement()
        {
            return new ModelNodeElement();
        }

        public void AddContextMenu()
        {
            
        }
    }
}