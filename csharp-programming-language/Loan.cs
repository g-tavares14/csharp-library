namespace csharp_programming_language;

public class Loan
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Book Book { get; init; }
    public required User User { get; init; }
    public DateOnly LoanDate { get; init; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly DueDate => LoanDate.AddDays(LoanDays);
    public DateOnly? ReturnDate { get; set; }

    private const int LoanDays = 7;
    private const decimal FinePerDay = 2.00m;

    public int DaysOverdue(DateOnly today)
    {
        var end = ReturnDate ?? today;
        var days = end.DayNumber - DueDate.DayNumber;
        return days > 0 ? days : 0;
    }

    public decimal CalculateFine(DateOnly today)
    {
        return DaysOverdue(today) * FinePerDay;
    }
}