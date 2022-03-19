using System;

namespace UMLRedactor.Models
{
    public class ModelNodeLine: ModelNodeBase
    {
        public Guid Source { get; set; }
        public Guid Target { get; set; }
        public string TextOnLine { get; set; }
        public string TextSourceOnLine { get; set; }
        public string TextTargetOnLine { get; set; }
    }
}