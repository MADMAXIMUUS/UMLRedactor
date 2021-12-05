using UMLRedactor.Additions;

namespace UMLRedactor.Tools.Elements
{
    public interface IElement
    {
        int LocalId { get; set; }
        string Header { get; set; }
        Enums.ElementTypes Type { get; set; }
    }
}