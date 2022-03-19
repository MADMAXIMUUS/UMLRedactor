using System.Collections.Generic;
using UMLRedactor.Additions;

namespace UMLRedactor.Models
{
    public class ModelNodeElement: ModelNodeBase
    {
        public List<Attribute> Attributes { get; set; }
        public List<Operation> Operations { get; set; }
    }
}