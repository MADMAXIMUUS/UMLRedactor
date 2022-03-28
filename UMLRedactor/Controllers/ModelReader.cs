using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
                return -2;
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
                            model.Root.ChildNodes.Add(GetElementFromMad(element));
                        else
                            model.Root.ChildNodes.Add(GetLineFromMad(element));
                    }
            }

            return 0;
        }

        private int GetModelFromOtherFile(out Model model)
        {
            model = new Model();
            XElement xElementRoot = XmlDocument.Root;
            if (xElementRoot == null)
                return -2;
            if (xElementRoot.FirstAttribute?.Value != "1.1")
                return -1;
            XElement xElementDocumentationRoot = xElementRoot.Element("XMI.header")?.Element("XMI.documentation");
            if (xElementDocumentationRoot == null)
                return -2;

            model.ProgramName = xElementDocumentationRoot.Element("XMI.exporter")?.Value;
            model.ProgramVersion = xElementDocumentationRoot.Element("XMI.exporterVersion")?.Value;
            model.Name = xElementRoot.Element("XMI.content")?.Element(Uml("Model"))?.Attribute("name")?.Value;

            XElement xModelRoot = xElementRoot.Element("XMI.content")?
                .Element(Uml("Model"))?
                .Element(Uml("Namespace.ownedElement"));

            if (xModelRoot == null)
                return -2;

            model.Root = new ModelNodeBase
            {
                Name = xModelRoot.Element(Uml("Class"))?.Attribute("name")?.Value,
                Id = xModelRoot.Element(Uml("Class"))?.Attribute("xmi.id")?.Value,
                ChildNodes = new List<ModelNodeBase>()
            };

            List<XElement> taggedValue = xModelRoot.Element(Uml("Package"))?
                .Element(Uml("ModelElement.taggedValue"))?
                .Elements()
                .ToList();

            if (taggedValue == null)
                return -2;

            foreach (XElement value in taggedValue)
            {
                if (value.Attribute("tag")?.Value != "author")
                    continue;
                model.Author = value.Attribute("value")?.Value;
            }

            List<XElement> elements = xModelRoot.Element(Uml("Package"))?
                .Element(Uml("Namespace.ownedElement"))?
                .Elements().ToList();
            if (elements == null)
                return -2;
            foreach (XElement element in elements)
            {
                if (GetElementType(element) == "Not element" && GetLineType(element) == "Not line")
                    continue;

                if (GetElementType(element) != "Not element")
                    model.Root.ChildNodes.Add(GetElementFromEa(element));
                else
                    model.Root.ChildNodes.Add(GetLineFromEa(element));
            }

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
                case "ActionState":
                    return "Activity";
                case "PseudoState":
                    return "Pseudo";
                default:
                    return "Not element";
            }
        }

        private string GetLineType(XElement element)
        {
            switch (element.Name.LocalName)
            {
                case "Association":
                    return "AssociationLink";
                case "Generalization":
                    return "GeneralizationLink";
                case "Transition":
                    return "TransitionLink";
                default:
                    return "Not line";
            }
        }

        private ModelNodeElement GetElementFromEa(XElement element)
        {
            ModelNodeElement elem = new ModelNodeElement()
            {
                Name = element.Attribute("name")?.Value,
                Id = element.Attribute("xmi.id")?.Value,
                Type = GetElementType(element),
                Stereotype = element.Element(Uml("ModelElement.stereotype"))?.Element(Uml("Stereotype"))?
                    .Attribute("name")?.Value,
                ChildNodes = new List<ModelNodeBase>(),
                Attributes = new List<Attribute>(),
                Operations = new List<Operation>()
            };

            List<XElement> taggedValue = element.Element(Uml("ModelElement.taggedValue"))?.Elements().ToList();
            if (taggedValue != null)
                foreach (XElement value in taggedValue)
                {
                    if (value.Attribute("tag")?.Value != "package")
                        continue;
                    elem.NamespaceId = value.Attribute("value")?.Value;
                }

            List<XElement> classifiers = element.Element(Uml("Classifier.feature"))?.Elements().ToList();
            if (classifiers != null)
            {
                foreach (XElement classifier in classifiers)
                {
                    if (classifier.Name.LocalName == "Attribute")
                        elem.Attributes.Add(GetAttributeFromEa(classifier));
                    if (classifier.Name.LocalName == "Operation")
                        elem.Operations.Add(GetOperationFromEa(classifier));
                }
            }

            return elem;
        }

        private Attribute GetAttributeFromEa(XElement classifier)
        {
            Attribute at = new Attribute()
            {
                Name = classifier.Attribute("name")?.Value,
                AccessModifier = classifier.Attribute("visibility")?.Value
            };
            List<XElement> taggedValue = classifier.Element(Uml("ModelElement.taggedValue"))?.Elements().ToList();
            if (taggedValue != null)
                foreach (XElement value in taggedValue)
                {
                    if (value.Attribute("tag")?.Value != "type")
                        continue;
                    at.DataType = value.Attribute("value")?.Value;
                    break;
                }

            return at;
        }

        private Operation GetOperationFromEa(XElement classifier)
        {
            Operation op = new Operation()
            {
                Name = classifier.Attribute("name")?.Value,
                AccessModifier = classifier.Attribute("visibility")?.Value,
                Parameters = new List<Parameter>()
            };
            List<XElement> taggedValue = classifier.Element(Uml("ModelElement.taggedValue"))?.Elements().ToList();
            if (taggedValue != null)
                foreach (XElement value in taggedValue)
                {
                    if (value.Attribute("tag")?.Value != "type")
                        continue;
                    op.DataTypeOfReturnValue = value.Attribute("value")?.Value;
                    break;
                }

            List<XElement> parameters = classifier.Element(Uml("BehavioralFeature.parameter"))?.Elements().ToList();
            if (parameters != null)
                foreach (XElement parameter in parameters)
                {
                    if (parameter.Attribute("kind")?.Value == "return")
                        continue;
                    op.Parameters.Add(GetParameterFromEa(parameter));
                }

            return op;
        }

        private Parameter GetParameterFromEa(XElement parameter)
        {
            Parameter p = new Parameter
            {
                Name = parameter.Attribute("name")?.Value,
                DefaultValue = parameter.Element(Uml("Parameter.defaultValue"))?
                    .Element(Uml("Expression"))?
                    .Attribute("body")?.Value
            };
            List<XElement> taggedValue = parameter.Element(Uml("ModelElement.taggedValue"))?.Elements().ToList();
            if (taggedValue != null)
                foreach (XElement value in taggedValue)
                {
                    if (value.Attribute("tag")?.Value != "type")
                        continue;
                    p.DataType = value.Attribute("value")?.Value;
                }

            return p;
        }

        private ModelNodeLine GetLineFromEa(XElement element)
        {
            ModelNodeLine line = new ModelNodeLine
            {
                Name = GetLineType(element),
                Id = element.Attribute("xmi.id")?.Value,
                Type = GetLineType(element),
                TextOnLine = element.Attribute("name")?.Value
            };
            List<XElement> ends = element.Element(Uml("Association.connection"))?.Elements().ToList();
            if (ends != null)
                foreach (XElement end in ends)
                {
                    List<XElement> taggedValue = end.Element(Uml("ModelElement.taggedValue"))?.Elements().ToList();
                    if (taggedValue!=null)
                        foreach (XElement value in taggedValue)
                        {
                            if (value.Attribute("tag")?.Value != "ea_end")
                                continue;

                            if (value.Attribute("value")?.Value == "source")
                            {
                                line.Source = end.Attribute("type")?.Value;
                                line.TextSourceOnLine = end.Attribute("name")?.Value;
                            }
                            else
                            {
                                line.Target = end.Attribute("type")?.Value;
                                line.TextTargetOnLine = end.Attribute("name")?.Value;
                            }
                        }
                }

            return line;
        }


        private ModelNodeElement GetElementFromMad(XElement element)
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
                        elem.Attributes.Add(GetAttributeFromMad(classifierFeature));
                    if (classifierFeature.Name.LocalName == "Operation")
                        elem.Operations.Add(GetOperationFromMad(classifierFeature));
                }

            return elem;
        }

        private Operation GetOperationFromMad(XElement element)
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

        private Attribute GetAttributeFromMad(XElement element)
        {
            Attribute at = new Attribute()
            {
                Name = element.Element("Name")?.Value,
                AccessModifier = element.Element("Access")?.Value,
                DataType = element.Element("DataType")?.Value
            };
            return at;
        }

        private ModelNodeLine GetLineFromMad(XElement element)
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