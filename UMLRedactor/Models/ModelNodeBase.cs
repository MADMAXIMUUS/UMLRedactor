using System;
using System.Collections.Generic;
using System.IO.Packaging;

namespace UMLRedactor.Models
{
    public class ModelNodeBase
    {
        public string Name;
        public string Type;
        public Guid Id;
        public Guid NamespaceId; 
        public List<ModelNodeBase> ChildNodes;

        public ModelNodeBase() { }

        public ModelNodeBase(
            string name,
            string type,
            Guid id)
        {
            Name = name;
            Type = type;
            Id = id;
            ChildNodes = new List<ModelNodeBase>();
        }
    }
}