using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UMLRedactor.Additions;
using UMLRedactor.Models;

namespace UMLRedactor.Controllers
{
    public class ModelReader
    {
        private static XDocument XmlDocument { get; set; }

        public ModelReader(string path)
        {
            XmlDocument = XDocument.Load(path);
        }

        public int GetModelFromFile(out Model model)
        {
            model = new Model();
            XElement xElementRoot = XmlDocument.Root;
            if (xElementRoot == null)
                return -1;
            if (xElementRoot.Element("XMI.header")?
                    .Element("XMI.documentation")?
                    .Element("XMI.exporter")?
                    .Value != "MadUML")
                return GetModelFromEaFile(out model);

            return GetModelFromMadFile(out model);
        }

        private int GetModelFromMadFile(out Model model)
        {
            model = new Model();
            XElement xElementRoot = XmlDocument.Root;

            if (xElementRoot == null)
                return -2;
            XElement xElementDocumentationRoot = xElementRoot.Element("XMI.header")?.Element("XMI.documentation");
            if (xElementDocumentationRoot != null)
            {
                model.ProgramName = xElementDocumentationRoot.Element("XMI.exporter")?.Value;
                model.ProgramVersion = xElementDocumentationRoot.Element("XMI.exporterVersion")?.Value;
                model.Name = xElementRoot.Element("XMI.content")?.Element(Uml("Model"))?.Attribute("name")?.Value;
            }

            XElement xContent = xElementRoot.Element("XMI.content");
            if (xContent != null)
            {
                model.Root = new ModelNodeBase
                {
                    Name = xContent.Element(Uml("Model"))?
                        .Element(Uml("Package"))?
                        .Element(Uml("ModelElement.rootElement"))?
                        .Element("Name")?
                        .Value,
                    Type = GetElementType(xContent.Element(Uml("Model"))?.Element(Uml("Class"))),
                    Id = xContent.Element(Uml("Model"))?
                        .Element(Uml("Package"))?
                        .Element(Uml("ModelElement.rootElement"))?
                        .Element("ID")?
                        .Value,
                    ChildNodes = new List<ModelNodeBase>()
                };
                model.Root.Namespace.PackageId = xContent.Element(Uml("Model"))?
                    .Element(Uml("Package"))?
                    .Element(Uml("ModelElement.rootElement"))?
                    .Element("Namespace")?
                    .Value;
                model.Root.Namespace.PackageName = xContent.Element(Uml("Model"))?
                    .Element(Uml("Package"))?
                    .Attribute("name")?
                    .Value;


                List<XElement> elements = xContent.Element(Uml("Model"))?
                    .Element(Uml("Package"))?
                    .Element(Uml("Namespace.ownedElement"))?
                    .Elements().ToList();

                if (elements != null)
                    foreach (var element in elements)
                    {
                        if (GetElementType(element) == "Not element" && GetLineType(element) == "Not line")
                            continue;

                        if (GetElementType(element) != "Not element")
                            model.Root.ChildNodes.Add(GetElementFromMad(element));
                        else
                            model.Root.ChildNodes.Add(GetLineFromMad(element));
                    }
            }

            return 0;
        }

