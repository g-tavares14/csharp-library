using System.Runtime.InteropServices.JavaScript;

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
        if (books.Count == 0)
        {
            Console.WriteLine("Nenhum livro encontrado a lista está vazia");
            return;
        }

        foreach (var book in books.Values)
        {
            string status = (book.Available ? "Disponível" : $"Emprestado até o dia {book.ReturnDate}");

            Console.WriteLine(
                $"Titulo: {book.Title}, Autor: {book.Author}, Genero: {book.Genre},  Status: {status}");
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
        book.ReturnDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
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
        book.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
        return true;
    }

    public void DeleteBook(string bookTitle)
    {
        Book? book = SearchBook(bookTitle);
        if (book == null)
        {
            Console.WriteLine("Nenhum livro encontrado");
            return;
        }

        if (!book.Available)
        {
            Console.WriteLine($"O livro {book.Title} não pode ser apagado porque ele está emprestado.");
            return;
        }

        books.Remove(book.Id);
        Console.WriteLine($"O livro {book.Title} foi apagado");
    }
}