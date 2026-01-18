using System;

namespace OCPConsole
{
    /// <summary>
    /// Represents a regular pentagon shape
    /// </summary>
    public class Pentagon : Shape
    {
        public double SideLength { get; private set; }
        
        public Pentagon(double sideLength)
        {
            if (sideLength <= 0)
                throw new ArgumentException("Side length must be a positive value.");
                
            SideLength = sideLength;
        }
        
        public override double CalculateArea()
        {
            // Formula for regular pentagon: (1/4) * ?(25 + 10?5) * s²
            double coefficient = 0.25 * Math.Sqrt(25 + 10 * Math.Sqrt(5));
            return coefficient * SideLength * SideLength;
        }
        
        public override string ToString()
        {
            return $"Pentagon(SideLength: {SideLength})";
        }
    }
}
