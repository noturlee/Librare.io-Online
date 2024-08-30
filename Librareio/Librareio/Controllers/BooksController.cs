using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Librareio.Models;

namespace Librareio.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryOnlineDbContext _context;

        public BooksController(LibraryOnlineDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.SubmissionId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookName,Author,Description,SubmissionDate")] Book book, IFormFile displayImage, IFormFile pdfFile)
        {
            if (ModelState.IsValid)
            {
                if (displayImage != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await displayImage.CopyToAsync(memoryStream);
                        book.DisplayImage = memoryStream.ToArray();
                    }
                }

                if (pdfFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await pdfFile.CopyToAsync(memoryStream);
                        book.PdfFile = memoryStream.ToArray();
                    }
                }

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }


        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubmissionId,BookName,Author,Description,SubmissionDate")] Book book, IFormFile displayImage, IFormFile pdfFile)
        {
            if (id != book.SubmissionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingBook = await _context.Books.FindAsync(id);

                    if (displayImage != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await displayImage.CopyToAsync(memoryStream);
                            existingBook.DisplayImage = memoryStream.ToArray();
                        }
                    }
                    else
                    {
                        existingBook.DisplayImage = existingBook.DisplayImage;
                    }

                    if (pdfFile != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await pdfFile.CopyToAsync(memoryStream);
                            existingBook.PdfFile = memoryStream.ToArray();
                        }
                    }
                    else
                    {
                        existingBook.PdfFile = existingBook.PdfFile;
                    }

                    existingBook.BookName = book.BookName;
                    existingBook.Author = book.Author;
                    existingBook.Description = book.Description;
                    existingBook.SubmissionDate = book.SubmissionDate;

                    _context.Update(existingBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.SubmissionId))
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
            return View(book);
        }


  
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.SubmissionId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.SubmissionId == id);
        }

        public async Task<IActionResult> DownloadPdf(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null || book.PdfFile == null)
            {
                return NotFound();
            }

            // Return the PDF file
            return File(book.PdfFile, "application/pdf", $"{book.BookName}.pdf");
        }

    }
}
