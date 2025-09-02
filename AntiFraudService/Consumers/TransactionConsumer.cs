using Confluent.Kafka;
using System.Text.Json;
using AntiFraudService.Models;
using AntiFraudService.Producers;

namespace AntiFraudService.Consumers;

public class TransactionConsumer
{
    private readonly StatusProducer _statusProducer;

    public TransactionConsumer(StatusProducer statusProducer)
    {
        _statusProducer = statusProducer;
    }

    public void Start()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "anti-fraud-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("transaction_created");

        Console.WriteLine("Anti-Fraud Service listening...");

        while (true)
        {
            var msg = consumer.Consume();
            var transaction = JsonSerializer.Deserialize<Transaction>(msg.Message.Value);

            if (transaction is null)
            {
                Console.WriteLine("Received null transaction, skipping...");
                continue;
            }

            var status = EvaluateTransaction(transaction);
            var update = new TransactionStatusUpdate { Id = transaction.Id, Status = status };

            _statusProducer.PublishStatusAsync(update).Wait();
            Console.WriteLine($"Processed transaction {transaction.Id}: {status}");
        }
    }

    private string EvaluateTransaction(Transaction tx)
    {
        // Simple fraud rules
        if (tx.Value > 2000)
            return "rejected";

        // TODO: Add daily total check via DB or cache
        return "approved";
    }
}
