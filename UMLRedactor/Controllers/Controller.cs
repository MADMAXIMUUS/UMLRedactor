using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using UMLRedactor.Additions;
using UMLRedactor.Models;
using UMLRedactor.Tools.Elements;
using UMLRedactor.Tools.Lines;
using UMLRedactor.View;
using Attribute = UMLRedactor.Additions.Attribute;

namespace UMLRedactor.Controllers
{
    public class Controller
    {
        private Model _model;
        private readonly List<Diagram> _diagrams;
        public Diagram CurrentDiagram;
        private readonly List<Diagram> _previewsDiagrams;
        private int _currentDiagramIndex;
        private int _previewsDiagramIndex;
        private string _modelFilePath = "";
        private string _currentDiagramFilePath = "";
        private readonly Random _rand;
        private bool _isDrawLine;
        private string _lineType;
        private ModelNodeElement _selectedSource;
        private ModelNodeElement _selectedTarget;
        public ModelNodeBase SelectedModelElement;
        public UIElement SelectedDiagramElement;
        private readonly Dictionary<string, int> _elementsCounter;

        public event EventHandler EndModelRead;
        public event EventHandler NewModel;
        public event EventHandler NewDiagramCreated;
        public event EventHandler ElementCreated;
        public event EventHandler TreeViewItemSelected;
        public event EventHandler DiagramElementSelected;
        public event EventHandler AttributeChanged;
        public event EventHandler OperationChanged;
        public event EventHandler SourceAndTargetSelected;
        public event EventHandler<MoveLineEventArgs> ElementMoveOrResized;

        public Controller(Model model)
        {
            _model = model;
            CurrentDiagram = new Diagram();
            _elementsCounter = new Dictionary<string, int>();
            _currentDiagramIndex = 0;
            _previewsDiagramIndex = 0;
            _rand = new Random();
            _diagrams = new List<Diagram>();
            _previewsDiagrams = new List<Diagram>(10);
            _selectedSource = null;
            _selectedTarget = null;
            SourceAndTargetSelected += CreateLine;
            InitialView();
        }

        public void treeItem_Selected(object sender, RoutedEventArgs e)
        {
            if (sender is TreeViewItem item)
                TreeViewItemSelected?.Invoke
                (
                    _model.GetNode(item.Uid),
                    EventArgs.Empty
                );
        }

        public void UpdateSelectedElement(object sender, MouseEventArgs e)
        {
            if (sender is IElement element)
            {
                if (_isDrawLine)
                {
                    if (_selectedSource == null)
                        _selectedSource = element.GetModelElement();
                    else
                    {
                        _selectedTarget = element.GetModelElement();
                        SourceAndTargetSelected?.Invoke(null, null);
                    }
                }
                else
                    SelectedModelElement = element.GetModelElement();
            }
            else if (sender is ILine line)
                SelectedModelElement = line.GetModelElement();

            if (!_isDrawLine)
            {
                SelectedDiagramElement = sender as UIElement;
                DiagramElementSelected?.Invoke(SelectedModelElement, EventArgs.Empty);
            }
        }

        public void UpdateSelectedElementSizeAndPosition(object sender, EventArgs eventArgs)
        {
            double x = Canvas.GetLeft(SelectedDiagramElement);
            double y = Canvas.GetTop(SelectedDiagramElement);
            ElementMoveOrResized?.Invoke(SelectedModelElement,
                new MoveLineEventArgs(x, y, ((UserControl)SelectedDiagramElement).ActualWidth,
                    ((UserControl)SelectedDiagramElement).ActualHeight));
            CurrentDiagram.UpdateElementPosition(x, y, SelectedModelElement.Id);
            CurrentDiagram.UpdateElementSize(((UserControl)SelectedDiagramElement).ActualWidth,
                ((UserControl)SelectedDiagramElement).ActualHeight, SelectedModelElement.Id);
            UpdateLinePosition(x, y);
        }

