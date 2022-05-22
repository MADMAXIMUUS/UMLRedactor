using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UMLRedactor.Additions;
using UMLRedactor.Controllers;
using UMLRedactor.Models;
using UMLRedactor.Tools.Elements.ActivityDiagram;
using UMLRedactor.Tools.Elements.ClassDiagram;
using UMLRedactor.Tools.Lines;
using Attribute = UMLRedactor.Additions.Attribute;

namespace UMLRedactor.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Controller _controller;

        public MainWindow(Controller controller)
        {
            InitializeComponent();
            _controller = controller;
            InitFunction();
        }

        private void InitFunction()
        {
            ButtonFileOpen.Click += _controller.OpenFile;
            ButtonFileNew.Click += _controller.NewFile;
            ButtonFileSave.Click += _controller.SaveFile;
            ButtonFileSaveAs.Click += _controller.SaveAs;
            ButtonFileExport.Click += _controller.Export;
            ButtonEditRedo.Click += _controller.Redo;
            ButtonEditUndo.Click += _controller.Undo;
            ButtonViewNew.Click += _controller.NewDiagram;
            ButtonViewClose.Click += _controller.CloseDiagram;
            ButtonViewNext.Click += _controller.NextDiagram;
            ButtonViewPrev.Click += _controller.PrevDiagram;
            ButtonViewOpen.Click += _controller.OpenDiagram;
            ButtonViewSave.Click += _controller.SaveDiagram;
            Class.Click += _controller.CreateElement;
            Activity.Click += _controller.CreateElement;
            Note.Click += _controller.CreateElement;
            Decision.Click += _controller.CreateElement;
            UseCase.Click += _controller.CreateElement;
            Aggregation.Click += _controller.CreateElement;
            Association.Click += _controller.CreateElement;
            Association.Click += ShowUnselectedButton;
            Generalization.Click += _controller.CreateElement;
            Generalization.Click += ShowUnselectedButton;
            Composition.Click += _controller.CreateElement;
            Composition.Click += ShowUnselectedButton;
            End.Click += _controller.CreateElement;
            Initial.Click += _controller.CreateElement;
            Lifecycle.Click += _controller.CreateElement;
            ActorLifecycle.Click += _controller.CreateElement;
            Message.Click += _controller.CreateElement;
            Use.Click += _controller.CreateElement;
            ControlFlow.Click += _controller.CreateElement;
            _controller.EndModelRead += DrawTree;
            _controller.EndModelRead += InitialDiagram;
            _controller.NewModel += RecreateView;
            _controller.ElementCreated += UpdateCanvas;
            _controller.ElementCreated += DrawTree;
            _controller.TreeViewItemSelected += ShowProperties;
            _controller.TreeViewItemSelected += ShowAttributesAndOperations;
            _controller.DiagramElementSelected += ShowProperties;
            _controller.DiagramElementSelected += ShowAttributesAndOperations;
            _controller.AttributeChanged += UpdateCanvas;
            _controller.OperationChanged += UpdateCanvas;
            _controller.PropertyChanged += UpdateCanvas;
        }

        private void ShowUnselectedButton(object sender, EventArgs e)
        {
            UnselectedLine.Visibility = Visibility.Visible;
            UnselectedLine.MouseLeftButtonDown += _controller.UnselectedLine;
            UnselectedLine.MouseLeftButtonDown += (o, args) => { UnselectedLine.Visibility = Visibility.Collapsed; };
        }

        private void ShowAttributesAndOperations(object sender, EventArgs e)
        {
            if (sender is ModelNodeElement element)
            {
                List<Attribute> attributes = element.Attributes;
                if (attributes != null && attributes.Count > 0)
                {
                    AttributesGrid.Children.Clear();
                    AttributesGrid.RowDefinitions.Clear();
                    foreach (Attribute attribute in attributes)
                    {
                        CreateAttribute(attribute);
                    }

                    CreateAttribute(new Attribute());
                }
                else
                {
                    AttributesGrid.Children.Clear();
                    AttributesGrid.RowDefinitions.Clear();
                    CreateAttribute(new Attribute());
                }

                List<Operation> operations = element.Operations;
                if (operations != null && operations.Count > 0)
                {
                    OperationsGrid.Children.Clear();
                    OperationsGrid.RowDefinitions.Clear();
                    foreach (Operation operation in operations)
                    {
                        CreateOperation(operation);
                    }

                    CreateOperation(new Operation());
                }
                else
                {
                    OperationsGrid.Children.Clear();
                    OperationsGrid.RowDefinitions.Clear();
                    CreateOperation(new Operation());
                }
            }
        }

        private void CreateAttribute(Attribute attribute)
        {
            RowDefinition rd = new RowDefinition
            {
                Height = new GridLength(30)
            };
            AttributesGrid.RowDefinitions.Add(rd);
            TextBox attributeName = new TextBox
            {
                Text = attribute.Name,
                FontSize = 14,
                Padding = new Thickness(0, 5, 0, 5),
                Foreground = Brushes.Black
            };
            attributeName.KeyDown += _controller.Attribute_OnKeyDown;
            Grid.SetColumn(attributeName, 0);
            Grid.SetRow(attributeName, AttributesGrid.RowDefinitions.Count - 1);
            TextBox attributeType = new TextBox
            {
                Text = attribute.DataType,
                FontSize = 14,
                Padding = new Thickness(0, 5, 0, 5),
                Foreground = Brushes.Black,
            };
            attributeType.KeyDown += _controller.Attribute_OnKeyDown;
            Grid.SetColumn(attributeType, 1);
            Grid.SetRow(attributeType, AttributesGrid.RowDefinitions.Count - 1);
            ComboBox attributeAccess = CreateComboBox(attribute.AccessModifier);
            attributeAccess.DropDownClosed += _controller.Attribute_ComboBoxSelected;
            Grid.SetColumn(attributeAccess, 2);
            Grid.SetRow(attributeAccess, AttributesGrid.RowDefinitions.Count - 1);
            AttributesGrid.Children.Add(attributeName);
            AttributesGrid.Children.Add(attributeType);
            AttributesGrid.Children.Add(attributeAccess);
        }

        private ComboBox CreateComboBox(string accessModifier)
        {
            ComboBox comboBox = new ComboBox
            {
                Padding = new Thickness(5, 0, 0, 5),
            };
            TextBlock publicTb = new TextBlock
            {
                Height = 30,
                Text = "Public",
                Padding = new Thickness(0, 5, 0, 5),
                FontSize = 14
            };
            comboBox.Items.Add(publicTb);
            TextBlock privateTb = new TextBlock
            {
                Height = 30,
                Text = "Private",
                Padding = new Thickness(0, 5, 0, 5),
                FontSize = 14
            };
            comboBox.Items.Add(privateTb);
            TextBlock privateProtectedTb = new TextBlock
            {
                Height = 30,
                Text = "Private Protected",
                Padding = new Thickness(0, 5, 0, 5),
                FontSize = 14
            };
            comboBox.Items.Add(privateProtectedTb);
            TextBlock protectedTb = new TextBlock
            {
                Height = 30,
                Text = "Protected",
                Padding = new Thickness(0, 5, 0, 5),
                FontSize = 14
            };
            comboBox.Items.Add(protectedTb);
            if (accessModifier == "Public" || accessModifier == "public")
                comboBox.SelectedIndex = 0;
            else if (accessModifier == "Private" || accessModifier == "private")
                comboBox.SelectedIndex = 1;
            else if (accessModifier == "Private Protected")
                comboBox.SelectedIndex = 2;
            else if (accessModifier == "Protected" || accessModifier == "protected")
                comboBox.SelectedIndex = 3;
            return comboBox;
        }

        private void CreateOperation(Operation operation)
        {
            RowDefinition rd = new RowDefinition
            {
                Height = new GridLength(30)
            };
            OperationsGrid.RowDefinitions.Add(rd);
            TextBox operationName = new TextBox
            {
                Text = operation.Name,
                FontSize = 14,
                Padding = new Thickness(0, 5, 0, 5),
                Foreground = Brushes.Black,
            };
            operationName.KeyDown += _controller.Operation_OnKeyDown;
            Grid.SetColumn(operationName, 0);
            Grid.SetRow(operationName, OperationsGrid.RowDefinitions.Count - 1);
            string parameterText = "";
            if (operation.Parameters != null)
            {
                foreach (Parameter parameter in operation.Parameters)
                {
                    parameterText += parameter.Name + ":" + parameter.DataType;
                    if (!string.IsNullOrEmpty(parameter.DefaultValue))
                        parameterText += "=" + parameter.DefaultValue;
                    parameterText += ", ";
                }

                parameterText = parameterText.Substring(0, parameterText.Length - 2);
            }

            TextBox operationParameter = new TextBox
            {
                Text = parameterText,
                FontSize = 14,
                Padding = new Thickness(0, 5, 0, 5),
                Foreground = Brushes.Black,
            };
            operationParameter.KeyDown += _controller.Operation_OnKeyDown;
            Grid.SetColumn(operationParameter, 1);
            Grid.SetRow(operationParameter, OperationsGrid.RowDefinitions.Count - 1);
            TextBox operationType = new TextBox
            {
                Text = operation.DataTypeOfReturnValue,
                FontSize = 14,
                Padding = new Thickness(0, 5, 0, 5),
                Foreground = Brushes.Black,
            };
            operationType.KeyDown += _controller.Operation_OnKeyDown;
            Grid.SetColumn(operationType, 2);
            Grid.SetRow(operationType, OperationsGrid.RowDefinitions.Count - 1);
            ComboBox operationAccess = CreateComboBox(operation.AccessModifier);
            operationAccess.DropDownClosed += _controller.Operation_ComboBoxSelected;
            Grid.SetColumn(operationAccess, 3);
            Grid.SetRow(operationAccess, OperationsGrid.RowDefinitions.Count - 1);
            OperationsGrid.Children.Add(operationName);
            OperationsGrid.Children.Add(operationParameter);
            OperationsGrid.Children.Add(operationType);
            OperationsGrid.Children.Add(operationAccess);
        }

        private void ShowProperties(object sender, EventArgs e)
        {
            OptionsGrid.Children.Clear();
            OptionsGrid.RowDefinitions.Clear();
            if (sender is ModelNodeElement element)
            {
                CreateProperty("Name", element.Name, true);
                CreateProperty("ID", element.Id, false);
                CreateProperty("Type", element.Type, false);
                CreateProperty("Stereotype", element.Stereotype, true);
            }
            else if (sender is ModelNodeLine line)
            {
                CreateProperty("Source", line.Source, false);
                CreateProperty("Target", line.Target, false);
                CreateProperty("Text", line.TextOnLine, true);
                CreateProperty("Text Source", line.TextSourceOnLine, true);
                CreateProperty("Text Target", line.TextTargetOnLine, true);
            }
        }

        private void CreateProperty(string propertyName, string propertyValue, bool isEditable)
        {
            RowDefinition rd = new RowDefinition
            {
                Height = GridLength.Auto
            };
            OptionsGrid.RowDefinitions.Add(rd);
            TextBlock propertyNameTb = new TextBlock
            {
                Text = propertyName,
                Height = 30,
                FontSize = 16,
                Padding = new Thickness(5, 0, 0, 0),
                Foreground = Brushes.Black
            };
            Border borderName = new Border
            {
                BorderBrush = Brushes.DarkGray,
                BorderThickness = new Thickness(0, 0, 1, 1)
            };
            Grid.SetColumn(borderName, 0);
            Grid.SetRow(propertyNameTb, OptionsGrid.RowDefinitions.Count - 1);
            Grid.SetRow(borderName, OptionsGrid.RowDefinitions.Count - 1);
            Grid.SetColumn(propertyNameTb, 0);
            TextBox propertyValueTb = new TextBox
            {
                Text = propertyValue,
                Height = 30,
                IsEnabled = isEditable,
                Padding = new Thickness(0, 0, 20, 0),
                FontSize = 16,
                BorderThickness = new Thickness(0),
                VerticalContentAlignment = VerticalAlignment.Center,
                Foreground = Brushes.Black,
            };
            propertyValueTb.KeyDown += _controller.Property_OnKeyDown;
            Border borderValue = new Border
            {
                BorderBrush = Brushes.DarkGray,
                BorderThickness = new Thickness(1, 0, 0, 1)
            };
            Grid.SetColumn(borderValue, 1);
            Grid.SetColumn(propertyValueTb, 1);
            Grid.SetRow(propertyValueTb, OptionsGrid.RowDefinitions.Count - 1);
            Grid.SetRow(borderValue, OptionsGrid.RowDefinitions.Count - 1);
            OptionsGrid.Children.Add(propertyNameTb);
            OptionsGrid.Children.Add(borderName);
            OptionsGrid.Children.Add(propertyValueTb);
            OptionsGrid.Children.Add(borderValue);
        }

        private void InitialDiagram(object sender, EventArgs e)
        {
            DrawCanvas.Children.Clear();
            Random random = new Random();
            List<ModelNodeBase> rootChildNodes = (sender as Model)?.Root.ChildNodes;
            if (rootChildNodes != null)
                foreach (ModelNodeBase element in rootChildNodes)
                {
                    switch (element.Type)
                    {
                        case "Class":
                            ClassElement classElement = new ClassElement(element as ModelNodeElement);
                            classElement.MouseLeftButtonDown += _controller.UpdateSelectedElement;
                            classElement.LayoutUpdated += _controller.UpdateSelectedElementSizeAndPosition;
                            DiagramNode nodeC = new DiagramNode
                            {
                                ModelElementId = element.Id,
                                X1 = random.NextDouble() * 400,
                                Y1 = random.NextDouble() * 400
                            };
                            Canvas.SetLeft(classElement, nodeC.X1);
                            Canvas.SetTop(classElement, nodeC.Y1);
                            nodeC.Height = classElement.ActualHeight;
                            nodeC.Width = classElement.ActualWidth;
                            _controller.SelectedDiagramElement = classElement;
                            _controller.SelectedModelElement = classElement.GetModelElement();
                            _controller.CurrentDiagram.Elements.Add(nodeC);
                            DrawCanvas.Children.Add(classElement);
                            break;
                        case "Association":
                            DiagramNode start =
                                _controller.CurrentDiagram.GetElement((element as ModelNodeLine)?.Source);
                            DiagramNode end =
                                _controller.CurrentDiagram.GetElement((element as ModelNodeLine)?.Target);
                            DiagramNode nodeA = new DiagramNode
                            {
                                ModelElementId = element.Id,
                                X1 = start.X1 + start.Width / 2,
                                Y1 = start.Y1 + start.Height / 2,
                                X2 = end.X1 + end.Width / 2,
                                Y2 = end.Y1 + end.Height / 2
                            };
                            Association association = new Association(element as ModelNodeLine)
                            {
                                X1 = nodeA.X1,
                                Y1 = nodeA.Y1,
                                X2 = nodeA.X2,
                                Y2 = nodeA.Y2
                            };
                            _controller.CurrentDiagram.Elements.Add(nodeA);
                            _controller.ElementMoveOrResized += association.MoveLine;
                            Canvas.SetLeft(association, 0);
                            Canvas.SetTop(association, 0);
                            DrawCanvas.Children.Insert(0, association);
                            break;
                    }
                }
        }

        private void UpdateCanvas(object sender, EventArgs eventArgs)
        {
            DrawCanvas.Children.Clear();
            foreach (DiagramNode element in _controller.CurrentDiagram.Elements)
            {
                ModelNodeBase modelElement = (sender as Model)?.GetNode(element.ModelElementId);
                switch (modelElement?.Type)
                {
                    case "Class":
                        ClassElement classElement = new ClassElement(modelElement as ModelNodeElement)
                        {
                            Width = element.Width,
                            Height = element.Height
                        };
                        classElement.MouseLeftButtonDown += _controller.UpdateSelectedElement;
                        classElement.LayoutUpdated += _controller.UpdateSelectedElementSizeAndPosition;
                        Canvas.SetLeft(classElement, element.X1);
                        Canvas.SetTop(classElement, element.Y1);
                        _controller.SelectedDiagramElement = classElement;
                        _controller.SelectedModelElement = classElement.GetModelElement();
                        classElement.RemovedElement += _controller.DeleteElementAndLinks;
                        DrawCanvas.Children.Add(classElement);
                        break;
                    case "Activity":
                        Activity activity = new Activity(modelElement as ModelNodeElement)
                        {
                            Width = element.Width,
                            Height = element.Height
                        };
                        activity.MouseLeftButtonDown += _controller.UpdateSelectedElement;
                        activity.LayoutUpdated += _controller.UpdateSelectedElementSizeAndPosition;
                        Canvas.SetLeft(activity, element.X1);
                        Canvas.SetTop(activity, element.Y1);
                        _controller.SelectedDiagramElement = activity;
                        _controller.SelectedModelElement = activity.GetModelElement();
                        DrawCanvas.Children.Add(activity);
                        break;
                    case "Association":
                        Association association = new Association(modelElement as ModelNodeLine)
                        {
                            X1 = element.X1,
                            Y1 = element.Y1,
                            X2 = element.X2,
                            Y2 = element.Y2
                        };
                        _controller.ElementMoveOrResized += association.MoveLine;
                        Canvas.SetLeft(association, 0);
                        Canvas.SetTop(association, 0);
                        DrawCanvas.Children.Insert(0, association);
                        break;
                }
            }
        }

        private void RecreateView(object sender, EventArgs e)
        {
            TreeView.Items.Clear();
            DrawCanvas.Children.Clear();
        }

        private void NewDiagram(object sender, EventArgs e) { }

        private void DrawTree(object sender, EventArgs e)
        {
            TreeView.Items.Clear();
            TreeViewItem root = GetTreeViewItem(
                (sender as Model)?.Root.Id,
                (sender as Model)?.Root.Namespace.PackageName,
                "Public");

            root.IsExpanded = true;

            List<ModelNodeBase> rootChildNodes = (sender as Model)?.Root.ChildNodes;
            if (rootChildNodes != null)
                foreach (ModelNodeBase child in rootChildNodes)
                {
                    if (child.GetType().ToString() != "UMLRedactor.Models.ModelNodeElement")
                        continue;

                    TreeViewItem item = GetElement(child);

                    List<ModelNodeBase> childChildren = child.ChildNodes;
                    if (childChildren != null)
                    {
                        foreach (ModelNodeBase cc in childChildren)
                        {
                            if (cc.GetType().ToString() != "UMLRedactor.Models.ModelNodeElement")
                                continue;
                            item.Items.Add(GetElement(cc));
                        }
                    }

                    root.Items.Add(item);
                }

            TreeView.Items.Add(root);
        }

        private TreeViewItem GetElement(ModelNodeBase element)
        {
            TreeViewItem item;
            if (!string.IsNullOrEmpty((element as ModelNodeElement)?.Stereotype))
                item = GetTreeViewItem(element.Id,
                    "«" + ((ModelNodeElement)element).Stereotype + "» " + element.Name,
                    "Public");
            else
                item = GetTreeViewItem(element.Id, element.Name, "Public");

            List<Attribute> attributes = (element as ModelNodeElement)?.Attributes;
            if (attributes != null)
                foreach (Attribute attribute in attributes)
                {
                    string text = attribute.Name + ": " + attribute.DataType;
                    item.Items.Add(GetTreeViewItem(Guid.NewGuid().ToString(), text, attribute.AccessModifier));
                }

            List<Operation> operations = (element as ModelNodeElement)?.Operations;
            if (operations != null)
                foreach (Operation operation in operations)
                {
                    string text = operation.Name + "(";
                    foreach (Parameter pr in operation.Parameters)
                    {
                        text += pr.DataType + ", ";
                    }

                    if (text[text.Length - 2] == ',')
                    {
                        text = text.Substring(0, text.Length - 2);
                        text += ")";
                    }
                    else
                        text += ")";

                    if (operation.DataTypeOfReturnValue != "void")
                        text += ":" + operation.DataTypeOfReturnValue;

                    item.Items.Add(GetTreeViewItem(Guid.NewGuid().ToString(), text,
                        operation.AccessModifier));
                }

            item.Selected += _controller.treeItem_Selected;
            return item;
        }

        private TreeViewItem GetTreeViewItem(string uid, string text, string imagePath)
        {
            TreeViewItem item = new TreeViewItem
            {
                Uid = uid,
                IsExpanded = false
            };

            StackPanel stack = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            Image image = new Image
            {
                Source = new BitmapImage
                    (new Uri(@"/UMLRedactor;component/Icons/" + imagePath + ".png", UriKind.Relative)),
                Width = 16,
                Height = 16
            };
            Label lbl = new Label
            {
                Content = text
            };

            stack.Children.Add(image);
            stack.Children.Add(lbl);

            item.Header = stack;
            return item;
        }

        private void SystemButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = ((Border)sender)?.Name == "SystemButtonClose"
                ? Brushes.Red
                : Brushes.LightBlue;
        }

        private void SystemButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = Brushes.LightGray;
        }

        private void SystemButtonTray_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainView.WindowState = WindowState.Minimized;
        }

        private void SystemButtonMaximize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainView.WindowState =
                MainView.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void SystemButtonClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _controller.CloseApplication();
        }

        private void TitleBar_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            if (e.ClickCount == 2)
                MainView.WindowState = MainView.WindowState == WindowState.Normal
                    ? WindowState.Maximized
                    : WindowState.Normal;
        }
    }
}