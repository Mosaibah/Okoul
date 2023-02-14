using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Okoul.Models;
using Okoul.Services;

namespace Okoul.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiQuotesController : ControllerBase
    {
        private readonly OkoulContext _context;
        private readonly IQuoteService quoteService;

        public ApiQuotesController(OkoulContext context, IQuoteService quoteService)
        {
            _context = context;
            this.quoteService = quoteService;
        }
        
        [HttpGet]
        public async Task<ActionResult<Quote>> GetRandomQuote()
        {
            return await quoteService.GetRandomQuote();
        }

        // GET: api/ApiQuotes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Quote>> GetQuote(int id)
        {
            var quote = await _context.Quote.FindAsync(id);

            if (quote == null)
            {
                return NotFound();
            }

            return quote;
        }

        // PUT: api/ApiQuotes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuote(int id, Quote quote)
        {
            if (id != quote.Id)
            {
                return BadRequest();
            }

            _context.Entry(quote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ApiQuotes
        [HttpPost]
        public async Task<ActionResult<Quote>> PostQuote(Quote quote)
        {
            await quoteService.AddQuote(quote);
            return CreatedAtAction("GetQuote", new { id = quote.Id }, quote);
        }

        // DELETE: api/ApiQuotes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            var quote = await _context.Quote.FindAsync(id);
            if (quote == null)
            {
                return NotFound();
            }

            await quoteService.DeleteQuote(id);

            return NoContent();
        }

        private bool QuoteExists(int id)
        {
            return _context.Quote.Any(e => e.Id == id);
        }
    }
}
