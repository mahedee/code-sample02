using System;

namespace OCPConsole
{
    /// <summary>
    /// Represents a rectangle shape
    /// </summary>
    public class Rectangle : Shape
    {
        public double Height { get; private set; }
        public double Width { get; private set; }
        
        public Rectangle(double height, double width)
        {
            if (height <= 0 || width <= 0)
                throw new ArgumentException("Height and width must be positive values.");
                
            Height = height;
            Width = width;
        }
        
        public override double CalculateArea()
        {
            return Height * Width;
        }
        
        public override string ToString()
        {
            return $"Rectangle(Width: {Width}, Height: {Height})";
        }
    }
}
