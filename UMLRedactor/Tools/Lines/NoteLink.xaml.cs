using System.Windows.Controls;
using UMLRedactor.Models;

namespace UMLRedactor.Tools.Lines
{
    /// <summary>
    /// Interaction logic for NoteLink.xaml
    /// </summary>
    public partial class NoteLink : UserControl, ILine
    {
        public NoteLink()
        {
            InitializeComponent();
        }

        public ModelNodeLine GetModelElement()
        {
            return new ModelNodeLine();
        }
    }
}