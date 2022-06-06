using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Book
{
    [Key] public int ISBN { set; get; } = 0;
    [Required, MaxLength(40)]
    public string Title  { set; get; } = null;
    public int? PublicationYear { set; get; } = 0;
    public int? NumOfPages { set; get; } = 0;
    public string? Genre { set; get; } = null;

    public Book()
    {
    }

    public override string ToString()
    {
        return $"ISBN: {ISBN}";
    }
}  