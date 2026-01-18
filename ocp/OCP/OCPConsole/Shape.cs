using System;

namespace OCPConsole
{
    /// <summary>
    /// Abstract base class for all geometric shapes
    /// Defines the contract that all shapes must follow
    /// </summary>
    public abstract class Shape
    {
        /// <summary>
        /// Calculate the area of the shape
        /// </summary>
        /// <returns>The area as a double value</returns>
        public abstract double CalculateArea();
        
        /// <summary>
        /// Get the name of the shape (optional: for display purposes)
        /// </summary>
        public virtual string GetShapeName() => this.GetType().Name;
    }
}
