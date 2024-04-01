namespace RaiCLI.Core.CommandClasses
{
    public class Simula : IRaiCLI
    {
        public void Invoke(string[] args)
        {
            Console.WriteLine("Command invoked");
        }

        public string Usage()
        {
            return "This is help for Simula";
        }
    }
}
