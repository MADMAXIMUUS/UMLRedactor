using System.Collections.Generic;
using System.Xml.Linq;
using UMLRedactor.Additions;
using UMLRedactor.Models;

namespace UMLRedactor.Controllers
{
    public class ModelWriter
    {
        private static XDocument _xmlDocument;

        public void SaveToXml(Model model, string path)
        {
            _xmlDocument = new XDocument(new XDeclaration("0.1", "utf-8", ""));
            XElement root = new XElement("XMI",
                new XAttribute("xmi.version", "1.1"),
                new XAttribute(XNamespace.Xmlns + "UML", "omg.org/UML1.3")
            );
            XElement documentation = new XElement("XMI.header",
                new XElement("XMI.documentation",
                    new XElement("XMI.exporter", "MadUML"),
                    new XElement("XMI.exporterVersion", "0.5")
                )
            );
            root.Add(documentation);
            XElement content = new XElement("XMI.content");
            XElement xModel = new XElement(Uml("Model"),
                new XAttribute("name", model.Name)
            );
            XElement xClass = new XElement(Uml("Class"),
                new XAttribute("isRoot", "true")
            );
            xModel.Add(xClass);
            XElement xPackage = new XElement(Uml("Package"),
                new XAttribute("name", model.Root.Namespace.PackageName),
                new XAttribute("xmi.id", model.Root.Namespace.PackageId)
            );
            XElement rootElement = new XElement(Uml("ModelElement.rootElement"),
                new XElement("Namespace", model.Root.Namespace.PackageId),
                new XElement("ID", model.Root.Id),
                new XElement("Name", model.Root.Name)
            );
            xPackage.Add(rootElement);
            XElement ownedElement = new XElement(Uml("Namespace.ownedElement"));
            List<ModelNodeBase> elements = model.Root.ChildNodes;
            if (elements != null)
                foreach (ModelNodeBase element in elements)
                {
                    if (element is ModelNodeElement nodeElement)
                        ownedElement.Add(GetElement(nodeElement));

                    if (element is ModelNodeLine nodeLine)
                        ownedElement.Add(GetLine(nodeLine));
                }
            xPackage.Add(ownedElement);
            xModel.Add(xPackage);
            content.Add(xModel);
            root.Add(content);
            _xmlDocument.Add(root);
            _xmlDocument.Save(path);
        }

        private XElement GetElement(ModelNodeElement nodeElement)
        {
            XElement xElementType = new XElement(Uml(nodeElement.Type));
            XElement xElementName = new XElement("Name", nodeElement.Name);
            XElement xElementId = new XElement("Id", nodeElement.Id);
            XElement xElementNamespace = new XElement("Namespace", nodeElement.Namespace.PackageId);
            XElement xElementStereotype = new XElement("Stereotype", nodeElement.Stereotype);
            xElementType.Add(xElementName, xElementId, xElementNamespace, xElementStereotype);
            if (nodeElement.Attributes.Count != 0 || nodeElement.Operations.Count != 0)
            {
                XElement xElementClassifier = new XElement(Uml("Classifier.feature"));
                if (nodeElement.Attributes.Count != 0)
                {
                    foreach (Attribute attribute in nodeElement.Attributes)
                    {
                        XElement attributeRoot = new XElement(Uml("Attribute"));
                        XElement name = new XElement("Name", attribute.Name);
                        XElement access = new XElement("Access", attribute.AccessModifier);
                        XElement datatype = new XElement("DataType", attribute.DataType);
                        attributeRoot.Add(name, access, datatype);
                        xElementClassifier.Add(attributeRoot);
                    }
                }

                if (nodeElement.Operations.Count != 0)
                {
                    foreach (Operation operation in nodeElement.Operations)
                    {
                        XElement operationRoot = new XElement(Uml("Operation"));
                        XElement name = new XElement("Name", operation.Name);
                        XElement access = new XElement("Access", operation.AccessModifier);
                        XElement datatype = new XElement("DataType", operation.DataTypeOfReturnValue);
                        if (operation.Parameters.Count != 0)
                        {
                            XElement xParameters = new XElement(Uml("BehavioralFeature.parameter"));
                            foreach (Parameter parameter in operation.Parameters)
                            {
                                XElement xParameter = new XElement(Uml("Parameter"));
                                XElement parameterName = new XElement("Name", parameter.Name);
                                XElement parameterDataType = new XElement("DataType", parameter.DataType);
                                XElement parameterDefaultValue =
                                    new XElement("DefaultValue", parameter.DataType);
                                xParameter.Add(parameterName, parameterDataType, parameterDefaultValue);
                                xParameters.Add(xParameter);
                            }

                            operationRoot.Add(name, access, datatype);
                            xElementClassifier.Add(operationRoot);
                        }
                    }
                }

                xElementType.Add(xElementClassifier);
                if (nodeElement.ChildNodes.Count != 0)
                {
                    XElement elementOwnedElements = new XElement(Uml("Namespace.ownedElement"));
                    foreach (ModelNodeBase childElement in nodeElement.ChildNodes)
                    {
                        if (childElement is ModelNodeElement nodeChildElement)
                            elementOwnedElements.Add(GetElement(nodeChildElement));
                        if (childElement is ModelNodeLine nodeChildLine)
                            elementOwnedElements.Add(GetLine(nodeChildLine));
                    }
                }
            }

            return xElementType;
        }

        private XElement GetLine(ModelNodeLine line)
        {
            XElement xElementType = new XElement(Uml(line.Type));
            XElement xElementName = new XElement("Name", line.Name);
            XElement xElementId = new XElement("Id", line.Id);
            XElement xElementNamespace = new XElement("Namespace", line.Namespace.PackageId);
            XElement xElementSource = new XElement("Source", line.Source);
            XElement xElementTarget = new XElement("Source", line.Target);
            XElement xElementTextOnLine = new XElement("TextOnLine", line.TextOnLine);
            XElement xElementTextSourceOnLine = new XElement("TextSourceOnLine", line.TextSourceOnLine);
            XElement xElementTextTargetOnLine = new XElement("TextTargetOnLine", line.TextTargetOnLine);
            xElementType.Add(xElementName, xElementId, xElementNamespace, xElementSource, xElementTarget,
                xElementTextOnLine, xElementTextSourceOnLine, xElementTextTargetOnLine);
            return xElementType;
        }

        static XName Uml(string name)
        {
            XNamespace uml = "omg.org/UML1.3";
            XName xName = uml + name;
            return xName;
        }
    }
}