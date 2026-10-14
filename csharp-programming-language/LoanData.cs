namespace csharp_programming_language;

public record LoanData(Guid Id, Guid BookId, Guid UserId, DateOnly LoanDate, DateOnly? ReturnDate);