        private void UpdateLinePosition(double x, double y)
        {
            List<ModelNodeLine> connections = _model.GetConnection(SelectedModelElement.Id);
            foreach (ModelNodeLine connection in connections)
            {
                DiagramNode node = CurrentDiagram.GetElement(connection.Id);
                if (connection.Source == SelectedModelElement.Id)
                    CurrentDiagram.UpdateLinePosition(x + ((UserControl)SelectedDiagramElement).ActualWidth / 2,
                        y + ((UserControl)SelectedDiagramElement).ActualHeight / 2, node.X2, node.Y2, connection.Id);
                else
                    CurrentDiagram.UpdateLinePosition(node.X1, node.Y1,
                        x + ((UserControl)SelectedDiagramElement).ActualWidth / 2,
                        y + ((UserControl)SelectedDiagramElement).ActualHeight / 2, connection.Id);
            }
        }

        private void InitialView()
        {
            EndModelRead?.Invoke(_model, EventArgs.Empty);
        }

        private void UpdateView()
        {
            ElementCreated?.Invoke(_model, EventArgs.Empty);
        }

        private string AskName(string title)
        {
            EnterDialog dialog = new EnterDialog(title, "Название");
            dialog.ShowDialog();
            return dialog.EnteredName.Text;
        }

        private void CreateNewModel()
        {
            _modelFilePath = "";
            NewModel?.Invoke(_model, EventArgs.Empty);
        }

        private void CreateNewDiagram()
        {
            _currentDiagramFilePath = "";
            NewDiagramCreated?.Invoke(null, EventArgs.Empty);
        }

        public void NewFile(object sender, RoutedEventArgs e)
        {
            if (_model.Name == "")
            {
                _model = new Model();
                _model.Root.Namespace.PackageName = AskName("Введите название модели");
                CreateNewModel();
                InitialView();
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                    "Несохраненные изменения будут утерены! Сохранить?",
                    "Внимание!",
                    MessageBoxButton.YesNoCancel
                );
                if (result == MessageBoxResult.Yes)
                {
                    SaveFile(null, null);
                    _model = new Model();
                    _model.Root.Namespace.PackageName = AskName("Введите название модели");
                    CreateNewModel();
                }
                else if (result == MessageBoxResult.No)
                {
                    _model = new Model();
                    _model.Root.Namespace.PackageName = AskName("Введите название модели");
                    CreateNewModel();
                }
            }
        }

