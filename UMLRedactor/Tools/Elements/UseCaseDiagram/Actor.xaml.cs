using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UMLRedactor.Additions;
using UMLRedactor.Models;
using UMLRedactor.View;

namespace UMLRedactor.Tools.Elements.UseCaseDiagram
{
    public partial class Actor : IElement
    {
        public Actor()
        {
            InitializeComponent();
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