namespace ConsoleApp.Classes
{
    public class Calculator
    {
        public double Add(double a, double b)
        {
            return a + b;
        }

        public double Divide(double a, double b)
        {
            if (b != 0)
            {
                return a / b;
            }

            // Arbitrary decision to handle division by 0
            return 0;
        }
    }
}
