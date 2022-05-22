using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UMLRedactor.Additions;
using UMLRedactor.Models;
using Attribute = UMLRedactor.Additions.Attribute;

namespace UMLRedactor.Tools.Elements.ClassDiagram
{
    public partial class ClassElement : IElement
    {
        private readonly ModelNodeElement _element;
        public event EventHandler RemovedElement;
        public ClassElement(ModelNodeElement modelNodeBase)
        {
            InitializeComponent();
            AddContextMenu();
            _element = modelNodeBase;
            Title.Text = modelNodeBase.Name;
            MinWidth = 200;
            MinHeight = 150;
            if (_element != null && !string.IsNullOrEmpty(_element.Stereotype))
            {
                Stereotype.Text = "<<" + _element.Stereotype + ">>";
                Stereotype.Visibility = Visibility.Visible;
            }

            List<Attribute> attributes = _element?.Attributes;
            if (attributes != null)
                foreach (Attribute attribute in attributes)
                    CreateAttribute(attribute);

            List<Operation> operations = _element?.Operations;
            if (operations != null)
                foreach (Operation operation in operations)
                    CreateOperation(operation);
        }

        private void CreateAttribute(Attribute attribute)
        {
            string text = "";
            if (attribute.Name=="")
                return;
            if (attribute.AccessModifier == "Public" || 
                attribute.AccessModifier == "public")
                text += "+ ";
            else if (attribute.AccessModifier == "Protected" || 
                     attribute.AccessModifier == "protected")
                text += "# ";
            else if (attribute.AccessModifier == "Private" || 
                     attribute.AccessModifier == "private")
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
            if (operation.Name=="")
                return;
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

        public ModelNodeElement GetModelElement()
        {
            return _element;
        }

        public void AddContextMenu()
        {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem removeElement = new MenuItem { Header = "Remove" };
            removeElement.Click += RemoveElement;
            contextMenu.Items.Add(removeElement);
            ContextMenu = contextMenu;
        }

        private void RemoveElement(object sender, RoutedEventArgs routedEventArgs)
        {
            RemovedElement?.Invoke(_element.Id, EventArgs.Empty);
        }
    }
}