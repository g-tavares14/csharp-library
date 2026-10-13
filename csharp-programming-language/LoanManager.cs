namespace csharp_programming_language;

public class LoanManager
{
    private Dictionary<Guid, Loan> loans = new Dictionary<Guid, Loan>();

    public bool BorrowBook(Book book, User user)
    {
        if (book.Available == false)
        {
            return false;
        }

        Loan loan = new Loan { Book = book, User = user };
        loans.Add(loan.Id, loan);
        book.Available = false;
        book.ReturnDate = loan.DueDate;

        return true;
    }

    public Loan? ReturnBook(Book book)
    {
        foreach (var borrowBook in loans.Values)
        {
            if (borrowBook.Book.Id == book.Id && borrowBook.ReturnDate == null)
            {
                borrowBook.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
                book.Available = true;
                return borrowBook;
            }
        }

        return null;
    }

    public List<Loan> GetActiveLoans(User? user = null)
    {
        List<Loan> activeLoans = new List<Loan>();

        foreach (var loan in loans.Values)
        {
            if (loan.ReturnDate == null && (user == null || loan.User.Id == user.Id))
            {
                activeLoans.Add(loan);
            }
        }

        return activeLoans;
    }
}