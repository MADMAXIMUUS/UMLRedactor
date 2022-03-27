using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UMLRedactor.Additions;
using UMLRedactor.Models;

namespace UMLRedactor.Controllers
{
    public class ModelReader
    {
        private static XDocument XmlDocument { get; set; }

        public ModelReader(string path)
        {
            XmlDocument = XDocument.Load(path);
        }

        public int GetModelFromFile(out Model model)
        {
            model = new Model();
            XElement xElementRoot = XmlDocument.Root;
            if (xElementRoot == null)
                return -1;
            if (xElementRoot.Element("XMI.header")?
                    .Element("XMI.documentation")?
                    .Element("XMI.exporter")?
                    .Value != "MadUML")
                return GetModelFromOtherFile(out model);
            
            return GetModelFromMadFile(out model);
        }

        private int GetModelFromMadFile(out Model model)
        {
            model = new Model();
            XElement xElementRoot = XmlDocument.Root;

            if (xElementRoot == null)
                return -1;
            XElement xElementDocumentationRoot = xElementRoot.Element("XMI.header")?.Element("XMI.documentation");
            if (xElementDocumentationRoot != null)
            {
                model.ProgramName = xElementDocumentationRoot.Element("XMI.exporter")?.Value;
                model.ProgramVersion = xElementDocumentationRoot.Element("XMI.exporterVersion")?.Value;
                model.Author = xElementDocumentationRoot.Element("Author")?.Value;
                model.Name = xElementRoot.Element("XMI.content")?.Element(Uml("Model"))?.Attribute("name")?.Value;
            }

            XElement xContent = xElementRoot.Element("XMI.content");
            if (xContent != null)
            {
                model.Root = new ModelNodeBase
                {
                    Name = xContent.Element(Uml("Model"))?
                        .Element(Uml("Package"))?
                        .Element(Uml("ModelElement.rootElement"))?
                        .Element("Name")?
                        .Value,
                    Type = GetElementType(xContent.Element(Uml("Model"))?.Element(Uml("Class"))),
                    Id = xContent.Element(Uml("Model"))?
                        .Element(Uml("Package"))?
                        .Element(Uml("ModelElement.rootElement"))?
                        .Element("ID")?
                        .Value,
                    NamespaceId = xContent.Element(Uml("Model"))?
                        .Element(Uml("Package"))?
                        .Element(Uml("ModelElement.rootElement"))?
                        .Element("Namespace")?
                        .Value,
                    ChildNodes = new List<ModelNodeBase>()
                };
                List<XElement> elements = xContent.Element(Uml("Model"))?
                    .Element(Uml("Package"))?
                    .Element(Uml("Namespace.ownedElement"))?
                    .Elements().ToList();
                if (elements != null)
                    foreach (var element in elements)
                    {
                        if (GetElementType(element) == "Not element" && GetLineType(element) == "Not line")
                            continue;

                        if (GetElementType(element) != "Not element")
                            model.Root.ChildNodes.Add(GetElement(element));
                        else
                            model.Root.ChildNodes.Add(GetLine(element));
                    }
            }

            return 0;
        }

        private int GetModelFromOtherFile(out Model model)
        {
            model = new Model();
            XElement xElementRoot = XmlDocument.Root;
            if (xElementRoot == null)
                return -1;
            if (xElementRoot.Attribute("xmi.version")?.Value == "1.1")
                return -1;

            model.ProgramName = xElementRoot.Element("XMI.exporter")?.Value;
            model.ProgramVersion = xElementRoot.Element("XMI.exporterVersion")?.Value;
            model.Name = xElementRoot.Element("UML:Model")?.Attribute("name")?.Value;

            ModelNodeBase root = new ModelNodeBase
            {
                Name = model.Name
            };


            return 0;
        }

        private string GetElementType(XElement element)
        {
            switch (element.Name.LocalName)
            {
                case "Class":
                    return "Class";
                case "Package":
                    return "Package";
                default:
                    return "Not element";
            }
        }

        private string GetLineType(XElement element)
        {
            switch (element.Name.LocalName)
            {
                case ("Association"):
                    return "AssociationLink";
                default:
                    return "Not line";
            }
        }

        private ModelNodeElement GetElement(XElement element)
        {
            ModelNodeElement elem = new ModelNodeElement
            {
                Name = element.Element("Name")?.Value,
                Id = element.Element("ID")?.Value,
                NamespaceId = element.Element("Namespace")?.Value,
                Stereotype = element.Element("Stereotype")?.Value,
                Type = GetElementType(element),
                Attributes = new List<Attribute>(),
                Operations = new List<Operation>(),
                ChildNodes = new List<ModelNodeBase>()
            };
            List<XElement> classifierFeatures = element.Element(Uml("Classifier.feature"))?.Elements().ToList();
            if (classifierFeatures != null)
                foreach (XElement classifierFeature in classifierFeatures)
                {
                    if (classifierFeature.Name.LocalName == "Attribute")
                        elem.Attributes.Add(GetAttribute(classifierFeature));
                    if (classifierFeature.Name.LocalName == "Operation")
                        elem.Operations.Add(GetOperation(classifierFeature));
                }
            return elem;
        }

        private Operation GetOperation(XElement element)
        {
            Operation op = new Operation
            {
                Name = element.Element("Name")?.Value,
                AccessModifier = element.Element("Access")?.Value,
                DataTypeOfReturnValue = element.Element("ReturnType")?.Value,
                Parameters = new List<Parameter>()
            };
            List<XElement> parameters = element.Element(Uml("BehavioralFeature.parameter"))?.Elements().ToList();
            if (parameters != null)
                foreach (XElement par in parameters)
                {
                    op.Parameters.Add(new Parameter()
                    {
                        Name = par.Element("Name")?.Value,
                        DataType = par.Element("DataType")?.Value,
                        DefaultValue = par.Element("DefaultValue")?.Value
                    });
                }

            return op;
        }

        private Attribute GetAttribute(XElement element)
        {
            Attribute at = new Attribute()
            {
                Name = element.Element("Name")?.Value,
                AccessModifier = element.Element("Access")?.Value,
                DataType = element.Element("DataType")?.Value
            };
            return at;
        }

        private ModelNodeLine GetLine(XElement element)
        {
            ModelNodeLine line = new ModelNodeLine
            {
                Name = element.Element("Name")?.Value,
                Id = element.Element("ID")?.Value,
                NamespaceId = element.Element("Namespace")?.Value,
                ChildNodes = null,
                Source = element.Element("Source")?.Value,
                Target = element.Element("Target")?.Value,
                Type = GetLineType(element),
                TextOnLine = element.Element("TextOnLine")?.Value,
                TextSourceOnLine = element.Element("TextSourceOnLine")?.Value,
                TextTargetOnLine = element.Element("TextTargetOnLine")?.Value
            };
            return line;
        }

        static XName Uml(string name) => XNamespace.Get("omg.org/UML1.3") + name;
    }
}