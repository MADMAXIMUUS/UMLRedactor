using System.Collections.Generic;
using UMLRedactor.Additions;

namespace UMLRedactor.Models
{
    public class ModelNodeElement: ModelNodeBase
    {

        public string Stereotype;
        public List<Attribute> Attributes;
        public List<Operation> Operations;

        public ModelNodeElement()
        {
            Stereotype = "";
            Attributes = new List<Attribute>();
            Operations = new List<Operation>();
        }
    }
}