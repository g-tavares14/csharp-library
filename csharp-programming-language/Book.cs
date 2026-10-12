namespace csharp_programming_language;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string Genre { get; set; } = "";
    public DateOnly RegistrationDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly ReturnDate { get; set; }
    public bool Available { get; set; } = true;
}