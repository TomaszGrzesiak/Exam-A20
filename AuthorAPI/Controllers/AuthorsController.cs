using Domain.Models;
using EfcData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuthorAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorsController : ControllerBase
{
    private Context context;

    public AuthorsController(Context context)
    {
        this.context = context;
    }
    
    [HttpPost]
    //[Route("/Author/")] // without this line, it would work like there 'd be "/Authors"
    public async Task<ActionResult<Author>> AddAuthor([FromBody] Author author)
    {
        try
        {
            EntityEntry<Author> addedEntityEntry = await context.Author.AddAsync(author);
            Author added = addedEntityEntry.Entity;
            await context.SaveChangesAsync();
            return Created($"/authors/{added.Id}", added);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<Author>>> GetALl()
    {
        try
        {
            ICollection<Author> authors = await context.Author
                .Include(a => a.Books)
                .ToListAsync();
            return Ok(authors);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}