using ClientePersonaService.Data;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

public class RabbitMQConsumer : IDisposable
{
    private readonly string _hostname;
    private readonly string _queueName;
    private IConnection _connection;
    private IModel _channel;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RabbitMQConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        _hostname = "localhost";
        _queueName = "validar_cliente_queue";
        _serviceScopeFactory = serviceScopeFactory;

        var factory = new ConnectionFactory() { HostName = _hostname };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: _queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    public void Start()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"[Received] Mensaje recibido: {message}");

            var clienteId = int.Parse(message.Split(':')[1]);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var cliente = context.Clientes.Find(clienteId);

                var responseMessage = cliente != null ? "ClienteValido" : "ClienteInvalido";

                var responseBytes = Encoding.UTF8.GetBytes(responseMessage);

                _channel.BasicPublish(exchange: "",
                                     routingKey: "respuesta_validacion_queue",
                                     basicProperties: null,
                                     body: responseBytes);

                Console.WriteLine($"[Sent] Respuesta enviada: {responseMessage}");
            }
        };

        _channel.BasicConsume(queue: _queueName,
                             autoAck: true,
                             consumer: consumer);

        Console.WriteLine("Esperando mensajes...");
    }

    public void Dispose()
    {
        Close();
    }

    public void Close()
    {
        if (_channel != null && _channel.IsOpen)
        {
            _channel.Close();
        }
        if (_connection != null && _connection.IsOpen)
        {
            _connection.Close();
        }
    }
}
