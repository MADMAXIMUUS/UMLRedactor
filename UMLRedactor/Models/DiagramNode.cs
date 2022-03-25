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
            
        }
        
        public DiagramNode(
            int x1, 
            int x2, 
            int y1, 
            int y2, 
            string id, 
            int width, 
            int height)
        {
            X1 = x1;
            X2 = x2;
            Y1 = y1;
            Y2 = y2;
            Id = id;
            Width = width;
            Height = height;
        }
    }
}