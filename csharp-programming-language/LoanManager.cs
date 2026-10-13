namespace csharp_programming_language;

public class LoanManager
{
    private readonly Dictionary<Guid, Loan> _loans = new Dictionary<Guid, Loan>();

    public Loan? GetActiveLoan(Book book)
    {
        return _loans.Values.SingleOrDefault(loan => loan.Book.Id == book.Id && loan.ReturnDate == null);
    }

    public bool IsAvailable(Book book)
    {
        return GetActiveLoan(book) == null;
    }

    public bool BorrowBook(Book book, User user)
    {
        if (!IsAvailable(book))
        {
            return false;
        }

        Loan loan = new Loan { Book = book, User = user };
        _loans.Add(loan.Id, loan);

        return true;
    }

    public Loan? ReturnBook(Book book)
    {
        Loan? loan = GetActiveLoan(book);

        if (loan == null)
        {
            return null;
        }

        loan.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
        return loan;
    }

    public List<Loan> GetActiveLoans(User? user = null)
    {
        List<Loan> activeLoans = new List<Loan>();

        foreach (var loan in _loans.Values)
        {
            if (loan.ReturnDate == null && (user == null || loan.User.Id == user.Id))
            {
                activeLoans.Add(loan);
            }
        }

        return activeLoans;
    }
}