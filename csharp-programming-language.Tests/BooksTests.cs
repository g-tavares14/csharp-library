namespace csharp_programming_language;

public class BooksTests
{
    [Fact]
    public void AddBook_DoesNotAdd_WhenTitleIsDuplicated()
    {
        var loanManager = new LoanManager();
        var bookManager = new BookManager(loanManager);

        var firstBook = new Book { Title = "Clean Code", Author = "Robert Martin", Genre = "Tecnologia" };
        var secondBook = new Book { Title = "CLEAN CODE", Author = "Outro Autor", Genre = "Ficção" };

        bookManager.AddBook(firstBook);
        bookManager.AddBook(secondBook);

        Assert.Single(bookManager.GetBooks());
    }
}