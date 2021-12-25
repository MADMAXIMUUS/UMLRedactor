using System;
using System.Collections.Generic;

namespace UMLRedactor.Additions
{
    public struct Atribute
    {
        public string AccessModifier { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
    }

    public struct Operation
    {
        public string AccessModifier { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public List<Parametr> parametrs { get; set; }
    }

    public struct Parametr
    {
        public string DataType { get; set; }
        public string Name { get; set; }
        public string DefaultValue { get; set; }
    }
}
