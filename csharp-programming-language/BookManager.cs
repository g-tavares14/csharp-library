namespace csharp_programming_language;

public class BookManager
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

        books.Add(book.Id, book);
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

    public void UpdateBook(string bookTitle, string newTitle, string newAuthor, string newGenre)
    {
        Book? book = SearchBook(bookTitle);

        if (book == null)
        {
            Console.WriteLine("Nenhum livro encontrado");
            return;
        }

        if (!book.Available)
        {
            Console.WriteLine($"O livro {book.Title} não pode ser atualizado porque ele está emprestado.");
            return;
        }

        if (string.IsNullOrWhiteSpace(newTitle) || string.IsNullOrWhiteSpace(newAuthor) ||
            string.IsNullOrWhiteSpace(newGenre))
        {
            Console.WriteLine("Título, autor e gênero são obrigatórios");
            return;
        }

        foreach (var uniqueBook in books.Values)
        {
            if (uniqueBook.Id != book.Id &&
                string.Equals(newTitle, uniqueBook.Title, StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine($"O livro {newTitle} já existe");
                return;
            }
        }

        book.Title = newTitle;
        book.Author = newAuthor;
        book.Genre = newGenre;
        Console.WriteLine($"O livro {book.Title} foi atualizado");
    }
}