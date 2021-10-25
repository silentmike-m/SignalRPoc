using System;

namespace Client
{
    using System.Threading.Tasks;

    static class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                if (args.Length != 3)
                {
                    throw new ArgumentOutOfRangeException();
                }

                await new SignalRClient().StartConnection(args[0], args[1], args[2]);

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}
