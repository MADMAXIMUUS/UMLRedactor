using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace UMLRedactor.Models
{
    public class DomModel
    {
        public List<DomNode> ChildrenNodes;

        DomModel()
        {
            ChildrenNodes = new List<DomNode>();
        }

        DomModel(string path)
        {
            ChildrenNodes = LoadModel(path);
        }

        public static List<DomNode> LoadModel(string path = "model.xml")
        {
            XDocument document = XDocument.Load(path);  
            return (from item in document.Element("model")?.Elements()  
                select new DomNode()  
                {
                    Name = item.Element("name")?.Value,  
                    Width = Int32.Parse(item.Element("width")?.Value ?? "200"), 
                    Height = Int32.Parse(item.Element("height")?.Value ?? "200")  
                }).ToList();
        }
        
    }
}