        private int GetModelFromEaFile(out Model model)
        {
            model = new Model();
            XElement xElementRoot = XmlDocument.Root;
            if (xElementRoot == null)
                return -2;
            if (xElementRoot.FirstAttribute?.Value != "1.1")
                return -1;
            XElement xElementDocumentationRoot = xElementRoot.Element("XMI.header")?.Element("XMI.documentation");
            if (xElementDocumentationRoot == null)
                return -2;

            model.ProgramName = xElementDocumentationRoot.Element("XMI.exporter")?.Value;
            model.ProgramVersion = xElementDocumentationRoot.Element("XMI.exporterVersion")?.Value;
            model.Name = xElementRoot.Element("XMI.content")?.Element(Uml("Model"))?.Attribute("name")?.Value;

            XElement xModelRoot = xElementRoot.Element("XMI.content")?
                .Element(Uml("Model"))?
                .Element(Uml("Namespace.ownedElement"));

            if (xModelRoot == null)
                return -2;

            model.Root = new ModelNodeBase
            {
                Name = xModelRoot.Element(Uml("Class"))?.Attribute("name")?.Value,
                Id = xModelRoot.Element(Uml("Class"))?.Attribute("xmi.id")?.Value,
                ChildNodes = new List<ModelNodeBase>()
            };
            model.Root.Namespace.PackageName = xModelRoot.Element(Uml("Package"))?.Attribute("name")?.Value;
            model.Root.Namespace.PackageId = xModelRoot.Element(Uml("Package"))?.Attribute("xmi.id")?.Value;

            List<XElement> elements = xModelRoot.Element(Uml("Package"))?
                .Element(Uml("Namespace.ownedElement"))?
                .Elements().ToList();
            if (elements == null)
                return -2;
            foreach (XElement element in elements)
            {
                if (element.Name.LocalName == "ActivityModel")
                {
                    List<XElement> activityTransition = element.Element(Uml("StateMachine.transitions"))?
                        .Elements().ToList();

                    if (activityTransition != null)
                        foreach (XElement transition in activityTransition)
                        {
                            if (GetLineType(transition) == "Not line")
                                continue;

                            ModelNodeLine mes = new ModelNodeLine
                            {
                                Id = transition.Attribute("xmi.id")?.Value,
                                Type = GetLineType(transition),
                                Source = transition.Attribute("source")?.Value,
                                Target = transition.Attribute("target")?.Value,
                            };

                            model.Root.ChildNodes.Add(mes);
                        }

                    List<XElement> activityState = element.Element(Uml("StateMachine.top"))?
                        .Element(Uml("CompositeState"))?
                        .Element(Uml("CompositeState.substate"))?
                        .Elements().ToList();

                    if (activityState != null)
                        foreach (XElement state in activityState)
                        {
                            if (GetElementType(state) == "Not element")
                                continue;

                            model.Root.ChildNodes.Add(GetElementFromEa(state));
                        }
                }
                else if (element.Element(Uml("Collaboration.interaction")) != null)
                {
                    List<XElement> seqElements = element.Element(Uml("Namespace.ownedElement"))?
                        .Elements().ToList();
                    if (seqElements != null)
                        foreach (XElement collaboration in seqElements)
                        {
                            ModelNodeElement col = new ModelNodeElement
                            {
                                Name = collaboration.Attribute("name")?.Value,
                                Id = collaboration.Attribute("xmi.id")?.Value,
                                Type = GetElementType(collaboration),
                                Namespace = new Package
                                {
                                    PackageId = collaboration.Attribute("base")?.Value,
                                    PackageName = ""
                                },
                            };

                            model.Root.ChildNodes.Add(col);
                        }

                    List<XElement> seqMessages = element.Element(Uml("Collaboration.interaction"))?
                        .Element(Uml("Interaction"))?
                        .Element(Uml("Interaction.message"))?
                        .Elements().ToList();

                    if (seqMessages != null)
                        foreach (XElement message in seqMessages)
                        {
                            ModelNodeLine mes = new ModelNodeLine
                            {
                                Name = message.Attribute("name")?.Value,
                                Id = message.Attribute("xmi.id")?.Value,
                                Type = GetLineType(message),
                                Source = message.Attribute("sender")?.Value,
                                Target = message.Attribute("receiver")?.Value,
                                TextOnLine = message.Attribute("name")?.Value
                            };

                            model.Root.ChildNodes.Add(mes);
                        }
                }
                else
                {
                    if (GetElementType(element) == "Not element" && GetLineType(element) == "Not line")
                        continue;

                    if (GetElementType(element) != "Not element")
                        model.Root.ChildNodes.Add(GetElementFromEa(element));
                    else
                        model.Root.ChildNodes.Add(GetLineFromEa(element));
                }
            }

            return 0;
        }

        private string GetElementType(XElement element)
        {
            switch (element.Name.LocalName)
            {
                case "Class":
                    return "Class";
                case "Package":
                    return "Package";
                case "ActionState":
                    return "Activity";
                case "PseudoState":
                    return "Pseudo";
                case "Actor":
                    return "Actor";
                case "UseCase":
                    return "UseCase";
                case "Message":
                    return "Message";
                case "ClassifierRole":
                    return "SeqObject";
                case "Comment":
                    return "Comment";
                default:
                    return "Not element";
            }
        }

        private string GetLineType(XElement element)
        {
            switch (element.Name.LocalName)
            {
                case "Association":
                    return "Association";
                case "Generalization":
                    return "Generalization";
                case "Message":
                    return "Message";
                case "Transition":
                    return "Transition";
                default:
                    return "Not line";
            }
        }

