using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using UMLRedactor.Additions;
using UMLRedactor.Models;
using UMLRedactor.View;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using TextBox = System.Windows.Forms.TextBox;

namespace UMLRedactor.Controllers
{
    public class Controller
    {
        private Model _model;
        private List<Diagram> _diagrams;
        public Diagram CurrentDiagram;
        private int _currentDiagramIndex;
        private string _filePath = "";
        private readonly Random _rand;
        public ModelNodeBase SelectedElement;
        private readonly Dictionary<string, int> _elementsCounter;

        public event EventHandler EndModelRead;
        public event EventHandler NewModel;
        public event EventHandler ElementCreated;

        public event EventHandler TreeViewItemSelected;

        public Controller(Model model)
        {
            _model = model;
            CurrentDiagram = new Diagram();
            _elementsCounter = new Dictionary<string, int>();
            _currentDiagramIndex = 0;
            _rand = new Random();
            _diagrams = new List<Diagram> { CurrentDiagram };
            InitialView();
        }

        public void treeItem_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item != null) 
                TreeViewItemSelected.Invoke(_model.GetNode(item.Uid.ToString()), EventArgs.Empty);
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

        private void InitialView()
        {
            EndModelRead?.Invoke(_model, EventArgs.Empty);
        }

        private void UpdateView()
        {
            ElementCreated?.Invoke(_model, EventArgs.Empty);
        }

        public void NewFile(object sender, RoutedEventArgs e)
        {
            if (_model.Name == "")
            {
                AskName();
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
                    AskName();
                    CreateNewModel();
                }
                else if (result == MessageBoxResult.No)
                {
                    _model = new Model();
                    AskName();
                    CreateNewModel();
                }
            }
        }

        private void AskName()
        {
            EnterDialog dialog = new EnterDialog("Введите название модели", "Название");
            dialog.ShowDialog();
            _model = new Model();
            _model.Root.Namespace.PackageName = dialog.EnteredName.Text;
        }

        private void CreateNewModel()
        {
            NewModel?.Invoke(_model, EventArgs.Empty);
        }

        public void SaveFile(object sender, RoutedEventArgs e)
        {
            if (_filePath != "")
            {
                ModelWriter writer = new ModelWriter();
                writer.SaveToXml(_model, Path.GetFullPath(_filePath));
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
                FileName = _model.Name
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                if (saveFileDialog.FileName != "Введите название файла")
                    _filePath = Path.GetFullPath(saveFileDialog.FileName);
                ModelWriter writer = new ModelWriter();
                writer.SaveToXml(_model, Path.GetFullPath(saveFileDialog.FileName));
            }
        }

        public void CreateElement(object sender, RoutedEventArgs e)
        {
            DiagramNode node = new DiagramNode
            {
                Id = Guid.NewGuid().ToString()
            };
            switch ((sender as Button)?.Name)
            {
                case "Class":
                    int counter = 1;
                    if (_elementsCounter.ContainsKey("Class"))
                        counter = _elementsCounter["Class"];
                    else
                        _elementsCounter["Class"] = counter;
                    ModelNodeElement classElement = new ModelNodeElement
                    {
                        Name = "Class" + counter,
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
                    node.X1 = _rand.NextDouble() * 300;
                    node.Y1 = _rand.NextDouble() * 300;
                    node.ModelElementId = classElement.Id;
                    CurrentDiagram.Elements.Add(node);
                    UpdateView();
                    break;
            }
        }
        
        

        public void Export(object sender, RoutedEventArgs e) { }

        public void Redo(object sender, RoutedEventArgs e) { }

        public void Undo(object sender, RoutedEventArgs e) { }

        public void NewDiagram(object sender, RoutedEventArgs e) { }

        public void CloseDiagram(object sender, RoutedEventArgs e) { }

        public void NextDiagram(object sender, RoutedEventArgs e) { }

        public void PrevDiagram(object sender, RoutedEventArgs e) { }

        public void OpenDiagram(object sender, RoutedEventArgs e) { }

        public void SaveDiagram(object sender, RoutedEventArgs e) { }

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
            
        }

        public void Attribute_ComboBoxSelected(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}