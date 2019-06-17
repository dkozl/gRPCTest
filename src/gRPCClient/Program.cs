using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Workshop;

namespace gRPCClient
{
    class Program
    {
        static CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        static async Task Main(string[] args)
        {
            var channel = new Channel("localhost:5010", ChannelCredentials.Insecure);
            var client = new Worker.WorkerClient(channel);
            var ct = _cancellationTokenSource.Token;

            



            await channel.ShutdownAsync();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
