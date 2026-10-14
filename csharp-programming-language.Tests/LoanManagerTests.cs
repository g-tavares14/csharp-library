namespace csharp_programming_language;

public class LoanManagerTests
{
    private static Book CreateBook(string title) => new() { Title = title, Author = "Autor", Genre = "Gênero" };

    private static User CreateUser(string name) => new() { Name = name, Email = $"{name.ToLower()}@x.com" };

    [Fact]
    public void IsAvailable_ReturnsTrue_WhenBookWasNeverBorrowed()
    {
        var loanManager = new LoanManager();

        Assert.True(loanManager.IsAvailable(CreateBook("Clean Code")));
    }

    [Fact]
    public void IsAvailable_ReturnsFalse_WhenBookIsBorrowed()
    {
        var loanManager = new LoanManager();
        var book = CreateBook("Clean Code");

        loanManager.BorrowBook(book, CreateUser("Ana"));

        Assert.False(loanManager.IsAvailable(book));
    }

    [Fact]
    public void ReturnBook_ReturnsNull_WhenBookIsNotBorrowed()
    {
        var loanManager = new LoanManager();

        var loan = loanManager.ReturnBook(CreateBook("Clean Code"));

        Assert.Null(loan);
    }

    [Fact]
    public void ReturnBook_SetsReturnDate_AndMakesBookAvailable()
    {
        var loanManager = new LoanManager();
        var book = CreateBook("Clean Code");
        loanManager.BorrowBook(book, CreateUser("Ana"));

        var loan = loanManager.ReturnBook(book);

        Assert.NotNull(loan);
        Assert.NotNull(loan.ReturnDate);
        Assert.True(loanManager.IsAvailable(book));
    }

    [Fact]
    public void BorrowBook_ReturnsTrue_WhenBookWasReturned()
    {
        var loanManager = new LoanManager();
        var book = CreateBook("Clean Code");
        var user = CreateUser("Ana");
        loanManager.BorrowBook(book, user);
        loanManager.ReturnBook(book);

        var result = loanManager.BorrowBook(book, user);

        Assert.True(result);
    }

    [Fact]
    public void GetLoans_KeepsReturnedLoansAsHistory()
    {
        var loanManager = new LoanManager();
        var book = CreateBook("Clean Code");
        var user = CreateUser("Ana");
        loanManager.BorrowBook(book, user);
        loanManager.ReturnBook(book);
        loanManager.BorrowBook(book, user);

        Assert.Equal(2, loanManager.GetLoans().Count);
        Assert.Single(loanManager.GetActiveLoans());
    }

    [Fact]
    public void GetActiveLoans_ExcludesReturnedLoans()
    {
        var loanManager = new LoanManager();
        var returnedBook = CreateBook("Clean Code");
        var borrowedBook = CreateBook("Dune");
        var user = CreateUser("Ana");
        loanManager.BorrowBook(returnedBook, user);
        loanManager.BorrowBook(borrowedBook, user);
        loanManager.ReturnBook(returnedBook);

        var activeLoans = loanManager.GetActiveLoans();

        Assert.Single(activeLoans);
        Assert.Equal("Dune", activeLoans[0].Book.Title);
    }

    [Fact]
    public void GetActiveLoans_ReturnsOnlyLoansOfTheUser()
    {
        var loanManager = new LoanManager();
        var ana = CreateUser("Ana");
        var bruno = CreateUser("Bruno");
        loanManager.BorrowBook(CreateBook("Clean Code"), ana);
        loanManager.BorrowBook(CreateBook("Dune"), bruno);

        var anaLoans = loanManager.GetActiveLoans(ana);

        Assert.Single(anaLoans);
        Assert.Equal("Clean Code", anaLoans[0].Book.Title);
    }

    [Fact]
    public void GetActiveLoans_OrdersByBookTitle()
    {
        var loanManager = new LoanManager();
        var user = CreateUser("Ana");
        loanManager.BorrowBook(CreateBook("dune"), user);
        loanManager.BorrowBook(CreateBook("Clean Code"), user);
        loanManager.BorrowBook(CreateBook("Alice"), user);

        var titles = loanManager.GetActiveLoans().Select(loan => loan.Book.Title);

        Assert.Equal(["Alice", "Clean Code", "dune"], titles);
    }

    [Fact]
    public void CountActiveLoansByUser_GroupsByUserAndIgnoresReturnedLoans()
    {
        var loanManager = new LoanManager();
        var ana = CreateUser("Ana");
        var bruno = CreateUser("Bruno");
        var returnedBook = CreateBook("Alice");
        loanManager.BorrowBook(CreateBook("Clean Code"), ana);
        loanManager.BorrowBook(CreateBook("Dune"), ana);
        loanManager.BorrowBook(returnedBook, bruno);
        loanManager.ReturnBook(returnedBook);

        var counts = loanManager.CountActiveLoansByUser();

        var item = Assert.Single(counts);
        Assert.Equal("Ana", item.UserName);
        Assert.Equal("ana@x.com", item.Email);
        Assert.Equal(2, item.Count);
    }
}
