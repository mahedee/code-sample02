using System;
using System.Collections.Generic;

namespace OCPConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Area Calculator using Open-Close Principle ===");
            Console.WriteLine();
            
            // Create a list of different shapes
            List<Shape> shapes = new List<Shape>
            {
                new Rectangle(20, 30),
                new Triangle(15, 25),
                new Circle(7),
                new Pentagon(10) // New shape added without modifying existing code!
            };
            
            // Calculate areas using polymorphism
            foreach (Shape shape in shapes)
            {
                try
                {
                    double area = shape.CalculateArea();
                    Console.WriteLine($"{shape} => Area: {area:F2}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error calculating area for {shape.GetShapeName()}: {ex.Message}");
                }
            }
            
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
