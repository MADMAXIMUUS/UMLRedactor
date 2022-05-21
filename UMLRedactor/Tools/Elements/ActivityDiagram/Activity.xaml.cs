using UMLRedactor.Models;

namespace UMLRedactor.Tools.Elements.ActivityDiagram
{
    public partial class Activity : IElement
    {
        private readonly ModelNodeElement _modelNodeElement;

        public Activity(ModelNodeElement element)
        {
            InitializeComponent();
            MinWidth = 150;
            MinHeight = 100;
            _modelNodeElement = element;
            Title.Text = _modelNodeElement.Name;
        }

        public ModelNodeElement GetModelElement()
        {
            return _modelNodeElement;
        }
    }
}