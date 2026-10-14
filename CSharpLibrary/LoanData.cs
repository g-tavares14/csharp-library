namespace CSharpLibrary;

public record LoanData(Guid Id, Guid BookId, Guid UserId, DateOnly LoanDate, DateOnly? ReturnDate);