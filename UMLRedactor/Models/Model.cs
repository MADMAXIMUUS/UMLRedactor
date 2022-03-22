namespace UMLRedactor.Models
{
    public class Model
    {
        public string Name;
        public string ProgramName;
        public string ProgramVersion;
        public string Author;
        public ModelNodeBase Root;

        public Model()
        {
            Root = new ModelNodeBase();
        }
        
        public Model(
            string name,
            string programName,
            string programVersion,
            string author,
            ModelNodeBase root)
        {
            Name = name;
            ProgramName = programName;
            ProgramVersion = programVersion;
            Author = author;
            Root = root;
        }
    }
}