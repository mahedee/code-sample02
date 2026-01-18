using System;

namespace OCPConsole
{
    /// <summary>
    /// Represents a circle shape
    /// </summary>
    public class Circle : Shape
    {
        public double Radius { get; private set; }
        
        public Circle(double radius)
        {
            if (radius <= 0)
                throw new ArgumentException("Radius must be a positive value.");
                
            Radius = radius;
        }
        
        public override double CalculateArea()
        {
            return Math.PI * Radius * Radius;
        }
        
        public override string ToString()
        {
            return $"Circle(Radius: {Radius})";
        }
    }
}
