using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UMLRedactor.Additions;
using UMLRedactor.Models;
using UMLRedactor.View;

namespace UMLRedactor.Tools.Elements.ActivityDiagram
{
    public partial class ActivityInitial : IElement
    {
        public ActivityInitial()
        {
            InitializeComponent();
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