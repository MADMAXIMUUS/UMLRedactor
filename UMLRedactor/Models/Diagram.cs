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
            Name = "";
            Namespace = new Package
            {
                PackageId = "",
                PackageName = ""
            };
            Elements = new List<DiagramNode>();
        }
    }
}