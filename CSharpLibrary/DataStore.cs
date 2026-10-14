using System.Text.Json;

namespace CSharpLibrary;

public class DataStore(string directory)
{
    private readonly string _booksPath = Path.Combine(directory, "books.json");
    private readonly string _usersPath = Path.Combine(directory, "users.json");
    private readonly string _loansPath = Path.Combine(directory, "loans.json");

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

    public void SaveLoans(List<Loan> loans)
    {
        var loansData = loans
            .Select(loan => new LoanData(loan.Id, loan.Book.Id, loan.User.Id, loan.LoanDate, loan.ReturnDate))
            .ToList();

        Save(_loansPath, loansData);
    }

    public List<Loan> LoadLoans(List<Book> books, List<User> users)
    {
        var loans = new List<Loan>();

        foreach (var data in Load<LoanData>(_loansPath, "de empréstimos"))
        {
            var book = books.FirstOrDefault(b => b.Id == data.BookId);
            var user = users.FirstOrDefault(u => u.Id == data.UserId);

            if (book == null || user == null)
            {
                Console.WriteLine("Um empréstimo foi descartado porque o livro ou o usuário não existe mais.");
                continue;
            }

            loans.Add(new Loan
            {
                Id = data.Id,
                Book = book,
                User = user,
                LoanDate = data.LoanDate,
                ReturnDate = data.ReturnDate
            });
        }

        return loans;
    }
}