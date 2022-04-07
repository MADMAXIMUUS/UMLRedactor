using System.Linq;
using System.Xml.Linq;
using UMLRedactor.Models;

namespace UMLRedactor.Controllers
{
    public class ModelWriter
    {
        private static XDocument XmlDocument { get; set; }

        public int SaveToXml(Model model, string path)
        {
            XmlDocument = new XDocument(
                new XDeclaration("0.1", "windows-1252", ""),
                new XElement("XMI",
                    new XAttribute("xmi.version", "1.1"),
                    new XAttribute(XNamespace.Xmlns + "UML", "omg.org/UML1.3"),
                    new XElement("XMI.header",
                        new XElement("XMI.documentation",
                            new XElement("XMI.exporter", model.ProgramName),
                            new XElement("XMI.exporterVersion", model.ProgramVersion)
                        )
                    ),
                    new XElement("XMI.content",
                        new XElement(Uml("Model"),
                            new XAttribute("name", model.Name),
                            new XElement(Uml("Class"),
                                new XAttribute("isRoot", "true")
                            ),
                            new XElement(Uml("Package"),
                                new XAttribute("name", model.Root.Namespace.PackageName),
                                new XAttribute("xmi.id", model.Root.Namespace.PackageId),
                                new XElement(Uml("ModelElement.rootElement"),
                                    new XElement("Namespace", model.Root.Namespace.PackageId),
                                    new XElement("ID", model.Root.Id),
                                    new XElement("Name", model.Root.Name)
                                ),
                                new XElement(Uml("Namespace.ownedElement"),
                                    model.Root.ChildNodes.Select(x =>
                                        new XElement(Uml(x.Type),
                                            new XElement("Name", x.Name),
                                            new XElement("ID", x.Id),
                                            new XElement("Namespace", x.Namespace.PackageId)
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
            );

            XmlDocument.Save(path);
            return 0;
        }

        private bool IsElement(ModelNodeBase element)
        {
            switch (element.Type)
            {
                case "Class":
                    return true;
                case "Package":
                    return true;
                case "Activity":
                    return true;
                case "Pseudo":
                    return true;
                case "Actor":
                    return true;
                case "UseCase":
                    return true;
                case "SeqObject":
                    return true;
                case "Comment":
                    return true;
                default:
                    return false;
            }
        }

        private bool IsLine(ModelNodeBase element)
        {
            switch (element.Type)
            {
                case "AssociationLink":
                    return true;
                case "GeneralizationLink":
                    return true;
                case "Message":
                    return true;
                case "TransitionLink":
                    return true;
                default:
                    return false;
            }
        }

        static XName Uml(string name)
        {
            XNamespace uml = "omg.org/UML1.3";
            XName xName = uml + name;
            return xName;
        }
    }
}