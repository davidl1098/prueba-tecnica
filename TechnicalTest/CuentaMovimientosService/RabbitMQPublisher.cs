using RabbitMQ.Client;
using System.Text;

public class RabbitMQPublisher
{
    private readonly string _hostname;
    private readonly string _queueName;
    private IConnection _connection;
    private IModel _channel;

    public RabbitMQPublisher(IConfiguration configuration)
    {
        _hostname = configuration["RabbitMQ:Hostname"];
        _queueName = configuration["RabbitMQ:PublisherQueueName"];

        var factory = new ConnectionFactory() { HostName = _hostname };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: _queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    public void PublishValidarCliente(int clienteId)
    {
        var message = $"ClienteId:{clienteId}";
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "",
                             routingKey: _queueName,
                             basicProperties: null,
                             body: body);

        Console.WriteLine($"[Sent] Solicitud de validación enviada: {message}");
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