        public void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Импорт модели",
                Filter = "XMI (*xml)|*.xml",
                FileName = "Выберите файл"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                ModelReader reader = new ModelReader(openFileDialog.FileName);
                switch (reader.GetModelFromFile(out _model))
                {
                    case 0:
                        InitialView();
                        break;
                    case -1:
                        MessageBox.Show("Версия XMI не соответсвует 1.1!", "Ошибка");
                        break;
                    case -2:
                        MessageBox.Show("Ошибка импортирования", "Ошибка");
                        break;
                    default:
                        MessageBox.Show("Ошибка импортирования", "Ошибка");
                        break;
                }
            }
        }

        public void SaveFile(object sender, RoutedEventArgs e)
        {
            if (_modelFilePath != "")
            {
                ModelWriter writer = new ModelWriter();
                writer.SaveToXml(_model, Path.GetFullPath(_modelFilePath));
            }
            else
            {
                SaveAs(null, null);
            }
        }

        public void SaveAs(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Экспорт модели",
                Filter = "XMI (*xml)|*.xml",
                FileName = "Введите название файла"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                if (saveFileDialog.FileName != "Введите название файла")
                {
                    _modelFilePath = Path.GetFullPath(saveFileDialog.FileName);
                    ModelWriter writer = new ModelWriter();
                    writer.SaveToXml(_model, Path.GetFullPath(saveFileDialog.FileName));
                }
                else
                {
                    MessageBox.Show("Некорректное имя файла");
                }
            }
        }

        public void CreateElement(object sender, RoutedEventArgs e)
        {
            DiagramNode node = new DiagramNode
            {
                Id = Guid.NewGuid().ToString()
            };
            _isDrawLine = false;
            _lineType = "";
            switch ((sender as Button)?.Name)
            {
                case "Class":
                    int counterClass = 1;
                    if (_elementsCounter.ContainsKey("Class"))
                        counterClass = _elementsCounter["Class"];
                    else
                        _elementsCounter["Class"] = counterClass;
                    ModelNodeElement classElement = new ModelNodeElement
                    {
                        Name = "Class" + counterClass,
                        Id = Guid.NewGuid().ToString(),
                        Type = "Class",
                        Namespace = new Package
                        {
                            PackageId = _model.Root.Id,
                            PackageName = _model.Root.Name
                        }
                    };
                    _elementsCounter["Class"] += 1;
                    _model.AddElement(classElement.Namespace.PackageId, classElement);
                    node.Width = 200;
                    node.Height = 150;
                    node.X1 = _rand.NextDouble() * 400;
                    node.Y1 = _rand.NextDouble() * 400;
                    node.ModelElementId = classElement.Id;
                    CurrentDiagram.Elements.Add(node);
                    UpdateView();
                    break;
                case "Activity":
                    int counterActivity = 1;
                    if (_elementsCounter.ContainsKey("Activity"))
                        counterActivity = _elementsCounter["Activity"];
                    else
                        _elementsCounter["Activity"] = counterActivity;
                    ModelNodeElement activityElement = new ModelNodeElement
                    {
                        Name = "Activity" + counterActivity,
                        Id = Guid.NewGuid().ToString(),
                        Type = "Activity",
                        Namespace = new Package
                        {
                            PackageId = _model.Root.Id,
                            PackageName = _model.Root.Name
                        }
                    };
                    _elementsCounter["Activity"] += 1;
                    _model.AddElement(activityElement.Namespace.PackageId, activityElement);
                    node.Width = 150;
                    node.Height = 100;
                    node.X1 = _rand.NextDouble() * 400;
                    node.Y1 = _rand.NextDouble() * 400;
                    node.ModelElementId = activityElement.Id;
                    CurrentDiagram.Elements.Add(node);
                    UpdateView();
                    break;
                case "Association":
                    _isDrawLine = true;
                    _lineType = "Association";
                    break;
            }
        }

        public void UnselectedLine(object sender, RoutedEventArgs routedEventArgs)
        {
            _isDrawLine = false;
            _lineType = "";
        }

        private void CreateLine(object sender, EventArgs eventArgs)
        {
            DiagramNode node = new DiagramNode
            {
                Id = Guid.NewGuid().ToString()
            };
            ModelNodeLine line = new ModelNodeLine()
            {
                Id = Guid.NewGuid().ToString(),
                Name = _lineType,
                Source = _selectedSource.Id,
                Target = _selectedTarget.Id,
                Namespace = _selectedSource.Namespace,
                Type = _lineType
            };
            _model.AddElement(line.Namespace.PackageId, line);
            DiagramNode source = CurrentDiagram.GetElement(_selectedSource.Id);
            DiagramNode target = CurrentDiagram.GetElement(_selectedTarget.Id);
            node.X1 = source.X1 + source.Width / 2;
            node.Y1 = source.Y1 + source.Height / 2;
            node.X2 = target.X1 + target.Width / 2;
            node.Y2 = target.Y1 + target.Height / 2;
            node.ModelElementId = line.Id;
            CurrentDiagram.Elements.Add(node);
            _selectedSource = null;
            _selectedTarget = null;
            UpdateView();
        }

        public void Export(object sender, RoutedEventArgs e) { }

        public void Redo(object sender, RoutedEventArgs e) { }

        public void Undo(object sender, RoutedEventArgs e) { }

        public void NewDiagram(object sender, RoutedEventArgs e)
        {
            if (CurrentDiagram.Name == "")
            {
                CurrentDiagram = new Diagram
                {
                    Name = AskName("Введите название диаграммы")
                };
                _diagrams.Add(CurrentDiagram);
                _currentDiagramIndex = _diagrams.Count - 1;
                CreateNewDiagram();
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                    "Несохраненные изменения будут утерены! Сохранить?",
                    "Внимание!",
                    MessageBoxButton.YesNoCancel
                );
                if (result == MessageBoxResult.Yes)
                {
                    SaveDiagram(null, null);
                    CreateNewDiagram();
                }
                else if (result == MessageBoxResult.No)
                {
                    CurrentDiagram = new Diagram();
                    CreateNewDiagram();
                }
            }
        }

        public void CloseDiagram(object sender, RoutedEventArgs e) { }

        public void NextDiagram(object sender, RoutedEventArgs e) { }

        public void PrevDiagram(object sender, RoutedEventArgs e) { }

        public void OpenDiagram(object sender, RoutedEventArgs e) { }

        public void SaveDiagram(object sender, RoutedEventArgs e)
        {
            if (_currentDiagramFilePath != "")
            {
                DiagramWriter diagramWriter = new DiagramWriter();
                diagramWriter.SaveToXml(CurrentDiagram, _currentDiagramFilePath, _modelFilePath);
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = "Экспорт диаграммы",
                    Filter = "XMI (*xml)|*.xml",
                    FileName = "Введите название файла"
                };
                if (saveFileDialog.ShowDialog() == true)
                {
                    if (saveFileDialog.FileName != "Введите название файла")
                    {
                        _currentDiagramFilePath = Path.GetFullPath(saveFileDialog.FileName);
                        DiagramWriter writer = new DiagramWriter();
                        writer.SaveToXml(CurrentDiagram, Path.GetFullPath(saveFileDialog.FileName), _modelFilePath);
                    }
                    else
                    {
                        MessageBox.Show("Некорректное имя файла");
                    }
                }
            }
        }

        public void CloseApplication()
        {
            if (_model.Name == "")
                Application.Current.Shutdown();
            else
            {
                MessageBoxResult result = MessageBox.Show(
                    "Несохраненные изменения будут утерены! Сохранить?",
                    "Внимание!",
                    MessageBoxButton.YesNoCancel
                );
                if (result == MessageBoxResult.Yes)
                {
                    SaveFile(null, null);
                    Application.Current.Shutdown();
                }
                else if (result == MessageBoxResult.No)
                {
                    Application.Current.Shutdown();
                }
            }
        }

        public void Attribute_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                List<Attribute> attributes = new List<Attribute>();
                if ((sender as TextBox)?.Parent is Grid gr)
                    for (int i = 0; i < gr.Children.Count; i += 3)
                    {
                        Attribute attribute = new Attribute
                        {
                            Name = (gr.Children[i] as TextBox)?.Text,
                            DataType = (gr.Children[i + 1] as TextBox)?.Text,
                            AccessModifier = (gr.Children[i + 2] as ComboBox)?.Text
                        };
                        attributes.Add(attribute);
                    }

                _model.UpdateAttributes(SelectedModelElement.Id, attributes);
                AttributeChanged?.Invoke(_model, null);
            }
        }

        public void Attribute_ComboBoxSelected(object sender, EventArgs eventArgs)
        {
            List<Attribute> attributes = new List<Attribute>();
            if ((sender as ComboBox)?.Parent is Grid gr)
                for (int i = 0; i < gr.Children.Count; i += 3)
                {
                    Attribute attribute = new Attribute
                    {
                        Name = (gr.Children[i] as TextBox)?.Text,
                        DataType = (gr.Children[i + 1] as TextBox)?.Text,
                        AccessModifier = (gr.Children[i + 2] as ComboBox)?.Text
                    };
                    attributes.Add(attribute);
                }

            _model.UpdateAttributes(SelectedModelElement.Id, attributes);
            AttributeChanged?.Invoke(_model, null);
        }

        public void Operation_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                List<Operation> operations = new List<Operation>();
                if ((sender as TextBox)?.Parent is Grid gr)
                    for (int i = 0; i < gr.Children.Count; i += 4)
                    {
                        Operation operation = new Operation
                        {
                            Name = (gr.Children[i] as TextBox)?.Text,
                            DataTypeOfReturnValue = (gr.Children[i + 2] as TextBox)?.Text,
                            AccessModifier = (gr.Children[i + 3] as ComboBox)?.Text,
                            Parameters = new List<Parameter>()
                        };
                        string[] parameters = (gr.Children[i + 1] as TextBox)?.Text.Split(new[] { ',', ' ' },
                            StringSplitOptions.RemoveEmptyEntries);
                        if (parameters != null)
                            foreach (string parameterChild in parameters)
                            {
                                Parameter parameter = new Parameter
                                {
                                    Name = parameterChild.Substring(0,
                                        parameterChild.IndexOf(":", StringComparison.Ordinal))
                                };
                                if (parameterChild.IndexOf("=", StringComparison.Ordinal) > 0)
                                {
                                    parameter.DataType = parameterChild.Substring(
                                        parameterChild.IndexOf(":", StringComparison.Ordinal) + 1,
                                        parameterChild.IndexOf("=", StringComparison.Ordinal) -
                                        parameterChild.IndexOf(":", StringComparison.Ordinal) - 1);
                                    parameter.DefaultValue = parameterChild
                                        .Substring(parameterChild.IndexOf("=", StringComparison.Ordinal) + 1);
                                }
                                else
                                    parameter.DataType = parameterChild.Substring(
                                        parameterChild.IndexOf(":", StringComparison.Ordinal) + 1);

                                operation.Parameters.Add(parameter);
                            }

                        operations.Add(operation);
                    }

                _model.UpdateOperation(SelectedModelElement.Id, operations);
                OperationChanged?.Invoke(_model, null);
            }
        }

        public void Operation_ComboBoxSelected(object sender, EventArgs eventArgs)
        {
            List<Operation> operations = new List<Operation>();
            if ((sender as TextBox)?.Parent is Grid gr)
                for (int i = 0; i < gr.Children.Count; i += 4)
                {
                    Operation operation = new Operation
                    {
                        Name = (gr.Children[i] as TextBox)?.Text,
                        DataTypeOfReturnValue = (gr.Children[i + 2] as TextBox)?.Text,
                        AccessModifier = (gr.Children[i + 3] as ComboBox)?.Text,
                        Parameters = new List<Parameter>()
                    };
                    string[] parameters = (gr.Children[i + 1] as TextBox)?.Text.Split(new[] { ',', ' ' },
                        StringSplitOptions.RemoveEmptyEntries);
                    if (parameters != null)
                        foreach (string parameterChild in parameters)
                        {
                            Parameter parameter = new Parameter
                            {
                                Name = parameterChild.Substring(0,
                                    parameterChild.IndexOf(":", StringComparison.Ordinal))
                            };
                            if (parameterChild.IndexOf("=", StringComparison.Ordinal) > 0)
                            {
                                parameter.DataType = parameterChild.Substring(
                                    parameterChild.IndexOf(":", StringComparison.Ordinal) + 1,
                                    parameterChild.IndexOf("=", StringComparison.Ordinal) -
                                    parameterChild.IndexOf(":", StringComparison.Ordinal) - 1);
                                parameter.DefaultValue = parameterChild
                                    .Substring(parameterChild.IndexOf("=", StringComparison.Ordinal) + 1);
                            }
                            else
                                parameter.DataType = parameterChild.Substring(
                                    parameterChild.IndexOf(":", StringComparison.Ordinal) + 1);

                            operation.Parameters.Add(parameter);
                        }

                    operations.Add(operation);
                }

            _model.UpdateOperation(SelectedModelElement.Id, operations);
            OperationChanged?.Invoke(_model, null);
        }
    }
}