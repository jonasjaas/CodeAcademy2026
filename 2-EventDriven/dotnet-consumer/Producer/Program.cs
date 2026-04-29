using CodeAcademy.DotnetConsumer.Common.Config;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

Console.WriteLine("Producer starting...");
// Establish connection to RabbitMQ
using var connection = await ConnectionHelper.ConnectAsync();
Console.WriteLine("Connected to RabbitMQ");

// Implement a basic producer here.
// Start with:
// - Create a channel
// - Declare a queue
// - Publish a message to the queue (you can use a simple JSON string as the message body)


// Create a channel and declare the queue
using var channel = await connection.CreateChannelAsync();
await channel.ExchangeDeclareAsync(exchange: "chat", type: "fanout", autoDelete: false, arguments: null);
await channel.QueueDeclareAsync(queue: "chat_jonas", durable: true, autoDelete: false, arguments: null);
await channel.QueueBindAsync(queue: "chat_jonas", exchange: "chat", routingKey: "", arguments: null);

// Publish messages to the queue with for loop to simulate multiple events
for (int i = 0; i < 10; i++)
{
    var message = $"Idems Event from Jonas 👋{i + 1} at {DateTime.Now}";

    var messageBody = JsonSerializer.Serialize(message);
    var body = Encoding.UTF8.GetBytes(messageBody);

    await channel.BasicPublishAsync(exchange: "chat", routingKey: String.Empty, body: body);
    Console.WriteLine($"Published event from Jonas 👋: {message}");

    await Task.Delay(2000);
}

Console.WriteLine("Producer finished.");