        private ModelNodeElement GetElementFromEa(XElement element)
        {
            ModelNodeElement elem = new ModelNodeElement()
            {
                Name = element.Attribute("name")?.Value,
                Id = element.Attribute("xmi.id")?.Value,
                Type = GetElementType(element),
                Stereotype = element.Element(Uml("ModelElement.stereotype"))?.Element(Uml("Stereotype"))?
                    .Attribute("name")?.Value,
                ChildNodes = new List<ModelNodeBase>(),
                Attributes = new List<Attribute>(),
                Operations = new List<Operation>()
            };

            List<XElement> taggedValue = element.Element(Uml("ModelElement.taggedValue"))?.Elements().ToList();
            if (taggedValue != null)
                foreach (XElement value in taggedValue)
                {
                    if (elem.Type == "Package")
                    {
                        if (value.Attribute("tag")?.Value == "parent")
                        {
                            elem.Namespace.PackageId = value.Attribute("value")?.Value;
                        }
                    }
                    else
                    {
                        if (value.Attribute("tag")?.Value == "package")
                        {
                            elem.Namespace.PackageId = value.Attribute("value")?.Value;
                        }
                    }

                    if (string.IsNullOrEmpty(elem.Stereotype) && (elem.Type=="Activity" || elem.Type=="Pseudo"))
                        elem.Stereotype = value.Attribute("tag")?.Value == "ea_stype"
                            ? value.Attribute("value")?.Value
                            : "";
                }

            List<XElement> classifiers = element.Element(Uml("Classifier.feature"))?.Elements().ToList();
            if (classifiers != null)
            {
                foreach (XElement classifier in classifiers)
                {
                    if (classifier.Name.LocalName == "Attribute")
                        elem.Attributes.Add(GetAttributeFromEa(classifier));
                    if (classifier.Name.LocalName == "Operation")
                        elem.Operations.Add(GetOperationFromEa(classifier));
                }
            }

            List<XElement> children = element.Element(Uml("Namespace.ownedElement"))?.Elements().ToList();
            if (children != null)
                foreach (XElement child in children)
                {
                    if (GetElementType(child) == "Not element" && GetLineType(child) == "Not line")
                        continue;

                    if (GetElementType(child) != "Not element")
                        elem.ChildNodes.Add(GetElementFromEa(child));
                    else
                        elem.ChildNodes.Add(GetLineFromEa(child));
                }

            return elem;
        }

        private Attribute GetAttributeFromEa(XElement classifier)
        {
            Attribute at = new Attribute()
            {
                Name = classifier.Attribute("name")?.Value,
                AccessModifier = classifier.Attribute("visibility")?.Value
            };
            List<XElement> taggedValue = classifier.Element(Uml("ModelElement.taggedValue"))?.Elements().ToList();
            if (taggedValue != null)
                foreach (XElement value in taggedValue)
                {
                    if (value.Attribute("tag")?.Value == "type")
                    {
                        at.DataType = value.Attribute("value")?.Value;
                        break;
                    }
                }

            return at;
        }

        private Operation GetOperationFromEa(XElement classifier)
        {
            Operation op = new Operation()
            {
                Name = classifier.Attribute("name")?.Value,
                AccessModifier = classifier.Attribute("visibility")?.Value,
                Parameters = new List<Parameter>()
            };
            List<XElement> taggedValue = classifier.Element(Uml("ModelElement.taggedValue"))?.Elements().ToList();
            if (taggedValue != null)
                foreach (XElement value in taggedValue)
                {
                    if (value.Attribute("tag")?.Value != "type")
                        continue;
                    op.DataTypeOfReturnValue = value.Attribute("value")?.Value;
                    break;
                }

            List<XElement> parameters = classifier.Element(Uml("BehavioralFeature.parameter"))?.Elements().ToList();
            if (parameters != null)
                foreach (XElement parameter in parameters)
                {
                    if (parameter.Attribute("kind")?.Value == "return")
                        continue;
                    op.Parameters.Add(GetParameterFromEa(parameter));
                }

            return op;
        }

        private Parameter GetParameterFromEa(XElement parameter)
        {
            Parameter p = new Parameter
            {
                Name = parameter.Attribute("name")?.Value,
                DefaultValue = parameter.Element(Uml("Parameter.defaultValue"))?
                    .Element(Uml("Expression"))?
                    .Attribute("body")?.Value
            };
            List<XElement> taggedValue = parameter.Element(Uml("ModelElement.taggedValue"))?.Elements().ToList();
            if (taggedValue != null)
                foreach (XElement value in taggedValue)
                {
                    if (value.Attribute("tag")?.Value == "type")
                    {
                        p.DataType = value.Attribute("value")?.Value;
                        break;
                    }
                }

            return p;
        }

