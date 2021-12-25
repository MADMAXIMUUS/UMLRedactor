using System.Xml;

namespace UMLRedactor.Models
{
    public class DomModel
    {
        public string Name { get; set; }
        public string ProgramName { get; set; }
        public string ProgramVersion { get; set; }
        public string Author { get; set; }
        public DomNode Root { get; set; }


        public void AddNode()
        {
            DomNode newNode = new DomNode()
            {

            };
        }

        public void RemoveNode()
        {

        }
    }
}