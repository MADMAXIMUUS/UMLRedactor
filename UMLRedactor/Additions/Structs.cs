using System;
using System.Collections.Generic;

namespace UMLRedactor.Additions
{
    public struct Attribute
    {
        public string AccessModifier;
        public string Name;
        public string DataType;
    }

    public struct Operation
    {
        public string AccessModifier;
        public string Name;
        public string DataTypeOfReturnValue;
        public List<Parameter> Parameters;
    }

    public struct Parameter
    {
        public string DataType;
        public string Name;
        public string DefaultValue;
    }
}
