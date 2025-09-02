using Confluent.Kafka;
using AntiFraudService.Producers;
using AntiFraudService.Consumers;

var kafkaConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };
var producer = new ProducerBuilder<Null, string>(kafkaConfig).Build();
var statusProducer = new StatusProducer(producer);

var consumer = new TransactionConsumer(statusProducer);
consumer.Start();
