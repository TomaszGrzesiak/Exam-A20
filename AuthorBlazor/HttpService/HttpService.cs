using System.Text;
using System.Text.Json;
using AuthorBlazor.Pages;
using Domain.Models;

namespace AuthorBlazor.HttpService;

public class HttpService
{
    HttpClient httpClient = new();
    private string HOST = "https://localhost:7011";

    public async Task<Author> AddAuthorAsync(Author author)
    {
        string authorAsJson = JsonSerializer.Serialize(author);
        StringContent content = new(authorAsJson, Encoding.UTF8, "application/json");
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(HOST+"/Authors/", content);
        string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {httpResponseMessage.StatusCode}, {responseContent}");
        }

        Author returned = JsonSerializer.Deserialize<Author>(responseContent, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        })!;
            
        return returned;
    }
    
    public async Task<List<Author>> GetAllAuthors()
    {
        HttpResponseMessage responseMessage = await httpClient.GetAsync(HOST+"/Authors/");
        string content = await responseMessage.Content.ReadAsStringAsync();
        
        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {responseMessage.StatusCode}, {content}");
        }

        List<Author> authors = JsonSerializer.Deserialize<List<Author>>(content, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        })!;
        return authors;
    }

    public async Task<Book> AddBook(Book book, int authorId)
    {
        string bookAsJson = JsonSerializer.Serialize(book);
        StringContent content = new(bookAsJson, Encoding.UTF8, "application/json");
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(HOST+"/Books/"+authorId, content);
        string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {httpResponseMessage.StatusCode}, {responseContent}");
        }

        Book returned = JsonSerializer.Deserialize<Book>(responseContent, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        })!;
            
        return returned;
    }
    
    public async Task<List<Book>> GetAllBooks()
    {
        HttpResponseMessage responseMessage = await httpClient.GetAsync(HOST+"/Books/");
        string content = await responseMessage.Content.ReadAsStringAsync();
        
        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {responseMessage.StatusCode}, {content}");
        }

        List<Book> books = JsonSerializer.Deserialize<List<Book>>(content, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        })!;
        return books;

    }
    
    public async Task DeleteBook(int ISBN)
    {
        using HttpClient httpClient = new();
        HttpResponseMessage responseMessage = await httpClient.DeleteAsync(HOST+"/Books/"+ISBN);
        string content = await responseMessage.Content.ReadAsStringAsync();

        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {responseMessage.StatusCode}, {content}");
        }
    }

}