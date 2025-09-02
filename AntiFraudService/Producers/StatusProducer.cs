using Confluent.Kafka;
using System.Text.Json;
using AntiFraudService.Models;

namespace AntiFraudService.Producers;

public class StatusProducer
{
    private readonly IProducer<Null, string> _producer;

    public StatusProducer(IProducer<Null, string> producer)
    {
        _producer = producer;
    }

    public async Task PublishStatusAsync(TransactionStatusUpdate update)
    {
        var json = JsonSerializer.Serialize(update);
        await _producer.ProduceAsync("transaction_status_updated", new Message<Null, string> { Value = json });
    }
}
