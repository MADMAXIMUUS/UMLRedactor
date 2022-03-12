using System;
using System.Collections.Generic;

namespace UMLRedactor.Additions
{
    public struct Attribute
    {
        public string AccessModifier { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
    }

    public struct Operation
    {
        public string AccessModifier { get; set; }
        public string Name { get; set; }
        public string DataTypeOfReturnValue { get; set; }
        public List<Parametr> Parametrs { get; set; }
    }

    public struct Parametr
    {
        public string DataType { get; set; }
        public string Name { get; set; }
        public string DefaultValue { get; set; }
    }
}
