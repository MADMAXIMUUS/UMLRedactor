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
            _controller.EndModelRead += DrawTree;
            _controller.NewModel += RecreateView;
        }

        private void RecreateView(object sender, EventArgs e)
        {
            TreeView.Items.Clear();
            DrawCanvas.Children.Clear();
        }

        private void DrawTree(object sender, EventArgs e)
        {
            TreeView.Items.Clear();
            TreeViewItem root = new TreeViewItem
            {
                Header = (sender as Model)?.Root.Namespace.PackageName,
                Uid = (sender as Model)?.Root.Id,
                IsExpanded = true
            };
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
            TreeViewItem item = new TreeViewItem
            {
                Uid = element.Id,
                IsExpanded = false
            };
            if (!string.IsNullOrEmpty((element as ModelNodeElement)?.Stereotype))
                item.Header = "«" + ((ModelNodeElement)element).Stereotype + "» " + element.Name;
            else
                item.Header = element.Name;

            List<Additions.Attribute> attributes = (element as ModelNodeElement)?.Attributes;
            if (attributes != null)
            {
                foreach (Additions.Attribute attribute in attributes)
                {
                    string text = attribute.Name + ": " + attribute.DataType;
                    item.Items.Add(GetTreeViewItem(Guid.NewGuid().ToString(), text, attribute.AccessModifier));
                }
            }

            List<Operation> operations = (element as ModelNodeElement)?.Operations;
            if (operations != null)
            {
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
            }

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
            Application.Current.Shutdown();
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