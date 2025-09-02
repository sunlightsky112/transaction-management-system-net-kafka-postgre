using Confluent.Kafka;
using System.Text.Json;

namespace TransactionService.Kafka;

public class KafkaProducer
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducer(IProducer<Null, string> producer)
    {
        _producer = producer;
    }

    public async Task PublishAsync<T>(string topic, T message)
    {
        var json = JsonSerializer.Serialize(message);
        await _producer.ProduceAsync(topic, new Message<Null, string> { Value = json });
    }
}
