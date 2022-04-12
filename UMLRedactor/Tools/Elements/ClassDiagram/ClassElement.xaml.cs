using System.Windows;
using System.Windows.Input;
using UMLRedactor.Models;

namespace UMLRedactor.Tools.Elements.ClassDiagram
{
    public partial class ClassElement : IElement
    {
        public ClassElement(ModelNodeBase modelNodeBase)
        {
            InitializeComponent();
            Title.Text = modelNodeBase.Name;
            MinWidth = 200;
            MinHeight = 150;
        }
    }
}