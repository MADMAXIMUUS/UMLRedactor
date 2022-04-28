using System.Xml.Linq;
using UMLRedactor.Models;

namespace UMLRedactor.Controllers
{
    public class DiagramWriter
    {
        private static XDocument _xmlDocument;

        public int SaveToXml(Diagram diagram, string pathToSave, string pathToModel)
        {
            _xmlDocument = new XDocument(new XDeclaration("0.1", "utf-8", ""));
            XElement root = new XElement("Diagram",
                new XAttribute("name", diagram.Name),
                new XAttribute("packageName", diagram.Namespace.PackageName),
                new XAttribute("packageId", diagram.Namespace.PackageId),
                new XAttribute("pathToModel", pathToModel)
            );
            foreach (DiagramNode dn in diagram.Elements)
            {
                XElement element = new XElement("element",
                    new XAttribute("id", dn.Id),
                    new XAttribute("modelElementId", dn.ModelElementId));
                XElement elementPosition = new XElement("Position",
                    new XAttribute("x1", dn.X1),
                    new XAttribute("Y1", dn.Y1),
                    new XAttribute("x2", dn.X2),
                    new XAttribute("y2", dn.Y2)
                );
                element.Add(elementPosition);
                XElement elementSize = new XElement("Size",
                    new XAttribute("width", dn.Width),
                    new XAttribute("height", dn.Height)
                );
                element.Add(elementSize);
                root.Add(element);
            }
            _xmlDocument.Add(root);
            _xmlDocument.Save(pathToSave);
            return 0;
        }
    }
}