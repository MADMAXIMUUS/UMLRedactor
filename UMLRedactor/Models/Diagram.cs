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

        public DiagramNode GetElement(string modelElementId)
        {
            foreach (DiagramNode node in Elements)
            {
                if (node.ModelElementId == modelElementId)
                    return node;
            }
            return new DiagramNode();
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

        public void UpdateLinePosition(double newX1, double newY1, double newX2, double newY2, string elementId)
        {
            foreach (DiagramNode element in Elements)
            {
                if (element.ModelElementId == elementId)
                {
                    element.X1 = newX1;
                    element.Y1 = newY1;
                    element.X2 = newX2;
                    element.Y2 = newY2;
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