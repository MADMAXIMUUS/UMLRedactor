using System;
using System.Collections.Generic;

namespace UMLRedactor.Models
{
    public class Diagram
    {
        public string Name;
        public string PackageId;
        public List<DiagramNode> Elements;

        public Diagram()
        {
            Elements = new List<DiagramNode>();
        }
        
        public Diagram(string name, string packageId)
        {
            Name = name;
            PackageId = packageId;
            Elements = new List<DiagramNode>();
        }
    }
}