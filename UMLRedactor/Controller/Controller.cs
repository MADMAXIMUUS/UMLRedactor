﻿using Microsoft.Win32;
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
        private DomModel _model;

        public Controller(DomModel model)
        {
            //_view = view;
            _model = model;
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
                DomParser parser = new DomParser(openFileDialog.FileName);
                _model = parser.GetModelFromMadFile();
            }

        }

        private void ResizeAndTranslate(MouseEventArgs e)
        {
            /*if (!_isSizing) return;
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
            }*/
        }
    }
}