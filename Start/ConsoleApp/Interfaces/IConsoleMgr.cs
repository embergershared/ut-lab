namespace ConsoleApp.Interfaces
{
    public interface IConsoleMgr
    {
        void WriteLine(string value);
        void Write(string value);
        ConsoleKeyInfo ReadKey();
        string? ReadLine();
        void Clear();
    }
}
