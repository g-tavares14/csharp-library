using System.Text.Json.Serialization;

namespace CSharpLibrary;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(User), "user")]
[JsonDerivedType(typeof(Admin), "admin")]
public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = "";
    public string Email { get; init; } = "";

    [JsonIgnore] public virtual string Role => "user";
}

public class Admin : User
{
    [JsonIgnore] public override string Role => "admin";
}