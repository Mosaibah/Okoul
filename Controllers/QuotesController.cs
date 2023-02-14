using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Okoul.Models;
using Okoul.Models.ViewModels;
using Okoul.Services;

namespace Okoul.Controllers
{
    public class QuotesController : Controller
    {
        private readonly OkoulContext _context;
        private readonly IQuoteService quoteService;

        public QuotesController(OkoulContext context, IQuoteService quoteServicecs)
        {
            _context = context;
            this.quoteService = quoteServicecs;
        }

        // GET: Quotes
        public async Task<IActionResult> Index()
        {
            var quotes = await _context.Quote.Include(q => q.Author).ToListAsync();

            ViewData["Authors"] = new MultiSelectList(_context.Author, "Id", "Name");

            return View();
        }

        [HttpPost]
        [Route("quotes/quoteslist")]
        public async Task<IActionResult> QuotesList([Bind("Id,Authors, Text, StartDate, EndDate")] SearchQuotesVM model)
        {
            model.PageSize = Convert.ToInt32(HttpContext.Request.Form["length"].FirstOrDefault() ?? "0");
            model.Skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            model.Draw = Request.Form["draw"].FirstOrDefault();

            SearchQuotesVM newModel = await quoteService.ListQuotes(model);


            return Json(new
            {
                draw = newModel.Draw,
                recordsTotal = newModel.RecordsTotal,
                recordsFiltered = newModel.RecordsFiltered,
                data = newModel.Quotes.Select(c => new
                {
                    id = c.Id,
                    text = c.Text,
                    author = c.Author.Name,
                    authorid = c.AuthorId,
                    createdat = c.CreatedAt.ToString("yyyy/MM/dd")
                }).ToList()
            });
        }

        // GET: Quotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Quote == null)
            {
                return NotFound();
            }

            var quote = await _context.Quote
                .Include(q => q.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quote == null)
            {
                return NotFound();
            }

            return View(quote);
        }

        // GET: Quotes/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "Name");
            return View();
        }

        // POST: Quotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,CreatedAt,AuthorId")] Quote quote)
        {
            if (ModelState.IsValid)
            {
                await quoteService.AddQuote(quote);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "Name", quote.AuthorId);
            return View(quote);
        }

        // GET: Quotes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Quote == null)
            {
                return NotFound();
            }

            var quote = await _context.Quote.FindAsync(id);
            if (quote == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "Name", quote.AuthorId);
            return View(quote);
        }

        // POST: Quotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,CreatedAt,AuthorId")] Quote quote)
        {
            if (id != quote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await quoteService.UpdateQuote(quote);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuoteExists(quote.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "Name", quote.AuthorId);
            return View(quote);
        }

        // GET: Quotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Quote == null)
            {
                return NotFound();
            }

            var quote = await _context.Quote
                .Include(q => q.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quote == null)
            {
                return NotFound();
            }

            return View(quote);
        }

        // POST: Quotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Quote == null)
            {
                return Problem("Entity set 'OkoulContext.Quote'  is null.");
            }
            await quoteService.DeleteQuote(id);
            return RedirectToAction(nameof(Index));
        }

        private bool QuoteExists(int id)
        {
          return _context.Quote.Any(e => e.Id == id);
        }
    }
}
