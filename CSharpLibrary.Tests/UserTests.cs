namespace CSharpLibrary;

public class UserTests
{
    [Fact]
    public void AddUser_DoesNotAdd_WhenEmailIsDuplicated()
    {
        var userManager = new UserManager();
        var firstUser = new User { Name = "João", Email = "joao@x.com" };
        var secondUser = new User { Name = "João", Email = "JOAO@x.com" };

        userManager.AddUser(firstUser);
        userManager.AddUser(secondUser);

        Assert.Single(userManager.GetUsers());
    }

    [Fact]
    public void AddUser_Adds_WhenEmailIsNotDuplicated()
    {
        var userManager = new UserManager();
        var firstUser = new User { Name = "João", Email = "joao@x.com" };
        var secondUser = new User { Name = "Guilherme", Email = "guilherme@x.com" };

        userManager.AddUser(firstUser);
        userManager.AddUser(secondUser);

        Assert.Equal(2, userManager.GetUsers().Count);
    }

    [Theory]
    [InlineData("", "joao@x.com")]
    [InlineData("João", "  ")]
    public void AddUser_DoesNotAdd_WhenNameOrEmailIsBlank(string name, string email)
    {
        var userManager = new UserManager();

        userManager.AddUser(new User { Name = name, Email = email });

        Assert.Empty(userManager.GetUsers());
    }

    [Fact]
    public void Role_IsUser_ForRegularUsers()
    {
        var user = new User { Name = "João", Email = "joao@x.com" };

        Assert.Equal("user", user.Role);
    }

    [Fact]
    public void Role_IsAdmin_ForAdmins()
    {
        var admin = new Admin { Name = "Maria", Email = "maria@x.com" };

        Assert.Equal("admin", admin.Role);
    }

    [Fact]
    public void SearchUser_FindsUser_IgnoringCase()
    {
        var userManager = new UserManager();
        userManager.AddUser(new User { Name = "João", Email = "joao@x.com" });

        var found = userManager.SearchUser("JOAO@X.COM");

        Assert.NotNull(found);
        Assert.Equal("João", found.Name);
    }

    [Fact]
    public void SearchUser_ReturnsNull_WhenEmailIsNotRegistered()
    {
        var userManager = new UserManager();
        userManager.AddUser(new User { Name = "João", Email = "joao@x.com" });

        Assert.Null(userManager.SearchUser("maria@x.com"));
    }
}
