using System.Windows;
using UMLRedactor.Additions;
using UMLRedactor.Tools.Elements;

namespace UMLRedactor.Tools.Lines
{
    public interface ILine
    {
        int LocalId { get; set; }
        Enums.LineTypes Type { get; set; }
        Point StartPoint { get; set; }
        Point EndPoint { get; set; }
        bool SrcIsOrdered { get; set; }
        bool DstIsOrdered { get; set; }
        IElement Source { get; set; }
        IElement Target { get; set; }
    }
}