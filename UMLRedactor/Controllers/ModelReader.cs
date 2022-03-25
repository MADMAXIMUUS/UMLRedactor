using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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
            ModelNodeBase root = new ModelNodeBase
            {
                Name = xElementRoot.Element("UML:Model")?.Element("UML:ModelElement.rootElement").Element("Name")
                    ?.Value,
                Type = GetElementType(xElementRoot.Element("UML:Model")?.Element("UML:Class")),
                Id = xElementRoot.Element("UML:Model")?.Element("UML:ModelElement.rootElement").Element("ID")?.Value,
                NamespaceId = xElementRoot.Element("UML:Model")?.Element("UML:ModelElement.rootElement")
                    .Element("Namespace")?.Value,
                ChildNodes = new List<ModelNodeBase>()
            };


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
                    return "Class";
            }
        }
    }
}