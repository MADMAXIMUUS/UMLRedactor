using System;

namespace UMLRedactor.Models
{
    public class DiagramNode
    {
        public int X1, X2, Y1, Y2;
        public string Id;
        public int Width;
        public int Height;

        public DiagramNode()
        {
            X1 = X2 = Y1 = Y2 = 0;
            Id = "";
            Width = 0;
            Height = 0;
        }
        
    }
}