namespace csharp_programming_language;

public class UserManager
{
    private Dictionary<Guid, User> _users = new Dictionary<Guid, User>();

    public void AddUser(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
        {
            Console.WriteLine("Nome e email são obrigatórios");
            return;
        }

        foreach (var existingUser in _users.Values)
        {
            if (string.Equals(user.Email, existingUser.Email, StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine($"O email {user.Email} já está cadastrado");
                return;
            }
        }

        _users.Add(user.Id, user);
        Console.WriteLine($"O usuário {user.Name} foi cadastrado como {user.Role}");
    }

    public void ListUsers()
    {
        if (_users.Count == 0)
        {
            Console.WriteLine("Nenhum usuário cadastrado");
        }

        foreach (var user in _users.Values)
        {
            Console.WriteLine($"Usuário é {user.Name} com email {user.Email} e com role {user.Role}");
        }
    }

    public User? SearchUser(string email)
    {
        foreach (var user in _users.Values)
        {
            if (user.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase))
            {
                return user;
            }
        }

        return null;
    }
}