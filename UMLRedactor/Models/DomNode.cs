using System.Collections.Generic;
using UMLRedactor.Additions;

namespace UMLRedactor.Models
{
    public class DomNode
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string GUID { get; set; }

        public List<DomNode> ChildNodes { get; set; }

        public List<Attribute> Attributes { get; set; }
        public List<Operation> Operations { get; set; }

        public string Source { get; set; }
        public string Target { get; set; }
        public string TextOnLine { get; set; }
        public string TextSourceOnLine { get; set; }
        public string TextTargetOnLine { get; set; }

    }
}