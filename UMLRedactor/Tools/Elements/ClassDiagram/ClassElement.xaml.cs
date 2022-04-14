using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UMLRedactor.Additions;
using UMLRedactor.Models;

namespace UMLRedactor.Tools.Elements.ClassDiagram
{
    public partial class ClassElement : IElement
    {
        public ModelNodeElement Element;
        
        public ClassElement(ModelNodeBase modelNodeBase)
        {
            InitializeComponent();
            Element = modelNodeBase as ModelNodeElement; 
            Title.Text = modelNodeBase.Name;
            MinWidth = 200;
            MinHeight = 150;

            List<Attribute> attributes = (modelNodeBase as ModelNodeElement)?.Attributes;
            if (attributes != null)
                foreach (Attribute attribute in attributes)
                    CreateAttribute(attribute);

            List<Operation> operations = (modelNodeBase as ModelNodeElement)?.Operations;
            if (operations != null)
                foreach (Operation operation in operations)
                    CreateOperation(operation);

            Height = Height;
            Width = Width;
        }

        private void CreateAttribute(Attribute attribute)
        {
            string text = "";
            if (attribute.AccessModifier == "Public" || attribute.AccessModifier == "public")
                text += "+ ";
            else if (attribute.AccessModifier == "Protected" || attribute.AccessModifier == "protected")
                text += "# ";
            else if (attribute.AccessModifier == "Private" || attribute.AccessModifier == "private")
                text += "- ";
            else
                text += "";
            TextBlock textAttribute = new TextBlock
            {
                Text = text + attribute.Name + ": " + attribute.DataType,
                FontSize = 12,
                MinHeight = 15,
                Background = Brushes.Transparent,
                TextWrapping = TextWrapping.Wrap,
                Padding = new Thickness(10, 0, 5, 0)
            };
            Border border = new Border
            {
                BorderBrush = Brushes.DarkGray,
                BorderThickness = new Thickness(0, 0, 0, 1),
                Child = textAttribute
            };
            AttributePanel.Children.Add(border);
        }

        private void CreateOperation(Operation operation)
        {
            string text = "";
            if (operation.AccessModifier == "Public" || operation.AccessModifier == "public")
                text += "+ ";
            else if (operation.AccessModifier == "Protected" || operation.AccessModifier == "protected")
                text += "# ";
            else if (operation.AccessModifier == "Private" || operation.AccessModifier == "private")
                text += "- ";
            else
                text += "";
            TextBlock textOperation = new TextBlock
            {
                FontSize = 12,
                MinHeight = 15,
                TextWrapping = TextWrapping.Wrap,
                Background = Brushes.Transparent,
                Padding = new Thickness(10, 0, 5, 0)
            };
            Border border = new Border
            {
                BorderBrush = Brushes.DarkGray,
                BorderThickness = new Thickness(0, 0, 0, 1),
                Child = textOperation
            };
            text += operation.Name + "(";
            List<Parameter> parameters = operation.Parameters;
            if (parameters != null)
                foreach (Parameter parameter in operation.Parameters)
                    text += parameter.DataType + ", ";

            text = text.Substring(0, text.Length - 2);

            text += "): " + operation.DataTypeOfReturnValue;
            textOperation.Text = text;
            OperationPanel.Children.Add(border);
        }
    }
}