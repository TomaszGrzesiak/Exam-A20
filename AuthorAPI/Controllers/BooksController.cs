using Domain.Models;
using EfcData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuthorAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
    private Context context;

    public BooksController(Context context)
    {
        this.context = context;
    }
    
    [HttpPost]
    [Route("{authorId:int}")]
    //[Route("/Book/")] // without this line, it would work like there 'd be "/Books"
    public async Task<ActionResult<Book>> AddBook(int authorId, [FromBody] Book book)
    {
        try
        {
            Author? existingAuthor = await context.Author.FindAsync(authorId);
            if (existingAuthor is null)
                throw new Exception($"Could not find an author with id {authorId}. No book was added.");
            EntityEntry<Book> addedEntityEntry = await context.Book.AddAsync(book);
            Book bookAdded = addedEntityEntry.Entity;
            existingAuthor.Books.Add(bookAdded);
            await context.SaveChangesAsync();
            return Created($"/books/{bookAdded.ISBN}", bookAdded);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message + "\nInner Exception: \n" + e.InnerException);
        }
    }

    // [HttpGet]
    // public async Task<ActionResult<ICollection<Book>>> GetALlAsync()
    // {
    //     try
    //     {
    //         ICollection<Book> books = await context.Book
    //             .ToListAsync();
    //         return Ok(books);
    //     }
    //     catch (Exception e)
    //     {
    //         return StatusCode(500, e.Message);
    //     }
    // }
    
    [HttpGet]
    public async Task<ActionResult<ICollection<Book>>> GetBooksFilteredAsync([FromQuery] string? authorsFirstNameFilter, [FromQuery] string? bookTitleFilter)
    {
        try
        {
           
            IQueryable<Book> books = context.Book.AsQueryable()
                .Include(b => b.Author);
            if (authorsFirstNameFilter != null)
                books = books.Where(b => b.Author.FirstName.ToLower().Contains(authorsFirstNameFilter.ToLower()));
            if (bookTitleFilter != null)
                books = books.Where(b => b.Title.ToLower().Contains(bookTitleFilter.ToLower()));
            
            var result = await books.ToListAsync();
            // the line below is to remove the inevitable cycle reference, which makes problem when serializing to and from JSON
            foreach (var b in result) b.Author!.Books = new List<Book>();
            
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpDelete]
    [Route("{isbn:int}")] // this works like "/Books/{isbn}"
    public async Task<ActionResult> DeleteBookById(int isbn)
    {
        try
        {
            Book? existing = await context.Book.FindAsync(isbn);
            if (existing is null) throw new Exception($"Could not find a book with isbn {isbn}. Nothing was deleted");
            context.Book.Remove(existing);
            await context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}