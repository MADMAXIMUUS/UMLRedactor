namespace UMLRedactor.Models
{
    public class DiagramNode
    {
        public double X1, X2, Y1, Y2;
        public string Id;
        public string ModelElementId;
        public double Width;
        public double Height;

        public DiagramNode()
        {
            X1 = X2 = Y1 = Y2 = 0;
            Id = "";
            ModelElementId = "";
            Width = 0;
            Height = 0;
        }
        
    }
}