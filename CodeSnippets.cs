//=====[ Hello ]=========

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(
                new HelloReply
                {
                    Message = "Hello " + request.Name
                });
        }
        
//-----------------------

            var reply = await client.SayHelloAsync(
                new HelloRequest
                {
                    Name = "GreeterClient"
                });

            Console.WriteLine("Greeting: " + reply.Message);

//=====[ Add ]===========

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

//-----------------------
        
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

//=====[ Random ]========

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
        
//-----------------------

            var randomStream = client.Random(
                new RandomRequest
                {
                    Count = 5,
                    MinValue = 100,
                    MaxValue = 200
                }).ResponseStream;
            
            while (await randomStream.MoveNext(ct))
            {
                Console.WriteLine($"Random number: {randomStream.Current.Value}");
            }
