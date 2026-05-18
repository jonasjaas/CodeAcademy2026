using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using CodeAcademy.DotnetConsumer.Common.Config;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Starting Consumer 2 application...");

// Establish connection to RabbitMQ
using var connection = await ConnectionHelper.ConnectAsync();
Console.WriteLine("Connected to RabbitMQ");

// Implement a basic consumer here.
// Start with:
// - Create a channel
// - Declare a queue
// - Create a consumer and subscribe to the queue
// - Handle incoming messages by deserializing the JSON and printing the content to the console

// Create a channel and declare the queue
using var channel = await connection.CreateChannelAsync();

// --- FANOUT EXCHANGE IMPLEMENTATION ---

await channel.ExchangeDeclareAsync(exchange: "fanout_exchange", type: ExchangeType.Fanout, durable: false, autoDelete: false);

var queueResult = await channel.QueueDeclareAsync(queue: string.Empty, durable: false, exclusive: true, autoDelete: true, arguments: null);
await channel.QueueBindAsync(queue: queueResult.QueueName, exchange: "fanout_exchange", routingKey: String.Empty);


// Set up a consumer to listen for messages
var consumer = new AsyncEventingBasicConsumer(channel);

// Handle received messages
consumer.ReceivedAsync += async (sender, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = JsonSerializer.Deserialize<JsonNode>(Encoding.UTF8.GetString(body));

    Console.WriteLine($"Received message: {message}");

    // Simulate processing time
    await Task.Delay(1000);

    // Acknowledge the message
    await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
};
// Start consuming messages
await channel.BasicConsumeAsync(queue: queueResult.QueueName, autoAck: false, consumerTag: "", noLocal: false, exclusive: false, arguments: null, consumer: consumer);
Console.ReadLine(); // Keep the application running to listen for messages

// --- DIRECT EXCHANGE IMPLEMENTATION ---

// await channel.ExchangeDeclareAsync(exchange: "direct_exchange", type: "direct", autoDelete: false, arguments: null);

// await channel.QueueDeclareAsync(queue: "direct_queue_2", durable: true, exclusive: false, autoDelete: false, arguments: null);
// await channel.QueueBindAsync(queue: "direct_queue_2", exchange: "direct_exchange", routingKey: "direct_key_2", arguments: null);

// // Set up a consumer to listen for messages
// var consumer = new AsyncEventingBasicConsumer(channel);

// // Handle received messages
// consumer.ReceivedAsync += async (sender, eventArgs) =>
// {
//     var body = eventArgs.Body.ToArray();
//     var message = JsonSerializer.Deserialize<JsonNode>(Encoding.UTF8.GetString(body));

//     Console.WriteLine($"Received message: {message}");

//     // Simulate processing time
//     await Task.Delay(1000);

//     // Acknowledge the message
//     await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
// };
// // Start consuming messages
// await channel.BasicConsumeAsync(queue: "direct_queue_2", autoAck: false, consumerTag: "", noLocal: false, exclusive: false, arguments: null, consumer: consumer);
// Console.ReadLine(); // Keep the application running to listen for messages

// --- TOPIC EXCHANGE IMPLEMENTATION ---

// await channel.ExchangeDeclareAsync(exchange: "topic_exchange", type: "topic", autoDelete: false, arguments: null);

// await channel.QueueDeclareAsync(queue: "topic_queue_2", durable: true, exclusive: false, autoDelete: false, arguments: null);
// await channel.QueueBindAsync(queue: "topic_queue_2", exchange: "topic_exchange", routingKey: "idem.*", arguments: null);

// // Set up a consumer to listen for messages
// var consumer = new AsyncEventingBasicConsumer(channel);

// // Handle received messages
// consumer.ReceivedAsync += async (sender, eventArgs) =>
// {
//     var body = eventArgs.Body.ToArray();
//     var message = JsonSerializer.Deserialize<JsonNode>(Encoding.UTF8.GetString(body));

//     Console.WriteLine($"Received message: {message}");

//     // Simulate processing time
//     await Task.Delay(1000);

//     // Acknowledge the message
//     await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
// };
// // Start consuming messages
// await channel.BasicConsumeAsync(queue: "topic_queue_2", autoAck: false, consumerTag: "", noLocal: false, exclusive: false, arguments: null, consumer: consumer);
// Console.ReadLine(); // Keep the application running to listen for messages

// --- HEADER EXCHANGE IMPLEMENTATION ---

// await channel.ExchangeDeclareAsync(exchange: "headers_exchange", type: "headers", autoDelete: false, arguments: null);

// await channel.QueueDeclareAsync(queue: "headers_queue_2", durable: true, exclusive: false, autoDelete: false, arguments: null);
// await channel.QueueBindAsync(
//         queue: "headers_queue_2",
//         exchange: "headers_exchange",
//         routingKey: "",
//         arguments: new Dictionary<string, object>
//             {
//                 { "x-match", "any" },
//                 { "author", "alice" },
//                 { "visibility", "draft" }
//             }
//         );
// // Set up a consumer to listen for messages
// var consumer = new AsyncEventingBasicConsumer(channel);

// // Handle received messages
// consumer.ReceivedAsync += async (sender, eventArgs) =>
// {
//     var body = eventArgs.Body.ToArray();
//     var message = JsonSerializer.Deserialize<JsonNode>(Encoding.UTF8.GetString(body));

//     Console.WriteLine($"Received message: {message}");

//     // Simulate processing time
//     await Task.Delay(1000);

//     // Acknowledge the message
//     await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
// };
// // Start consuming messages
// await channel.BasicConsumeAsync(queue: "headers_queue_2", autoAck: false, consumerTag: "", noLocal: false, exclusive: false, arguments: null, consumer: consumer);
// Console.ReadLine(); // Keep the application running to listen for messages