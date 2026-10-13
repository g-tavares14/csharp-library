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

    public bool ReturnBook(Book book)
    {
        foreach (var borrowBook in loans.Values)
        {
            if (borrowBook.Book.Id == book.Id && borrowBook.ReturnDate == null)
            {
                borrowBook.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
                book.Available = true;
                return true;
            }
        }

        return false;
    }
}