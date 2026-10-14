namespace csharp_programming_language;

public class BookManager(LoanManager loanManager)
{
    private readonly Dictionary<Guid, Book> _books = new Dictionary<Guid, Book>();
    private readonly LoanManager _loanManager = loanManager;

    private bool HasRequiredFields(string title, string author, string genre)
    {
        return !string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(author) &&
               !string.IsNullOrWhiteSpace(genre);
    }

    private bool TitleExists(string title, Guid? ignoreId = null)
    {
        return _books.Values.Any(uniqueBook =>
            uniqueBook.Id != ignoreId &&
            string.Equals(title, uniqueBook.Title, StringComparison.InvariantCultureIgnoreCase));
    }

    public void AddBook(Book book)
    {
        if (!HasRequiredFields(book.Title, book.Author, book.Genre))
        {
            Console.WriteLine("Título, autor e gênero são obrigatórios");
            return;
        }

        if (TitleExists(book.Title))
        {
            Console.WriteLine($"O livro {book.Title} já existe");
            return;
        }

        _books.Add(book.Id, book);
        Console.WriteLine($"O livro {book.Title} foi adicionado");
    }

    public void ListBooks()
    {
        if (_books.Count == 0)
        {
            Console.WriteLine("Nenhum livro encontrado a lista está vazia");
            return;
        }

        foreach (var book in _books.Values.OrderBy(book => book.Title, StringComparer.InvariantCultureIgnoreCase))
        {
            Console.WriteLine(DescribeBook(book));
        }
    }

    public string DescribeBook(Book book)
    {
        var activeLoan = _loanManager.GetActiveLoan(book);
        var status = (activeLoan == null ? "Disponível" : $"Emprestado até o dia {activeLoan.DueDate}");

        return $"Titulo: {book.Title}, Autor: {book.Author}, Genero: {book.Genre},  Status: {status}";
    }

    public Book? SearchBook(string bookTitle)
    {
        if (string.IsNullOrWhiteSpace(bookTitle))
        {
            return null;
        }

        return _books.Values.FirstOrDefault(book =>
            book.Title.Contains(bookTitle, StringComparison.InvariantCultureIgnoreCase));
    }

    public void DeleteBook(string bookTitle)
    {
        var book = SearchBook(bookTitle);
        if (book == null)
        {
            Console.WriteLine("Nenhum livro encontrado");
            return;
        }

        if (!_loanManager.IsAvailable(book))
        {
            Console.WriteLine($"O livro {book.Title} não pode ser apagado porque ele está emprestado.");
            return;
        }

        _books.Remove(book.Id);
        Console.WriteLine($"O livro {book.Title} foi apagado");
    }

    public void UpdateBook(string bookTitle, string newTitle, string newAuthor, string newGenre)
    {
        var book = SearchBook(bookTitle);

        if (book == null)
        {
            Console.WriteLine("Nenhum livro encontrado");
            return;
        }

        if (!_loanManager.IsAvailable(book))
        {
            Console.WriteLine($"O livro {book.Title} não pode ser atualizado porque ele está emprestado.");
            return;
        }

        if (!HasRequiredFields(newTitle, newAuthor, newGenre))
        {
            Console.WriteLine("Título, autor e gênero são obrigatórios");
            return;
        }

        if (TitleExists(newTitle, book.Id))
        {
            Console.WriteLine($"O livro {newTitle} já existe");
            return;
        }

        book.Title = newTitle;
        book.Author = newAuthor;
        book.Genre = newGenre;
        Console.WriteLine($"O livro {book.Title} foi atualizado");
    }

    public List<Book> FilterBy(BookFilter filter, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return [];
        }

        return
        [
            .. _books.Values.Where(book =>
                    GetField(book, filter).Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(book => book.Title, StringComparer.InvariantCultureIgnoreCase)
        ];
    }

    private static string GetField(Book book, BookFilter filter) => filter switch
    {
        BookFilter.Title => book.Title,
        BookFilter.Author => book.Author,
        BookFilter.Genre => book.Genre,
        _ => ""
    };
}