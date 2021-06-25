using System;

namespace Client
{
    using System.Threading.Tasks;

    static class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 2)
                await new SignalRClient().StartConnection(args[0], args[1]);
            else
                await new SignalRClient().StartConnection("User1", "P@ssword!");


            Console.ReadKey();
        }
    }
}
