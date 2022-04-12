using System;
using System.Collections.Generic;

namespace UMLRedactor.Models
{
    public class Model
    {
        public string Name;
        public string ProgramName;
        public string ProgramVersion;
        public ModelNodeBase Root;

        public Model()
        {
            Name = "Model";
            ProgramName = "MadUML";
            ProgramVersion = "0.5";
            Root = new ModelNodeBase
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Root",
                Namespace =
                {
                    PackageName = "Model",
                    PackageId = ""
                },
                Type = "Root",
                ChildNodes = new List<ModelNodeBase>()
            };
        }

        public void AddElement(string parentId, ModelNodeBase element)
        {
            if (Root.Id == parentId)
                Root.ChildNodes.Add(element);
            else
                foreach (ModelNodeBase node in Root.ChildNodes)
                    CheckNodeId(parentId, node, element);
        }

        private void CheckNodeId(string id, ModelNodeBase node, ModelNodeBase element)
        {
            if (node.Id == id)
                node.ChildNodes.Add(element);
            else if (node.ChildNodes.Count != 0)
                foreach (ModelNodeBase child in node.ChildNodes)
                    CheckNodeId(id, child, element);
        }

        public ModelNodeBase GetNode(string id)
        {
            foreach (ModelNodeBase node in Root.ChildNodes)
            {
                if (node.Id == id)
                    return node;
                foreach (ModelNodeBase child in node.ChildNodes)
                    return GetType(id, child);
            }

            return null;
        }

        private ModelNodeBase GetType(string id, ModelNodeBase element)
        {
            if (element.Id == id)
                return element;
            foreach (ModelNodeBase child in element.ChildNodes)
                return GetType(id, child);
            return null;
        }
    }
}