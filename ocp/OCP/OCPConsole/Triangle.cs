using System;

namespace OCPConsole
{
    /// <summary>
    /// Represents a triangle shape
    /// </summary>
    public class Triangle : Shape
    {
        public double Base { get; private set; }
        public double Height { get; private set; }
        
        public Triangle(double baseLength, double height)
        {
            if (baseLength <= 0 || height <= 0)
                throw new ArgumentException("Base and height must be positive values.");
                
            Base = baseLength;
            Height = height;
        }
        
        public override double CalculateArea()
        {
            return 0.5 * Base * Height;
        }
        
        public override string ToString()
        {
            return $"Triangle(Base: {Base}, Height: {Height})";
        }
    }
}
