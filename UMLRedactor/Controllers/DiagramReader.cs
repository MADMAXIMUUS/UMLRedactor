using System.Xml.Linq;
using UMLRedactor.Models;

namespace UMLRedactor.Controllers
{
    public class DiagramReader
    {
        private static XDocument _xmlDocument;

        public DiagramReader(string path)
        {
            _xmlDocument = XDocument.Load(path);
        }
    }
}