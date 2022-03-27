using System.Collections.Generic;

namespace UMLRedactor.Models
{
    public class ModelNodeBase
    {
        public string Name;
        public string Type;
        public string Id;
        public string NamespaceId; 
        public List<ModelNodeBase> ChildNodes;

        public ModelNodeBase() { }

        public ModelNodeBase(
            string name,
            string type,
            string id)
        {
            Name = name;
            Type = type;
            Id = id;
            ChildNodes = new List<ModelNodeBase>();
        }
    }
}