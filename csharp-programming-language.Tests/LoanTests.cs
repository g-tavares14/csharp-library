namespace csharp_programming_language;

public class LoanTests
{
    [Fact]
    public void CalculateFine_ReturnsZero_WhenOnTime()
    {
        // Arrange
        var loan = new Loan { Book = new Book(), User = new User() };

        // Act
        var fine = loan.CalculateFine(loan.LoanDate);

        // Assert
        Assert.Equal(0m, fine);
    }

    [Fact]
    public void CalculateFine_Returns6_WhenThreeDaysLate()
    {
        var loan = new Loan { Book = new Book(), User = new User() };
        var today = loan.LoanDate.AddDays(10); // prazo é 7 dias, então são 3 de atraso

        var fine = loan.CalculateFine(today);

        Assert.Equal(6.00m, fine);
    }

    [Fact]
    public void BorrowBook_ReturnsFalse_WhenBookIsAlreadyBorrowed()
    {
        var loanManager = new LoanManager();
        var book = new Book { Title = "Clean Code", Author = "Robert Martin", Genre = "Tecnologia" };
        var user = new User { Name = "João", Email = "joao@x.com" };

        loanManager.BorrowBook(book, user);

        var result = loanManager.BorrowBook(book, user);

        Assert.False(result);
    }

    [Fact]
    public void BorrowBook_ReturnsTrue_WhenBookIsAvailable()
    {
        var loanManager = new LoanManager();
        var book = new Book { Title = "Clean Code", Author = "Robert Martin", Genre = "Tecnologia" };
        var user = new User { Name = "João", Email = "joao@x.com" };

        var result = loanManager.BorrowBook(book, user);

        Assert.True(result);
    }

    [Fact]
    public void DueDate_IsSevenDaysAfterLoanDate()
    {
        var loan = new Loan { Book = new Book(), User = new User(), LoanDate = new DateOnly(2026, 1, 1) };

        Assert.Equal(new DateOnly(2026, 1, 8), loan.DueDate);
    }

    [Fact]
    public void DaysOverdue_IsZero_OnDueDate()
    {
        var loan = new Loan { Book = new Book(), User = new User(), LoanDate = new DateOnly(2026, 1, 1) };

        Assert.Equal(0, loan.DaysOverdue(loan.DueDate));
    }

    [Fact]
    public void DaysOverdue_CountsUntilReturnDate_WhenLoanWasReturned()
    {
        var loan = new Loan
        {
            Book = new Book(),
            User = new User(),
            LoanDate = new DateOnly(2026, 1, 1),
            ReturnDate = new DateOnly(2026, 1, 11)
        };

        var daysOverdue = loan.DaysOverdue(new DateOnly(2026, 3, 1));

        Assert.Equal(3, daysOverdue);
    }

    [Fact]
    public void CalculateFine_ReturnsZero_WhenReturnedOnTime()
    {
        var loan = new Loan
        {
            Book = new Book(),
            User = new User(),
            LoanDate = new DateOnly(2026, 1, 1),
            ReturnDate = new DateOnly(2026, 1, 8)
        };

        var fine = loan.CalculateFine(new DateOnly(2026, 3, 1));

        Assert.Equal(0m, fine);
    }

    [Fact]
    public void CalculateFine_ChargesUntilReturnDate_WhenReturnedLate()
    {
        var loan = new Loan
        {
            Book = new Book(),
            User = new User(),
            LoanDate = new DateOnly(2026, 1, 1),
            ReturnDate = new DateOnly(2026, 1, 11)
        };

        var fine = loan.CalculateFine(new DateOnly(2026, 3, 1));

        Assert.Equal(6.00m, fine);
    }
}
