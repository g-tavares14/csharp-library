namespace csharp_programming_language;

public class Loan
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public required Book Book { get; init; }
    public required User User { get; init; }
    public DateOnly LoanDate { get; init; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly DueDate { get; init; } = DateOnly.FromDateTime(DateTime.Now.AddDays(7));
    public DateOnly? ReturnDate { get; set; }

    private const decimal FinePerDay = 2.00m;

    public int DaysOverdue(DateOnly today)
    {
        DateOnly end = ReturnDate ?? today;
        int days = end.DayNumber - DueDate.DayNumber;
        return days > 0 ? days : 0;
    }

    public decimal CalculateFine(DateOnly today)
    {
        return DaysOverdue(today) * FinePerDay;
    }
}