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

        try
        {
            var json = File.ReadAllText(booksPath);
            return JsonSerializer.Deserialize<List<Book>>(json) ?? [];
        }
        catch (JsonException)
        {
            var backupPath = booksPath + ".corrompido";
            File.Move(booksPath, backupPath, overwrite: true);
            Console.WriteLine(
                $"O arquivo de livros está corrompido. Uma cópia foi guardada em {backupPath} e o programa vai começar sem livros.");
            return [];
        }
    }
}