        private ModelNodeLine GetLineFromEa(XElement element)
        {
            ModelNodeLine line = new ModelNodeLine
            {
                Name = GetLineType(element),
                Id = element.Attribute("xmi.id")?.Value,
                Type = GetLineType(element),
                TextOnLine = element.Attribute("name")?.Value
            };
            List<XElement> ends = element.Element(Uml("Association.connection"))?.Elements().ToList();
            if (ends != null)
                foreach (XElement end in ends)
                {
                    List<XElement> taggedValue = end.Element(Uml("ModelElement.taggedValue"))?.Elements().ToList();
                    if (taggedValue != null)
                        foreach (XElement value in taggedValue)
                        {
                            if (value.Attribute("tag")?.Value != "ea_end")
                                continue;

                            if (value.Attribute("value")?.Value == "source")
                            {
                                line.Source = end.Attribute("type")?.Value;
                                line.TextSourceOnLine = end.Attribute("name")?.Value;
                            }
                            else
                            {
                                line.Target = end.Attribute("type")?.Value;
                                line.TextTargetOnLine = end.Attribute("name")?.Value;
                            }
                        }
                }

            return line;
        }

        private ModelNodeElement GetElementFromMad(XElement element)
        {
            ModelNodeElement elem = new ModelNodeElement
            {
                Name = element.Element("Name")?.Value,
                Id = element.Element("ID")?.Value,
                Stereotype = element.Element("Stereotype")?.Value,
                Type = GetElementType(element),
                Attributes = new List<Attribute>(),
                Operations = new List<Operation>(),
                ChildNodes = new List<ModelNodeBase>()
            };
            elem.Namespace.PackageId = element.Element("Namespace")?.Value;
            List<XElement> classifierFeatures = element.Element(Uml("Classifier.feature"))?.Elements().ToList();
            if (classifierFeatures != null)
                foreach (XElement classifierFeature in classifierFeatures)
                {
                    if (classifierFeature.Name.LocalName == "Attribute")
                        elem.Attributes.Add(GetAttributeFromMad(classifierFeature));
                    if (classifierFeature.Name.LocalName == "Operation")
                        elem.Operations.Add(GetOperationFromMad(classifierFeature));
                }

            return elem;
        }

        private Operation GetOperationFromMad(XElement element)
        {
            Operation op = new Operation
            {
                Name = element.Element("Name")?.Value,
                AccessModifier = element.Element("Access")?.Value,
                DataTypeOfReturnValue = element.Element("ReturnType")?.Value,
                Parameters = new List<Parameter>()
            };
            List<XElement> parameters = element.Element(Uml("BehavioralFeature.parameter"))?.Elements().ToList();
            if (parameters != null)
                foreach (XElement par in parameters)
                {
                    op.Parameters.Add(new Parameter()
                    {
                        Name = par.Element("Name")?.Value,
                        DataType = par.Element("DataType")?.Value,
                        DefaultValue = par.Element("DefaultValue")?.Value
                    });
                }

            return op;
        }

        private Attribute GetAttributeFromMad(XElement element)
        {
            Attribute at = new Attribute()
            {
                Name = element.Element("Name")?.Value,
                AccessModifier = element.Element("Access")?.Value,
                DataType = element.Element("DataType")?.Value
            };
            return at;
        }

        private ModelNodeLine GetLineFromMad(XElement element)
        {
            ModelNodeLine line = new ModelNodeLine
            {
                Name = element.Element("Name")?.Value,
                Id = element.Element("ID")?.Value,
                ChildNodes = null,
                Source = element.Element("Source")?.Value,
                Target = element.Element("Target")?.Value,
                Type = GetLineType(element),
                TextOnLine = element.Element("TextOnLine")?.Value,
                TextSourceOnLine = element.Element("TextSourceOnLine")?.Value,
                TextTargetOnLine = element.Element("TextTargetOnLine")?.Value
            };
            line.Namespace.PackageId = element.Element("Namespace")?.Value;
            return line;
        }

        static XName Uml(string name) => XNamespace.Get("omg.org/UML1.3") + name;
    }
}