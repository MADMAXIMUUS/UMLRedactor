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
            
        }
        
        public ModelNodeElement(
            string name, 
            string type, 
            string id,
            string stereotype, 
            List<Attribute> attributes, 
            List<Operation> operations) : 
            base(name,type,id)
        {
            Stereotype = stereotype;
            Attributes = new List<Attribute>();
            Operations = new List<Operation>();
        }
    }
}