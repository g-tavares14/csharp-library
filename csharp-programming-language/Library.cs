namespace csharp_programming_language;

public class Library
{
    private Dictionary<Guid, Book> books = new Dictionary<Guid, Book>();

    public void AddBook(Book book)
    {
        if (string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author) ||
            string.IsNullOrWhiteSpace(book.Genre))
        {
            Console.WriteLine("Título, autor e gênero são obrigatórios");
            return;
        }

        foreach (var uniqueBook in books.Values)
        {
            if (string.Equals(book.Title, uniqueBook.Title, StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine($"O livro {book.Title} já existe");
                return;
            }
        }

        Guid bookId = Guid.NewGuid();
        book.Id = bookId;
        books.Add(bookId, book);
        Console.WriteLine($"O livro {book.Title} foi adicionado");
    }

    public void ListBooks()
    {
        foreach (var book in books.Values)
        {
            Console.WriteLine(
                $"Titulo: {book.Title}, Autor: {book.Author}, Genero: {book.Genre}, Status: {(book.Available ? "Disponível" : "Emprestado")}");
        }
    }

    public Book? SearchBook(string bookTitle)
    {
        foreach (var book in books.Values)
        {
            if (book.Title.Contains(bookTitle, StringComparison.InvariantCultureIgnoreCase))
            {
                return book;
            }
        }

        return null;
    }

    public bool BorrowBook(string bookTitle)
    {
        Book? book = SearchBook(bookTitle);

        if (book == null || !book.Available)
        {
            return false;
        }

        book.Available = false;
        book.ReturnDate = DateTime.Now.AddDays(7);
        return true;
    }

    public bool ReturnBook(string bookTitle)
    {
        Book? book = SearchBook(bookTitle);

        if (book == null || book.Available)
        {
            return false;
        }

        book.Available = true;
        book.ReturnDate = DateTime.Now;
        return true;
    }
}