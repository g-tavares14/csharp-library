namespace CSharpLibrary;

public class BooksTests
{
    [Fact]
    public void AddBook_DoesNotAdd_WhenTitleIsDuplicated()
    {
        var loanManager = new LoanManager();
        var bookManager = new BookManager(loanManager);

        var firstBook = new Book { Title = "Clean Code", Author = "Robert Martin", Genre = "Tecnologia" };
        var secondBook = new Book { Title = "CLEAN CODE", Author = "Outro Autor", Genre = "Ficção" };

        bookManager.AddBook(firstBook);
        bookManager.AddBook(secondBook);

        Assert.Single(bookManager.GetBooks());
    }

    private static Book CreateBook(string title, string author = "Autor", string genre = "Gênero") =>
        new() { Title = title, Author = author, Genre = genre };

    [Fact]
    public void AddBook_Adds_WhenBookIsValid()
    {
        var bookManager = new BookManager(new LoanManager());

        bookManager.AddBook(CreateBook("Clean Code"));

        Assert.Single(bookManager.GetBooks());
    }

    [Theory]
    [InlineData("", "Autor", "Gênero")]
    [InlineData("Clean Code", "  ", "Gênero")]
    [InlineData("Clean Code", "Autor", "")]
    public void AddBook_DoesNotAdd_WhenRequiredFieldIsBlank(string title, string author, string genre)
    {
        var bookManager = new BookManager(new LoanManager());

        bookManager.AddBook(CreateBook(title, author, genre));

        Assert.Empty(bookManager.GetBooks());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void SearchBook_ReturnsNull_WhenTextIsBlank(string text)
    {
        var bookManager = new BookManager(new LoanManager());
        bookManager.AddBook(CreateBook("Clean Code"));

        Assert.Null(bookManager.SearchBook(text));
    }

    [Fact]
    public void SearchBook_FindsBook_ByPartOfTheTitleIgnoringCase()
    {
        var bookManager = new BookManager(new LoanManager());
        bookManager.AddBook(CreateBook("Clean Code"));

        var found = bookManager.SearchBook("clean");

        Assert.NotNull(found);
        Assert.Equal("Clean Code", found.Title);
    }

    [Fact]
    public void SearchBook_ReturnsNull_WhenNoTitleMatches()
    {
        var bookManager = new BookManager(new LoanManager());
        bookManager.AddBook(CreateBook("Clean Code"));

        Assert.Null(bookManager.SearchBook("Dune"));
    }

    [Fact]
    public void DeleteBook_Removes_WhenBookIsAvailable()
    {
        var bookManager = new BookManager(new LoanManager());
        bookManager.AddBook(CreateBook("Clean Code"));

        bookManager.DeleteBook("Clean Code");

        Assert.Empty(bookManager.GetBooks());
    }

    [Fact]
    public void DeleteBook_DoesNotRemove_WhenBookIsBorrowed()
    {
        var loanManager = new LoanManager();
        var bookManager = new BookManager(loanManager);
        var book = CreateBook("Clean Code");
        bookManager.AddBook(book);
        loanManager.BorrowBook(book, new User { Name = "Ana", Email = "ana@x.com" });

        bookManager.DeleteBook("Clean Code");

        Assert.Single(bookManager.GetBooks());
    }

    [Fact]
    public void DeleteBook_KeepsOtherBooks_WhenTitleIsNotFound()
    {
        var bookManager = new BookManager(new LoanManager());
        bookManager.AddBook(CreateBook("Clean Code"));

        bookManager.DeleteBook("Dune");

        Assert.Single(bookManager.GetBooks());
    }

    [Fact]
    public void UpdateBook_ChangesFields_WhenBookIsAvailable()
    {
        var bookManager = new BookManager(new LoanManager());
        var book = CreateBook("Clean Code", "Robert Martin", "Tecnologia");
        bookManager.AddBook(book);

        bookManager.UpdateBook("Clean Code", "Código Limpo", "Robert C. Martin", "Programação");

        Assert.Equal("Código Limpo", book.Title);
        Assert.Equal("Robert C. Martin", book.Author);
        Assert.Equal("Programação", book.Genre);
    }

    [Fact]
    public void UpdateBook_AllowsKeepingTheSameTitle()
    {
        var bookManager = new BookManager(new LoanManager());
        var book = CreateBook("Clean Code", "Robert Martin", "Tecnologia");
        bookManager.AddBook(book);

        bookManager.UpdateBook("Clean Code", "Clean Code", "Robert C. Martin", "Tecnologia");

        Assert.Equal("Robert C. Martin", book.Author);
    }

    [Fact]
    public void UpdateBook_DoesNotChange_WhenBookIsBorrowed()
    {
        var loanManager = new LoanManager();
        var bookManager = new BookManager(loanManager);
        var book = CreateBook("Clean Code");
        bookManager.AddBook(book);
        loanManager.BorrowBook(book, new User { Name = "Ana", Email = "ana@x.com" });

        bookManager.UpdateBook("Clean Code", "Outro Título", "Outro Autor", "Outro Gênero");

        Assert.Equal("Clean Code", book.Title);
    }

    [Fact]
    public void UpdateBook_DoesNotChange_WhenNewTitleBelongsToAnotherBook()
    {
        var bookManager = new BookManager(new LoanManager());
        var book = CreateBook("Clean Code");
        bookManager.AddBook(book);
        bookManager.AddBook(CreateBook("Dune"));

        bookManager.UpdateBook("Clean Code", "DUNE", "Outro Autor", "Outro Gênero");

        Assert.Equal("Clean Code", book.Title);
        Assert.Equal("Autor", book.Author);
    }

    [Fact]
    public void UpdateBook_DoesNotChange_WhenNewFieldIsBlank()
    {
        var bookManager = new BookManager(new LoanManager());
        var book = CreateBook("Clean Code");
        bookManager.AddBook(book);

        bookManager.UpdateBook("Clean Code", "Novo Título", "", "Outro Gênero");

        Assert.Equal("Clean Code", book.Title);
    }
}
