using System.Collections.Generic;
using UMLRedactor.Additions;

namespace UMLRedactor.Models
{
    public class DomModel
    {
        public string Name { get; set; }
        public string ProgramName { get; set; }
        public string ProgramVersion { get; set; }
        public string Author { get; set; }
        public DomNode Root { get; set; }

        public void AddNode(
            ref DomNode parentNode,
            string name,
            string type,
            string GUID,
            List<Attribute> atributes,
            List<Operation> operations
        )
        {
            DomNode newNode = new DomNode()
            {
                Name = name,
                Type = type,
                GUID = GUID,
                Attributes = atributes,
                Operations = operations
            };

            parentNode.ChildNodes.Add(newNode);
        }

        public void AddNode(
            ref DomNode parentNode,
            string name,
            string type,
            string GUID,
            string source,
            string target,
            string textOnLine,
            string textSourceOnLine,
            string textTargetOnLine
        )
        {
            DomNode newNode = new DomNode()
            {
                Name = name,
                Type = type,
                GUID = GUID,
                Source = source,
                Target = target,
                TextOnLine = textOnLine,
                TextSourceOnLine = textSourceOnLine,
                TextTargetOnLine = textTargetOnLine

            };

            parentNode.ChildNodes.Add(newNode);
        }

        public void RemoveNodeById(string GUID)
        {

        }
    }
}