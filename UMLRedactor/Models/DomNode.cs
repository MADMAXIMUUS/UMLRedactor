using System.Collections.Generic;
using System.Xml;
using UMLRedactor.Additions;

namespace UMLRedactor.Models
{
    public class DomNode
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string UID { get; set; }
        
        public List<Atribute> Atributes { get; set; }
        public List<Operation> Operations { get; set; }

        public string Source { get; set; }
        public string Target { get; set; }
        public string Text { get; set; }
        public string SourceText { get; set; }
        public string TargetText { get; set; }

    }
}