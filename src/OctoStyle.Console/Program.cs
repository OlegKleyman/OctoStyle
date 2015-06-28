namespace OctoStyle.Console
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            Arguments arguments;

            try
            {
                arguments = Arguments.Parse(args);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }
    }
}
