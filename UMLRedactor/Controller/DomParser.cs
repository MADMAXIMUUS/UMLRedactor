using System.Xml;

namespace UMLRedactor.Controller
{
    public class DomParser
    {
        private readonly XmlDocument _xmlDocument = new XmlDocument();

        public DomParser(string path)
        {
            _xmlDocument.Load(path);
        }
    }
}