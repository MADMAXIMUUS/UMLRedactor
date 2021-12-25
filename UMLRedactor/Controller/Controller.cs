using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UMLRedactor.Additions;
using UMLRedactor.Models;
using UMLRedactor.View;

namespace UMLRedactor.Controller
{
    public class Controller
    {
        private MainWindow _view;
        private DomModel model;
        private bool _isSizing;
        private int _borderEdge;
        private double _sizingOffsetX;
        private double _sizingOffsetY;
        private UserControl _selectedElement;

        public Controller(MainWindow view)
        {
            _view = view;
            model = new DomModel();
        }

        public void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() 
            {
                Title = "Импорт модели",
                Filter = "Модель (*xml)|*.xml",
                FileName = "Выберите файл"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                DomParser parser = new DomParser(openFileDialog.FileName);
                model = parser.GetModelFromMadFile();
            }

        }

        private void ResizeAndTranslate(MouseEventArgs e)
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
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isSizing) ResizeAndTranslate(e);
        }

        public void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isSizing)
            {
                _isSizing = false;
                _borderEdge = -1;
                _sizingOffsetX = 0;
                _sizingOffsetY = 0;
                _selectedElement = null;
            }
        }
    }
}