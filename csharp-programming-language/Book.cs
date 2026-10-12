namespace csharp_programming_language;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string Genre { get; set; } = "";
    public DateTime RegistrationDate { get; set; } = DateTime.Now;
    public DateTime ReturnDate { get; set; }
    public bool Available { get; set; } = true;

}