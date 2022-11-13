using ConsoleApp.Interfaces;

namespace ConsoleApp.Classes
{
    public class ProgramMgr : IProgramMgr
    {
        private readonly IConsoleMgr _consoleMgr;
        private const string Tab = "   ";

        public ProgramMgr(IConsoleMgr consoleMgr)
        {
            _consoleMgr = consoleMgr;
        }
        public void Run()
        {
            _consoleMgr.WriteLine("ConsoleApp program started");
            _consoleMgr.WriteLine("");

            string? input;
            
            do
            {
                _consoleMgr.WriteLine($"{Tab}Enter any text and it will be repeated");
                _consoleMgr.WriteLine("");

                _consoleMgr.WriteLine($"{Tab}Press Enter to clear the screen");
                _consoleMgr.WriteLine($"{Tab}Enter \"q\" to quit the program");
                _consoleMgr.WriteLine("");

                _consoleMgr.Write("Please enter a choice: ");

                input = _consoleMgr.ReadLine();

                if (!string.IsNullOrEmpty(input))
                {
                    _consoleMgr.WriteLine($"{Tab}You entered: {input}");
                    _consoleMgr.WriteLine("");
                }
                else
                {
                    _consoleMgr.Clear();
                }
            } while (input != "q");
        }
    }
}
