namespace csharp_programming_language;

public class User
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; init; } = "";
    public string Email { get; init; } = "";
    public virtual string Role => "user";
}

public class Admin : User
{
    public override string Role => "admin";
}