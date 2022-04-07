using System.Xml.Linq;
using UMLRedactor.Models;

namespace UMLRedactor.Controllers
{
    public class DiagramReader
    {
        private static XDocument XmlDocument { get; set; }

        public DiagramReader(string path)
        {
            XmlDocument = XDocument.Load(path);
        }

        public int SaveToXml(Model model)
        {
            return 0;
        }
        
    }
}