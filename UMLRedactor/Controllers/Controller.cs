using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using UMLRedactor.Models;
using UMLRedactor.Tools.Elements.ClassDiagram;
using UMLRedactor.View;

namespace UMLRedactor.Controllers
{
    public class Controller
    {
        private Model _model;
        private List<Diagram> _diagrams;
        public Diagram CurrentDiagram;
        private string _filePath = ""; //AppDomain.CurrentDomain.BaseDirectory+"\\Saves";
        private Random rand = new Random();
        
        public event EventHandler EndModelRead;
        public event EventHandler NewModel;

        public event EventHandler UpdateDiagram;

        public Controller(Model model)
        {
            _model = model;
            CurrentDiagram = new Diagram();
            _diagrams = new List<Diagram>();
        }

        public void OpenFile(object sender, RoutedEventArgs e)
        {
            if (_model.Name == "")
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
                            UpdateTreeView();
                            break;
                        case -1:
                            MessageBox.Show("Версия XMI не соответсвует 1.1!");
                            break;
                        case -2:
                            MessageBox.Show("Ошибка импортирования");
                            break;
                        default:
                            MessageBox.Show("Ошибка импортирования");
                            break;
                    }
                }
            }
            else
            {
                SaveFile(null, null);
            }
        }

        private void UpdateTreeView()
        {
            EndModelRead?.Invoke(_model, EventArgs.Empty);
        }

        public void NewFile(object sender, RoutedEventArgs e)
        {
            if (_model.Name == "")
            {
                AskName();
                CreateNewModel();
                UpdateTreeView();
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы не сохранили изменения! Сохранить?",
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
            _model.Root.Namespace.PackageName=dialog.EnteredName.Text;
        }

        private void CreateNewModel()
        {
            NewModel?.Invoke(_model, EventArgs.Empty);
        }

        public void SaveFile(object sender, RoutedEventArgs e)
        {
            if (_filePath == "")
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
                FileName = "Введите название файла"
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
                    node.Width = 200;
                    node.Height = 150;
                    node.X1 = rand.NextDouble()*500;
                    node.Y1 = rand.NextDouble()*500;
                    CurrentDiagram.Elements.Add(node);
                    UpdateDiagram?.Invoke(CurrentDiagram, EventArgs.Empty);
                    break;
            }
        }

        public void Export(object sender, RoutedEventArgs e)
        {
        }

        public void Redo(object sender, RoutedEventArgs e)
        {
        }

        public void Undo(object sender, RoutedEventArgs e)
        {
        }

        public void NewDiagram(object sender, RoutedEventArgs e)
        {
        }

        public void CloseDiagram(object sender, RoutedEventArgs e)
        {
        }

        public void NextDiagram(object sender, RoutedEventArgs e)
        {
        }

        public void PrevDiagram(object sender, RoutedEventArgs e)
        {
        }

        public void OpenDiagram(object sender, RoutedEventArgs e)
        {
        }

        public void SaveDiagram(object sender, RoutedEventArgs e)
        {
        }

        /*private void ResizeAndTranslate(MouseEventArgs e)
        {
            if (!_isSizing) return;
            if (_borderEdge < 0) return;
            if (_selectedElement == null) return;

            if (e.LeftButton != MouseButtonState.Pressed)
            {
                _isSizing = false;
            }
            else
            {
                Point cursorPosition = e.GetPosition(_view.DrawCanvas);
                Point position = new Point(Canvas.GetLeft(_selectedElement), Canvas.GetTop(_selectedElement));
                double diffX = position.X - cursorPosition.X;
                double diffY = position.Y - cursorPosition.Y;
                if (_borderEdge == (int)Enums.EdgeTypes.MiddleTop)
                {
                    double newLeft = cursorPosition.X - _sizingOffsetX;
                    double newTop = cursorPosition.Y - _sizingOffsetY;
                    if (newLeft > 0) Canvas.SetLeft(_selectedElement, newLeft);
                    if (newTop > 0) Canvas.SetTop(_selectedElement, newTop);
                    _selectedElement.UpdateLayout();
                }
                else
                {
                    if (_borderEdge == (int)Enums.EdgeTypes.LeftTop || _borderEdge == (int)Enums.EdgeTypes.LeftBottom)
                    {
                        double newLeft = cursorPosition.X;
                        double newWidth = _selectedElement.Width + diffX;
                        if (newLeft > 0 && newWidth > _selectedElement.MinWidth)
                            Canvas.SetLeft(_selectedElement, newLeft);
                        if (newWidth > _selectedElement.MinWidth) _selectedElement.Width = newWidth;
                        _selectedElement.UpdateLayout();
                    }

                    if (_borderEdge == (int)Enums.EdgeTypes.LeftTop || _borderEdge == (int)Enums.EdgeTypes.RightTop)
                    {
                        double newTop = cursorPosition.Y;
                        double newHeight = _selectedElement.Height + diffY;
                        if (newTop > 0 && newHeight > _selectedElement.MinHeight)
                            Canvas.SetTop(_selectedElement, newTop);
                        if (newHeight > _selectedElement.MinHeight) _selectedElement.Height = newHeight;
                        _selectedElement.UpdateLayout();
                    }

                    if (_borderEdge == (int)Enums.EdgeTypes.RightTop || _borderEdge == (int)Enums.EdgeTypes.RightBottom)
                    {
                        double newWidth = cursorPosition.X + position.X;
                        if (newWidth > _selectedElement.MinWidth) _selectedElement.Width = newWidth;
                        _selectedElement.UpdateLayout();
                    }

                    if (_borderEdge == (int)Enums.EdgeTypes.LeftBottom ||
                        _borderEdge == (int)Enums.EdgeTypes.RightBottom)
                    {
                        double newHeight = cursorPosition.Y + position.Y;
                        if (newHeight > _selectedElement.MinHeight) _selectedElement.Height = newHeight;
                        _selectedElement.UpdateLayout();
                    }
                }
            }
        }*/

        public void CloseApplication()
        {
            if (_model.Name == "")
                Application.Current.Shutdown();
            else
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы не сохранили изменения! Сохранить?",
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
    }
}