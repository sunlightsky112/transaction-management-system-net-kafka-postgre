# 🧠 Transaction & Anti-Fraud Microservices

A distributed, event-driven backend system built with .NET 8, Kafka, and PostgreSQL. It consists of two microservices:

- **Transaction Service**: Handles creation and retrieval of financial transactions.
- **Anti-Fraud Service**: Validates transactions against fraud rules and updates their status via Kafka.

---

## 📦 Tech Stack

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Kafka (Confluent 5.5.3)](https://docs.confluent.io/platform/current/)
- [PostgreSQL 14](https://www.postgresql.org/)
- [Docker Compose](https://docs.docker.com/compose/)
- Entity Framework Core

---

## 🧱 Architecture Overview

```text
[Client] → [Transaction API] → [PostgreSQL]
                      ↓
              Kafka: transaction_created
                      ↓
            [Anti-Fraud Service]
                      ↓
              Kafka: transaction_status_updated
                      ↓
         [Transaction API updates status]
