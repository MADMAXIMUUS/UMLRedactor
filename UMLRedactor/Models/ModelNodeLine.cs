using System;
using System.Collections.Generic;

namespace UMLRedactor.Models
{
    public class ModelNodeLine: ModelNodeBase
    {
        public string Source;
        public string Target;
        public string TextOnLine;
        public string TextSourceOnLine;
        public string TextTargetOnLine;

        public ModelNodeLine()
        {
            
        }
        
        public ModelNodeLine(
            string name, 
            string type, 
            string id,
            string source, 
            string target, 
            string textOnLine, 
            string textSourceOnLine, 
            string textTargetOnLine) : 
            base(name,type,id)
        {
            Source = source;
            Target = target;
            TextOnLine = textOnLine;
            TextSourceOnLine = textSourceOnLine;
            TextTargetOnLine = textTargetOnLine;
        }
    }
}