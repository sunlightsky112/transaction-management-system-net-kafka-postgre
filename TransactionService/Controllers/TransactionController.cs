using Microsoft.AspNetCore.Mvc;
using TransactionService.Models;
using TransactionService.Data;
using TransactionService.Kafka;

namespace TransactionService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly KafkaProducer _kafka;

    public TransactionController(AppDbContext context, KafkaProducer kafka)
    {
        _context = context;
        _kafka = kafka;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Transaction transaction)
    {
        transaction.Id = Guid.NewGuid();
        transaction.Status = "pending";
        transaction.CreatedAt = DateTime.UtcNow;

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        await _kafka.PublishAsync("transaction_created", transaction);

        return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null) return NotFound();
        return Ok(transaction);
    }
}
