using System;

namespace UMLRedactor.Additions
{
    public class MoveLineEventArgs: EventArgs
    {
        public MoveLineEventArgs(double newX, double newY, double width, double height)
        {
            NewX = newX;
            NewY = newY;
            Width = width;
            Height = height;
        }
        
        public double NewX;
        public double NewY;
        public double Width;
        public double Height;
    }
}