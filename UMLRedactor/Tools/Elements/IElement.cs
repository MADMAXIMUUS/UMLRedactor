using UMLRedactor.Models;

namespace UMLRedactor.Tools.Elements
{
    public interface IElement
    {
        ModelNodeElement GetModelElement();
        void AddContextMenu();
    }
}