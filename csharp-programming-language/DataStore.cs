using System.Text.Json;

namespace csharp_programming_language;

public class DataStore(string booksPath)
{
    public void SaveBooks(List<Book> books)
    {
        try
        {
            var json = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(booksPath, json);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public List<Book> LoadBooks()
    {
        if (!File.Exists(booksPath))
        {
            return [];
        }

        var json = File.ReadAllText(booksPath);
        return JsonSerializer.Deserialize<List<Book>>(json) ?? [];
    }
}