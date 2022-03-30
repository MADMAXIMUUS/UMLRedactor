using System.Collections.Generic;
using UMLRedactor.Additions;

namespace UMLRedactor.Models
{
    public class Diagram
    {
        public string Name;
        public Package Namespace;
        public List<DiagramNode> Elements;

        public Diagram()
        {
            Elements = new List<DiagramNode>();
        }
    }
}