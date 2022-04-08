﻿namespace UMLRedactor.Models
{
    public class Model
    {
        public string Name;
        public string ProgramName;
        public string ProgramVersion;
        public ModelNodeBase Root;

        public Model()
        {
            Name = "";
            ProgramName = "MadUML";
            ProgramVersion = "0.5";
            Root = new ModelNodeBase();
        }
    }
}