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

// Create a channel and declare the exchange
using var channel = await connection.CreateChannelAsync();

// --- FANOUT EXCHANGE IMPLEMENTATION ---

await channel.ExchangeDeclareAsync(exchange: "direct_exchange", type: "fanout", autoDelete: false, arguments: null);

// Publish messages to the exchange with for loop to simulate multiple events
for (int i = 0; i < 10; i++)
{
    var message = $"📣 Fanout message 👋{i + 1} at {DateTime.Now}";

    var messageBody = JsonSerializer.Serialize(message);
    var body = Encoding.UTF8.GetBytes(messageBody);

    await channel.BasicPublishAsync(exchange: "fanout_exchange", routingKey: String.Empty, body: body);
    Console.WriteLine($"Published event: {message}");

    await Task.Delay(5000);
}

Console.WriteLine("Producer finished.");

// --- DIRECT EXCHANGE IMPLEMENTATION ---

// await channel.ExchangeDeclareAsync(exchange: "topic_exchange", type: "topic", autoDelete: false, arguments: null);

// // Publish messages to the exchange with for loop to simulate multiple events
// for (int i = 0; i < 30; i++)
// {
//     var message = $"📣 Topic message 👋{i + 1} at {DateTime.Now}";

//     var messageBody = JsonSerializer.Serialize(message);
//     var body = Encoding.UTF8.GetBytes(messageBody);

//     if (i % 2 == 0)
//     {
//         await channel.BasicPublishAsync(exchange: "topic_exchange", routingKey: "idem.public", body: body);
//     }
//     else
//     {
//         await channel.BasicPublishAsync(exchange: "topic_exchange", routingKey: "idem.public.reply.mention", body: body);
//     }


//     Console.WriteLine($"Published event: {message}");

//     await Task.Delay(5000);
// }

// Console.WriteLine("Producer finished.");

// --- HEADER EXCHANGE IMPLEMENTATION ---

// await channel.ExchangeDeclareAsync(exchange: "headers_exchange", type: "headers", autoDelete: false, arguments: null);

// // Publish messages to the exchange with for loop to simulate multiple events
// for (int i = 0; i < 30; i++)
// {
//     var message = $"📣 Header message 👋{i + 1} at {DateTime.Now}";

//     var messageBody = JsonSerializer.Serialize(message);
//     var body = Encoding.UTF8.GetBytes(messageBody);



//     if (i % 2 == 0)
//     {
//         var properties1 = new BasicProperties
//         {
//             Headers = new Dictionary<string, object>
//         {
//             { "author", "alice" },
//             { "visibility", "public" }
//         }
//         };

//         await channel.BasicPublishAsync(
//             exchange: "headers_exchange",
//             routingKey: "",
//             mandatory: false,
//             basicProperties: properties1,
//             body: body
//         );
//     }
//     else if (i % 3 == 0)
//     {
//         var properties2 = new BasicProperties
//         {
//             Headers = new Dictionary<string, object>
//         {
//             { "author", "alice" },
//             { "visibility", "private" }
//         }
//         };

//         await channel.BasicPublishAsync(
//             exchange: "headers_exchange",
//             routingKey: "",
//             mandatory: false,
//             basicProperties: properties2,
//             body: body
//         );

//     }
//     else
//     {
//         var properties3 = new BasicProperties
//         {
//             Headers = new Dictionary<string, object>
//         {
//             { "author", "bob" },
//             { "visibility", "draft" }
//         }
//         };

//         await channel.BasicPublishAsync(
//             exchange: "headers_exchange",
//             routingKey: "",
//             mandatory: false,
//             basicProperties: properties3,
//             body: body
//         );
//     }


//     Console.WriteLine($"Published event: {message}");

//     await Task.Delay(2000);
// }

// Console.WriteLine("Producer finished.");