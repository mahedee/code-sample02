namespace ObserverPattern
{
    // IObserver interface is used to update all the observers
    public interface IObserver
    {
        // Update method is called when the subject changes
        void Update(string state);
    }

    // Interface for the subject
    // It has methods to attach, detach and notify the observers
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }

    // Concreate Subject class
    public class ConcreateSubject : ISubject
    {
        // List of observers attached to the subject
        private List<IObserver> _observers = new List<IObserver>();
        private string _subjectState;

        public string SubjectState
        {
            get => _subjectState;

            // When the state changes of subject, notify all the observers
            set
            {
                _subjectState = value;
                // Call Notify method to notify all the observers
                Notify(); 
            }
        }

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        // Notify all the observers
        public void Notify()
        {
            // Call Update method of all the observers attached to the subject
            foreach (var observer in _observers)
            {
                observer.Update(_subjectState);
            }
        }
    }

    // Concrete Observer class
    public class ConcreateObserver : IObserver
    {
        // Used to set the name of the observer
        private string _observerName;

        public ConcreateObserver(string name)
        {
            _observerName = name;
        }
        public void Update(string state)
        {
            Console.WriteLine($"{_observerName} received update: State changed to {state}");
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            // Create a subject
            ConcreateSubject subject = new ConcreateSubject();

            // Create observers
            IObserver observer1 = new ConcreateObserver("Observer-1");
            IObserver observer2 = new ConcreateObserver("Observer-2");
            IObserver observer3 = new ConcreateObserver("Observer-3");

            // Attach the observers to the subject
            subject.Attach(observer1);
            subject.Attach(observer2);
            subject.Attach(observer3);

            // Change the state of the subject
            Console.WriteLine("Changing state to State 1");
            subject.SubjectState = "State-1";

            Console.WriteLine("Changing state to State 2");
            subject.SubjectState = "State-2";

            // Detach observer 1
            subject.Detach(observer1);

            Console.WriteLine("Changing state to State 3");
            subject.SubjectState = "State-3";

            Console.ReadLine();
        }
    }
}
