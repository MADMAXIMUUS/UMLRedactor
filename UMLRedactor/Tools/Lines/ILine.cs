using System.Windows;
using UMLRedactor.Additions;

namespace UMLRedactor.Tools.Lines
{
    public interface ILine
    {
        Enums.LineTypes Type { get; set; }
        Point StartPoint { get; set; }
        Point EndPoint { get; set; }
    }
}