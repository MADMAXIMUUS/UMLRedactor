using System.Collections.Generic;
using UMLRedactor.Additions;

namespace UMLRedactor.Models
{
    public class ModelNodeBase
    {
        public string Name;
        public string Type;
        public string Id;
        public Package Namespace; 
        public List<ModelNodeBase> ChildNodes;

        public ModelNodeBase()
        {
            ChildNodes = new List<ModelNodeBase>();
        }
    }
}