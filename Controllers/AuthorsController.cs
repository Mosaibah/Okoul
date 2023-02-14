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
using Fluentx.Mvc;

namespace Okoul.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly OkoulContext _context;
        private readonly IAuthorService authorService;

        public AuthorsController(OkoulContext context, IAuthorService authorService)
        {
            _context = context;
            this.authorService = authorService;
        }

        // GET: Authors
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Authors"] = new MultiSelectList(_context.Author, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Route("authors/authorslist")]
        public async Task<IActionResult> AuthorsList([Bind("Id,AuthorIds ,StartDate, EndDate")] SearchAuthorVM model)
        {
            model.PageSize = Convert.ToInt32(HttpContext.Request.Form["length"].FirstOrDefault() ?? "0");
            model.Skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            model.Draw = Request.Form["draw"].FirstOrDefault();

            SearchAuthorVM newModel = await authorService.ListAuthors(model);


            return Json(new
            {
                draw = newModel.Draw,
                recordsTotal = newModel.RecordsTotal,
                recordsFiltered = newModel.RecordsFiltered,
                data = newModel.Authors.Select(c => new
                {
                    id = c.Id,
                    name = c.Name,
                    createdat = c.CreatedAt.ToString("yyyy/MM/dd")
                }).ToList()
            });
        }

        //public RedirectAndPostActionResult AuthorQuotes(int? id)
        //{
        //    Dictionary<string, object> postData = new Dictionary<string, object>();
        //    SearchAuthorVM a = new SearchAuthorVM();
        //    postData.Add("first", "someValueOne");
        //    postData.Add("second", "someValueTwo");

        //    return Fluentx.Mvc.Extensions.RedirectAndPost("http://TheUrlToPostDataTo", a);
        //}
        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Author == null)
            {
                return NotFound();
            }

            var author = await _context.Author
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CreatedAt")] Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Author == null)
            {
                return NotFound();
            }

            var author = await _context.Author.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CreatedAt")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id))
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
            return View(author);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Author == null)
            {
                return NotFound();
            }

            var author = await _context.Author
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Author == null)
            {
                return Problem("Entity set 'OkoulContext.Author'  is null.");
            }
            var author = await _context.Author.FindAsync(id);
            if (author != null)
            {
                _context.Author.Remove(author);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
          return _context.Author.Any(e => e.Id == id);
        }
    }
}
