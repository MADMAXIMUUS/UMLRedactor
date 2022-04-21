using System.Collections.Generic;
using UMLRedactor.Additions;

namespace UMLRedactor.Models
{
    public class Diagram
    {
        public string Name;
        public Package Namespace;
        public readonly List<DiagramNode> Elements;

        public Diagram()
        {
            Name = "MadDiagram";
            Namespace = new Package
            {
                PackageId = "",
                PackageName = ""
            };
            Elements = new List<DiagramNode>();
        }

        public void UpdateElementPosition(double newX, double newY, string elementId)
        {
            foreach (DiagramNode element in Elements)
            {
                if (element.ModelElementId == elementId)
                {
                    element.X1 = newX;
                    element.Y1 = newY;
                }
            }
        }

        public void UpdateElementSize(double newWidth, double newHeight, string elementId)
        {
            foreach (DiagramNode element in Elements)
            {
                if (element.ModelElementId == elementId)
                {
                    element.Width = newWidth;
                    element.Height = newHeight;
                }
            }
        }
    }
}