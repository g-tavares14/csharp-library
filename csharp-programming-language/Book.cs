namespace csharp_programming_language;

public class Book
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string Genre { get; set; } = "";
    public DateOnly RegistrationDate { get; init; } = DateOnly.FromDateTime(DateTime.Now);
}