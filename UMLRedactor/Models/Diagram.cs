﻿using System;
using System.Collections.Generic;

namespace UMLRedactor.Models
{
    public class Diagram
    {
        public string Name;
        public Guid PackageId;
        public List<DiagramNode> Elements;

        public Diagram()
        {
            
        }
        
        public Diagram(string name, Guid packageId, List<DiagramNode> elements)
        {
            Name = name;
            PackageId = packageId;
            Elements = elements;
        }
    }
}