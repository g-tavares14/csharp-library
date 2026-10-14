namespace csharp_programming_language;

public class BookReportsTests
{
    private static Book CreateBook(string title, string author = "Autor", string genre = "Gênero") =>
        new() { Title = title, Author = author, Genre = genre };

    [Fact]
    public void FilterBy_ReturnsEmpty_WhenValueIsBlank()
    {
        var bookManager = new BookManager(new LoanManager());
        bookManager.AddBook(CreateBook("Clean Code"));

        Assert.Empty(bookManager.FilterBy(BookFilter.Title, "  "));
    }

    [Fact]
    public void FilterBy_ReturnsMatchingBooksOrderedByTitle()
    {
        var bookManager = new BookManager(new LoanManager());
        bookManager.AddBook(CreateBook("Dune", "Frank Herbert"));
        bookManager.AddBook(CreateBook("Clean Code", "Robert Martin"));
        bookManager.AddBook(CreateBook("Children of Dune", "Frank Herbert"));

        var titles = bookManager.FilterBy(BookFilter.Author, "herbert").Select(book => book.Title);

        Assert.Equal(["Children of Dune", "Dune"], titles);
    }

    [Fact]
    public void FilterBy_LooksOnlyAtTheChosenField()
    {
        var bookManager = new BookManager(new LoanManager());
        bookManager.AddBook(CreateBook("Dune", "Frank Herbert", "Ficção"));

        Assert.Empty(bookManager.FilterBy(BookFilter.Genre, "Dune"));
        Assert.Single(bookManager.FilterBy(BookFilter.Genre, "ficção"));
    }

    [Fact]
    public void GetStatistics_CountsTotalBorrowedAndAvailable()
    {
        var loanManager = new LoanManager();
        var bookManager = new BookManager(loanManager);
        var borrowedBook = CreateBook("Clean Code");
        bookManager.AddBook(borrowedBook);
        bookManager.AddBook(CreateBook("Dune"));
        bookManager.AddBook(CreateBook("Alice"));
        loanManager.BorrowBook(borrowedBook, new User { Name = "Ana", Email = "ana@x.com" });

        var (total, borrowed, available) = bookManager.GetStatistics();

        Assert.Equal(3, total);
        Assert.Equal(1, borrowed);
        Assert.Equal(2, available);
    }

    [Fact]
    public void CountBy_GroupsIgnoringCase_AndOrdersByValue()
    {
        var bookManager = new BookManager(new LoanManager());
        bookManager.AddBook(CreateBook("Dune", genre: "Ficção"));
        bookManager.AddBook(CreateBook("Clean Code", genre: "Tecnologia"));
        bookManager.AddBook(CreateBook("Alice", genre: "ficção"));

        var groups = bookManager.CountBy(BookFilter.Genre);

        Assert.Equal(2, groups.Count);
        Assert.Equal(2, groups[0].Count);
        Assert.Equal("Tecnologia", groups[1].Value);
        Assert.Equal(1, groups[1].Count);
    }

    [Fact]
    public void CountBy_ReturnsEmpty_WhenFieldIsNotValid()
    {
        var bookManager = new BookManager(new LoanManager());
        bookManager.AddBook(CreateBook("Clean Code"));

        Assert.Empty(bookManager.CountBy((BookFilter)99));
    }

    [Fact]
    public void DescribeBook_ShowsAvailable_WhenBookIsNotBorrowed()
    {
        var bookManager = new BookManager(new LoanManager());

        var description = bookManager.DescribeBook(CreateBook("Clean Code"));

        Assert.Contains("Disponível", description);
    }

    [Fact]
    public void DescribeBook_ShowsBorrowed_WhenBookIsBorrowed()
    {
        var loanManager = new LoanManager();
        var bookManager = new BookManager(loanManager);
        var book = CreateBook("Clean Code");
        loanManager.BorrowBook(book, new User { Name = "Ana", Email = "ana@x.com" });

        var description = bookManager.DescribeBook(book);

        Assert.Contains("Emprestado", description);
    }
}
