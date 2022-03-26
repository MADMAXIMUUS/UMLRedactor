using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UMLRedactor.Additions;
using UMLRedactor.Models;
using Attribute = UMLRedactor.Additions.Attribute;

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
            if (xElementRoot != null && xElementRoot.Element("XMI.exporter")?.Value != "MadUML")
                return GetModelFromOtherFile(out model);

            return GetModelFromMadFile(out model);
        }

        private int GetModelFromMadFile(out Model model)
        {
            model = new Model();
            XElement xElementRoot = XmlDocument.Root;
            if (xElementRoot == null)
                return -1;
            model.ProgramName = xElementRoot.Element("XMI.exporter")?.Value;
            model.ProgramVersion = xElementRoot.Element("XMI.exporterVersion")?.Value;
            model.Author = xElementRoot.Element("Author")?.Value;
            model.Name = xElementRoot.Element("UML:Model")?.Attribute("name")?.Value;
            model.Root = new ModelNodeBase
            {
                Name = xElementRoot.Element("UML:Model")?
                    .Element("UML:ModelElement.rootElement")?
                    .Element("Name")?
                    .Value,
                Type = GetElementType(xElementRoot.Element("UML:Model")?.Element("UML:Class")),
                Id = xElementRoot.Element("UML:Model")?
                    .Element("UML:ModelElement.rootElement")?
                    .Element("ID")?
                    .Value,
                NamespaceId = xElementRoot.Element("UML:Model")?
                    .Element("UML:ModelElement.rootElement")?
                    .Element("Namespace")?
                    .Value,
                ChildNodes = new List<ModelNodeBase>()
            };
            var elements = xElementRoot.Element("UML:Namespace.ownedElement")?.Elements().ToList();
            if (elements != null)
                foreach (var element in elements)
                {
                    if (GetElementType(element) == "Not element" && GetLineType(element) == "Not line")
                        continue;

                    if (GetElementType(element) != "Not element" && GetLineType(element) == "Not line")
                    {
                        model.Root.ChildNodes.Add(new ModelNodeElement
                        {
                            Name = element.Element("Name")?.Value,
                            Id = element.Element("ID")?.Value,
                            NamespaceId = element.Element("Namespace")?.Value,
                            Stereotype = element.Element("Stereotype")?.Value,
                            Type = GetElementType(element),
                            Attributes = new List<Attribute>(),
                            Operations = new List<Operation>(),
                            ChildNodes = new List<ModelNodeBase>()
                        });
                        var classifierFeatures = element.Element("UML:Classifier.feature")?.Elements().ToList();
                        if (classifierFeatures != null)
                            foreach (XElement classifierFeature in classifierFeatures)
                            {
                                if (classifierFeature.Name.LocalName == "UML:Attribute")
                                    (model.Root.ChildNodes[model.Root.ChildNodes.Count - 1] as ModelNodeElement)
                                        ?.Attributes
                                        .Add(new Attribute()
                                        {
                                        });
                                if (classifierFeature.Name.LocalName == "UML:Operation")
                                {
                                    (model.Root.ChildNodes[model.Root.ChildNodes.Count - 1] as ModelNodeElement)
                                        ?.Operations
                                        .Add(new Operation()
                                        {
                                        });
                                }
                            }
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
                case ("UML:Class"):
                    return "Class";
                case ("UML:Package"):
                    return "Package";
                default:
                    return "Not element";
            }
        }

        private string GetLineType(XElement element)
        {
            switch (element.Name.LocalName)
            {
                case ("UML:Association"):
                    return "AssociationLink";
                default:
                    return "Not link";
            }
        }

        private Operation GetOperation(XElement element)
        {
            return new Operation();
        }

        private Attribute GetAttribute(XElement element)
        {
            return new Attribute();
        }
    }
}