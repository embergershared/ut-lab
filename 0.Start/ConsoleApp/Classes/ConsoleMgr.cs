using ConsoleApp.Interfaces;

namespace ConsoleApp.Classes
{
    public class ConsoleMgr : IConsoleMgr
    {
        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        public void Write(string value)
        {
            Console.Write(value);
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }

        public string? ReadLine()
        {
            return Console.ReadLine();
        }

        public void Clear()
        {
            Console.Clear();
            //throw new NotImplementedException();
        }
    }
}
