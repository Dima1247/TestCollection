using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestWebApp.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class RabbitMqTestController : ControllerBase {
    [HttpGet]
    public string Get() {
      return "Check /data_light and /data_hard";
    }

    [HttpGet("test_consume")]
    public Task TestConsume() {
      return RabbitMqTestService.TestConsume();
    }

    [HttpGet("test_send/{count}")]
    public void TestSend(int count) {
      RabbitMqTestService.TestSend(count);
    }
  }

  public class RabbitMqTestData {
    public string Summary { get; set; }
  }

  public static class RabbitMqTestService {
    private const string TEST_CONNECTION_ENDPOINT = "rabbitmqtest";

    private const string TEST_QUEUE = "testQueue";
    private const string TEST_EXCHANGE = "testExchange";
    private const string TEST_DIRECT_RT = "testDirectRoutingKey";

    public async static Task TestConsume() {
      try {
        using var connection = EstablishConnection("consumer");
        using var channel = EnsureChannelConfigured(connection);

        var ct1 = CreateConsumer(channel, "Dima");
        var ct2 = CreateConsumer(channel, "Joe");

        await Task.Delay(15000);

        channel.BasicCancel(ct1);

        await Task.Delay(15000);

        channel.BasicCancel(ct2);
        channel.Close();
        connection.Close();
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }
    }

    public static void TestSend(int count = 1) {
      try {
        using var connection = EstablishConnection("producer");
        using var channel = EstablishChannel(connection);

        //In review
        var ct = CancellationToken.None;
        var loop = Parallel.For(0, count, new ParallelOptions { CancellationToken = ct }, (i) => {
          var messageBodyBytes = Encoding.UTF8.GetBytes($"Hello, world {i}!");
          channel.BasicPublish(TEST_EXCHANGE, TEST_DIRECT_RT, null, messageBodyBytes);
          Thread.Sleep(1000);
          Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} thread finished work #{i}");
        });

        while (!loop.IsCompleted) {
          Task.Delay(100);
          Console.WriteLine("Loop awaiter!");
        }
        //In review

        channel.Close();
        connection.Close();
      }
      catch (Exception ex) {
        Console.WriteLine(ex.Message);
      }
    }

    private static IConnection EstablishConnection(string clientName) {
      var connectionFactory = new ConnectionFactory {
        Endpoint = new AmqpTcpEndpoint(TEST_CONNECTION_ENDPOINT),
        ClientProvidedName = clientName
      };

      return connectionFactory.CreateConnection();
    }

    private static IModel EstablishChannel(IConnection connection) {
      return connection.CreateModel();
    }

    private static IModel EnsureChannelConfigured(IConnection connection) {
      var channel = EstablishChannel(connection);
      channel.ExchangeDeclare(TEST_EXCHANGE, ExchangeType.Fanout);
      channel.QueueDeclare(TEST_QUEUE, false, true, true);
      channel.QueueBind(TEST_QUEUE, TEST_EXCHANGE, TEST_DIRECT_RT, null);

      return channel;
    }

    private static string CreateConsumer(IModel channel, string name) {
      var consumer = new EventingBasicConsumer(channel);
      consumer.Received += (ch, ea) => {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine($"Consumed message {message} by {name}");

        channel.BasicAck(ea.DeliveryTag, false);
      };
      
      return channel.BasicConsume(TEST_QUEUE, false, consumer);
    }
  }
}
