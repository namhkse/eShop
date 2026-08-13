


using Basket.API;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("http://localhost:5111");
var clietn = new Greeter.GreeterClient(channel);
var reply = await clietn.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });

Console.WriteLine(reply.Message);

Console.ReadKey();
