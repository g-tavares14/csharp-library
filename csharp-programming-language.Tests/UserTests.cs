namespace csharp_programming_language;

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
}