using System.IO;
using System.Xml;
using System.Xml.Linq;
using UMLRedactor.Models;

namespace UMLRedactor.Controller
{
    public class DomParser
    {
        private XDocument _xmlDocument { get; set; }

        public DomParser(string path)
        {
            _xmlDocument = XDocument.Load(path);
        }

        public DomModel GetModelFromMadFile()
        {
            DomModel domModel = new DomModel();
            XElement xElementRoot = _xmlDocument.Root;
            domModel.ProgramName = xElementRoot.Element("XMI.exporter").Value;
            domModel.ProgramVersion = xElementRoot.Element("XMI.exporterVersion").Value;
            return domModel;
        }

        public DomModel GetModelFromOtherFile()
        {
            DomModel domModel = new DomModel();
            return domModel;
        }

        public static void SaveModelToFile(DomModel model)
        {

        }
    }
}