namespace FacadePattern
{
    public class SubsystemA
    {
        public void OperationA()
        {
            Console.WriteLine("SubsystemA: OperationA executed.");
        }
    }

    // Subsystem 2
    public class SubsystemB
    {
        public void OperationB()
        {
            Console.WriteLine("SubsystemB: OperationB executed.");
        }
    }

    // Subsystem 3
    public class SubsystemC
    {
        public void OperationC()
        {
            Console.WriteLine("SubsystemC: OperationC executed.");
        }
    }

    // Subsystem 4
    public class SubsystemD
    {
        public void OperationD()
        {
            Console.WriteLine("SubsystemD: OperationD executed.");
        }
    }

    // Facade
    public class Facade
    {
        private SubsystemA _subsystemA;
        private SubsystemB _subsystemB;
        private SubsystemC _subsystemC;
        private SubsystemD _subsystemD;

        public Facade(SubsystemA subsystemA, SubsystemB subsystemB, 
            SubsystemC subsystemC, SubsystemD subsystemD)
        {
            _subsystemA = subsystemA;
            _subsystemB = subsystemB;
            _subsystemC = subsystemC;
            _subsystemD = subsystemD;
        }

        public void PerformOperation1()
        {
            Console.WriteLine("Operation 1\n" +
                              "-----------");
            _subsystemA.OperationA();
            _subsystemB.OperationB();
            _subsystemC.OperationC();
        }

        public void PerformOperation2()
        {
            Console.WriteLine("Operation 2\n" +
                              "-----------");
            _subsystemA.OperationA();
            _subsystemD.OperationD();
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            // Create subsystems
            var subsystemA = new SubsystemA();
            var subsystemB = new SubsystemB();
            var subsystemC = new SubsystemC();
            var subsystemD = new SubsystemD();

            // Create facade
            var facade = new Facade(subsystemA, subsystemB, subsystemC, subsystemD);

            // Use facade to perform operations
            facade.PerformOperation1();
            Console.WriteLine();
            facade.PerformOperation2();
        }
    }
}
