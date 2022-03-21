using System;
using System.Collections.Generic;
using UMLRedactor.Additions;
using Attribute = UMLRedactor.Additions.Attribute;

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
            Guid id, 
            List<ModelNodeBase> childNodes,
            string stereotype, 
            List<Attribute> attributes, 
            List<Operation> operations) : 
            base(name,type,id,childNodes)
        {
            Stereotype = stereotype;
            Attributes = attributes;
            Operations = operations;
        }
    }
}