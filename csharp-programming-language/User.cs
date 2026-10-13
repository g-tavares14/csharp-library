namespace csharp_programming_language;

public class User
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public virtual string Role => "user";
}

public class Admin : User
{
    public override string Role => "admin";
}