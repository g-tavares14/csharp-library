using System.Text.Json;

namespace csharp_programming_language;

public class DataStore(string directory)
{
    private readonly string _booksPath = Path.Combine(directory, "books.json");
    private readonly string _usersPath = Path.Combine(directory, "users.json");

    public void SaveBooks(List<Book> books)
    {
        Save(_booksPath, books);
    }

    public void SaveUsers(List<User> users)
    {
        Save(_usersPath, users);
    }

    public List<Book> LoadBooks()
    {
        return Load<Book>(_booksPath, "de livros");
    }

    public List<User> LoadUsers()
    {
        return Load<User>(_usersPath, "de usuários");
    }

    private static void Save<T>(string path, List<T> items)
    {
        var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    private static List<T> Load<T>(string path, string description)
    {
        if (!File.Exists(path))
        {
            return [];
        }

        try
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<T>>(json) ?? [];
        }
        catch (JsonException)
        {
            var backupPath = path + ".corrompido";
            File.Move(path, backupPath, overwrite: true);
            Console.WriteLine(
                $"O arquivo {description} está corrompido. Uma cópia foi guardada em {backupPath} e o programa vai começar vazio.");
            return [];
        }
    }
}