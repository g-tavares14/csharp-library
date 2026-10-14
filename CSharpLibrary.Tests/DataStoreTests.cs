namespace CSharpLibrary;

public class DataStoreTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
    private readonly DataStore _dataStore;

    public DataStoreTests()
    {
        Directory.CreateDirectory(_directory);
        _dataStore = new DataStore(_directory);
    }

    public void Dispose()
    {
        Directory.Delete(_directory, recursive: true);
    }

    private static Book CreateBook(string title) => new() { Title = title, Author = "Autor", Genre = "Gênero" };

    [Fact]
    public void LoadBooks_ReturnsEmpty_WhenFileDoesNotExist()
    {
        Assert.Empty(_dataStore.LoadBooks());
    }

    [Fact]
    public void SaveBooks_ThenLoadBooks_ReturnsTheSameBooks()
    {
        var book = CreateBook("Clean Code");

        _dataStore.SaveBooks([book]);
        var loaded = _dataStore.LoadBooks();

        var loadedBook = Assert.Single(loaded);
        Assert.Equal(book.Id, loadedBook.Id);
        Assert.Equal("Clean Code", loadedBook.Title);
        Assert.Equal(book.RegistrationDate, loadedBook.RegistrationDate);
    }

    [Fact]
    public void SaveUsers_ThenLoadUsers_KeepsTheAdminRole()
    {
        var user = new User { Name = "João", Email = "joao@x.com" };
        var admin = new Admin { Name = "Maria", Email = "maria@x.com" };

        _dataStore.SaveUsers([user, admin]);
        var loaded = _dataStore.LoadUsers();

        Assert.Equal(2, loaded.Count);
        Assert.Equal("user", loaded.Single(loadedUser => loadedUser.Id == user.Id).Role);
        Assert.Equal("admin", loaded.Single(loadedUser => loadedUser.Id == admin.Id).Role);
    }

    [Fact]
    public void LoadBooks_MovesFileToBackup_WhenJsonIsCorrupted()
    {
        var booksPath = Path.Combine(_directory, "books.json");
        File.WriteAllText(booksPath, "{lixo");

        var loaded = _dataStore.LoadBooks();

        Assert.Empty(loaded);
        Assert.False(File.Exists(booksPath));
        Assert.True(File.Exists(booksPath + ".corrompido"));
    }

    [Fact]
    public void LoadUsers_DoesNotTouchBooksFile_WhenUsersFileIsCorrupted()
    {
        _dataStore.SaveBooks([CreateBook("Clean Code")]);
        File.WriteAllText(Path.Combine(_directory, "users.json"), "{lixo");

        _dataStore.LoadUsers();

        Assert.Single(_dataStore.LoadBooks());
    }

    [Fact]
    public void SaveLoans_ThenLoadLoans_RebuildsLoansWithBookAndUser()
    {
        var book = CreateBook("Clean Code");
        var user = new User { Name = "João", Email = "joao@x.com" };
        var loan = new Loan
        {
            Book = book,
            User = user,
            LoanDate = new DateOnly(2026, 1, 1),
            ReturnDate = new DateOnly(2026, 1, 5)
        };

        _dataStore.SaveLoans([loan]);
        var loaded = _dataStore.LoadLoans([book], [user]);

        var loadedLoan = Assert.Single(loaded);
        Assert.Equal(loan.Id, loadedLoan.Id);
        Assert.Same(book, loadedLoan.Book);
        Assert.Same(user, loadedLoan.User);
        Assert.Equal(new DateOnly(2026, 1, 1), loadedLoan.LoanDate);
        Assert.Equal(new DateOnly(2026, 1, 5), loadedLoan.ReturnDate);
    }

    [Fact]
    public void SaveLoans_StoresOnlyTheIds_NotTheWholeBookAndUser()
    {
        var loan = new Loan { Book = CreateBook("Clean Code"), User = new User { Name = "João", Email = "joao@x.com" } };

        _dataStore.SaveLoans([loan]);
        var json = File.ReadAllText(Path.Combine(_directory, "loans.json"));

        Assert.DoesNotContain("Clean Code", json);
        Assert.DoesNotContain("joao@x.com", json);
    }

    [Fact]
    public void LoadLoans_Discards_WhenTheBookNoLongerExists()
    {
        var book = CreateBook("Clean Code");
        var user = new User { Name = "João", Email = "joao@x.com" };
        _dataStore.SaveLoans([new Loan { Book = book, User = user }]);

        var loaded = _dataStore.LoadLoans([], [user]);

        Assert.Empty(loaded);
    }

    [Fact]
    public void LoadLoans_Discards_WhenTheUserNoLongerExists()
    {
        var book = CreateBook("Clean Code");
        var user = new User { Name = "João", Email = "joao@x.com" };
        _dataStore.SaveLoans([new Loan { Book = book, User = user }]);

        var loaded = _dataStore.LoadLoans([book], []);

        Assert.Empty(loaded);
    }

    [Fact]
    public void SaveUsers_DoesNotOverwriteTheBooksFile()
    {
        _dataStore.SaveBooks([CreateBook("Clean Code")]);

        _dataStore.SaveUsers([new User { Name = "João", Email = "joao@x.com" }]);

        Assert.Equal("Clean Code", _dataStore.LoadBooks().Single().Title);
        Assert.Equal("João", _dataStore.LoadUsers().Single().Name);
    }
}
