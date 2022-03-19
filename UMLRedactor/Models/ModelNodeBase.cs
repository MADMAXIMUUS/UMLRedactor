using System;
using System.Collections.Generic;

namespace UMLRedactor.Models
{
    public class ModelNodeBase
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Guid GUID { get; set; }

        public List<ModelNodeBase> ChildNodes { get; set; }
    }
}