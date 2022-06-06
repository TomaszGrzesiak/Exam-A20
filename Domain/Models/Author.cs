using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Author
{
    [Key]
    public int Id { set; get; }
    [Required, MaxLength(15)]
    public  string FirstName { set; get; }
    [Required, MaxLength(15)]
    public  string LastName { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();

    public Author()
    {
    }

    public override string ToString()
    {
        string booksToString = ""; 
        foreach (var VARIABLE in Books) { booksToString += VARIABLE + ", "; }
        return $"ID: {Id}, FirstName: {FirstName}, LastName: {LastName}, Books: {booksToString}";
    }
}