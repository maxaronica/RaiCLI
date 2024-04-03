namespace RaiCLI.Core.CommandClasses
{
    public class Simula : IRaiCLI
    {
        public Simula(IServiceProvider sp)
        {

        }
        public void Invoke(string[] args)
        {
            Console.WriteLine("Command invoked............................");
        }

        public string Usage()
        {
            return "Simula: This is help for Simula";
        }
    }
}
