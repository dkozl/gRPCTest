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

            var reply = await client.SayHelloAsync(
                new HelloRequest
                {
                    Name = "GreeterClient"
                });

            Console.WriteLine("Greeting: " + reply.Message);

            var randomStream = client.Random(
                new RandomRequest
                {
                    Count = 10,
                    MinValue = 100,
                    MaxValue = 200
                }).ResponseStream;

            var ct = _cancellationTokenSource.Token;

            while (await randomStream.MoveNext(ct))
            {
                Console.WriteLine($"Random number: {randomStream.Current.Value}");
            }

            using (var addClient = client.Add())
            {
                while (true)
                {
                    Console.Write("Enter number (0 to exit): ");
                    var value = int.Parse(Console.ReadLine());
                    if (value == 0)
                        break;

                    await addClient.RequestStream.WriteAsync(new AddRequest {Value = value});
                }
                await addClient.RequestStream.CompleteAsync();
                var response = await addClient.ResponseAsync;
                Console.WriteLine($"Total: {response.Value}");
            }

            await channel.ShutdownAsync();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
