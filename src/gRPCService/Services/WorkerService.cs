using System;
using System.Threading.Tasks;
using Grpc.Core;
using Workshop;

namespace gRPCService.Services
{
    public class WorkerService : Worker.WorkerBase
    {
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(
                new HelloReply
                {
                    Message = "Hello " + request.Name
                });
        }

        public override async Task<AddResponse> Add(IAsyncStreamReader<AddRequest> requestStream, ServerCallContext context)
        {
            var total = 0;

            while (await requestStream.MoveNext(context.CancellationToken))
            {
                total += requestStream.Current.Value;
            }

            return new AddResponse
            {
                Value = total
            };
        }

        public override async Task Random(RandomRequest request, IServerStreamWriter<RandomResponse> responseStream, ServerCallContext context)
        {
            var random = new Random();
            for (var i = 0; i < request.Count; i++)
            {
                await responseStream.WriteAsync(
                    new RandomResponse
                    {
                        Value = random.Next(request.MinValue, request.MaxValue)
                    });
                await Task.Delay(2000);
            }
        }
    }
}