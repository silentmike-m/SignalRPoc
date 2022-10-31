using System;

namespace Client
{
    using System.Threading.Tasks;

    static class Program
    {
        static async Task Main(string[] args)
        {
            Console.ReadLine();

            try
            {
                //if (args.Length != 3)
                //{
                //    throw new ArgumentOutOfRangeException();
                //}

                await new SignalRClient().StartConnection("User2", "P@ssword!", "company1");

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
