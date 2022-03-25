using System.Collections.Generic;
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
            model.Name = xElementRoot.Element("UML:Model")?.Attribute("name")?.Value;
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
    }
}