using System.Xml.Linq;
using UMLRedactor.Models;

namespace UMLRedactor.Controllers
{
    public class ModelReader
    {
        private static XDocument _xmlDocument { get; set; }

        public ModelReader(string path)
        {
            _xmlDocument = XDocument.Load(path);
        }

        public int GetModelFromFile(out Model model)
        {
            model = new Model();
            XElement xElementRoot = _xmlDocument.Root;
            if (xElementRoot.Element("XMI.exporter").Value != "MadUML")
                return GetModelFromOtherFile(out model);

            return GetModelFromMadFile(out model);
        }

        public int GetModelFromMadFile(out Model model)
        {
            model = new Model();
            XElement xElementRoot = _xmlDocument.Root;
            return 0;
        }

        public int GetModelFromOtherFile(out Model model)
        {
            model = new Model();
            XElement xElementRoot = _xmlDocument.Root;
            if (xElementRoot.Attribute("xmi.version").Value == "1.1")
                return -1;
            model.ProgramName = xElementRoot.Element("XMI.exporter").Value;
            model.ProgramVersion = xElementRoot.Element("XMI.exporterVersion").Value;
            model.Name = xElementRoot.Element("UML:Model").Attribute("name").Value;
            ModelNodeBase root = new ModelNodeBase
            {
                Name = model.Name
            };
            return 0;
        }
    }
}