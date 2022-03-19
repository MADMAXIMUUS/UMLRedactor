using System;
using System.Collections.Generic;
using UMLRedactor.Additions;
using Attribute = UMLRedactor.Additions.Attribute;

namespace UMLRedactor.Models
{
    public class DomModel
    {
        public string Name { get; set; }
        public string ProgramName { get; set; }
        public string ProgramVersion { get; set; }
        public string Author { get; set; }
        public ModelNodeBase Root { get; set; }
    }
}