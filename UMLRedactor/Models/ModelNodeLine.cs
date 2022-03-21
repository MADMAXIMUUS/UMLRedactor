using System;
using System.Collections.Generic;

namespace UMLRedactor.Models
{
    public class ModelNodeLine: ModelNodeBase
    {
        public Guid Source;
        public Guid Target;
        public string TextOnLine;
        public string TextSourceOnLine;
        public string TextTargetOnLine;

        public ModelNodeLine()
        {
            
        }
        
        public ModelNodeLine(
            string name, 
            string type, 
            Guid id, 
            List<ModelNodeBase> childNodes,
            Guid source, 
            Guid target, 
            string textOnLine, 
            string textSourceOnLine, 
            string textTargetOnLine) : 
            base(name,type,id,childNodes)
        {
            Source = source;
            Target = target;
            TextOnLine = textOnLine;
            TextSourceOnLine = textSourceOnLine;
            TextTargetOnLine = textTargetOnLine;
        }
    }
}