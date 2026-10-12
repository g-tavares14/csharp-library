namespace csharp_programming_language;

public class Library
{
    private Dictionary<Guid, Book> books = new Dictionary<Guid, Book>();

    public void AddBook(Book book)
    {
        Guid bookId = Guid.NewGuid();
        book.Id = bookId;
        books.Add(bookId, book);
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