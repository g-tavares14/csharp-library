namespace csharp_programming_language;

public class Library
{
    private readonly BookManager _bookManager;
    private readonly UserManager _userManager = new UserManager();
    private readonly LoanManager _loanManager = new LoanManager();

    public Library()
    {
        _bookManager = new BookManager(_loanManager);
    }

    public void Run()
    {
        int option;

        do
        {
            Console.WriteLine(
                "1 - Cadastrar | 2 - Listar | 3 - Buscar | 4 - Emprestar | 5 - Devolver | 6 - Remover | 7 - Editar | 8 - Cadastrar usuário | 9 - Listar usuários | 10 - Empréstimos ativos | 11 - Empréstimos de um usuário | 0 - Sair");

            while (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Digite um número válido.");
            }

            var today = DateOnly.FromDateTime(DateTime.Now);

            switch (option)
            {
                case 1:
                    RegisterBook();
                    break;
                case 2:
                    ShowBooks();
                    break;
                case 3:
                    SearchBook();
                    break;
                case 4:
                    BorrowBook();
                    break;
                case 5:
                    ReturnBook(today);
                    break;
                case 6:
                    DeleteBook();
                    break;
                case 7:
                    UpdateBook();
                    break;
                case 8:
                    RegisterUser();
                    break;
                case 9:
                    ShowUsers();
                    break;
                case 10:
                    ShowActiveLoans(today);
                    break;
                case 11:
                    ShowUserLoans(today);
                    break;
                case 0:
                    Console.WriteLine("Saindo...");
                    break;
                default:
                    Console.WriteLine("Opção inválida");
                    break;
            }
        } while (option != 0);
    }

    private static string Prompt(string message)
    {
        Console.WriteLine(message);
        return Console.ReadLine() ?? "";
    }

    private static string OverdueText(Loan loan, DateOnly today)
    {
        if (loan.DaysOverdue(today) > 0)
        {
            return $" - ATRASADO há {loan.DaysOverdue(today)} dia(s), multa de R$ {loan.CalculateFine(today):F2}";
        }

        return "";
    }

    private void RegisterBook()
    {
        Console.WriteLine("Cadastrar um livro");
        var title = Prompt("Título do livro: ");
        var author = Prompt("Autor do livro: ");
        var genre = Prompt("Gênero do livro: ");

        var book = new Book
        {
            Title = title,
            Author = author,
            Genre = genre,
        };

        _bookManager.AddBook(book);
    }

    private void ShowBooks()
    {
        Console.WriteLine("Listar um livro");
        _bookManager.ListBooks();
    }

    private void SearchBook()
    {
        var bookTitle = Prompt(" Buscar um livro: ");
        var found = _bookManager.SearchBook(bookTitle);

        Console.WriteLine(found == null ? "Nenhum livro encontrado" : found.Title);
    }

    private void BorrowBook()
    {
        var borrowTitle = Prompt("Emprestar um livro: ");
        var borrowEmail = Prompt("Email do usuário: ");

        var bookToBorrow = _bookManager.SearchBook(borrowTitle);
        var borrower = _userManager.SearchUser(borrowEmail);

        if (bookToBorrow == null)
        {
            Console.WriteLine("Livro não encontrado");
        }
        else if (borrower == null)
        {
            Console.WriteLine("Usuário não encontrado");
        }
        else if (_loanManager.BorrowBook(bookToBorrow, borrower))
        {
            Console.WriteLine($"Livro {bookToBorrow.Title} emprestado para {borrower.Name}!");
        }
        else
        {
            Console.WriteLine($"O livro {bookToBorrow.Title} já está emprestado.");
        }
    }

    private void ReturnBook(DateOnly today)
    {
        var returnTitle = Prompt("Devolver um livro: ");

        var bookToReturn = _bookManager.SearchBook(returnTitle);

        if (bookToReturn == null)
        {
            Console.WriteLine("Livro não encontrado");
            return;
        }

        var returnedLoan = _loanManager.ReturnBook(bookToReturn);

        if (returnedLoan == null)
        {
            Console.WriteLine($"O livro {bookToReturn.Title} não está emprestado.");
            return;
        }

        Console.WriteLine($"Livro {bookToReturn.Title} devolvido!");

        if (returnedLoan.DaysOverdue(today) > 0)
        {
            Console.WriteLine(
                $"Devolução com {returnedLoan.DaysOverdue(today)} dia(s) de atraso. Multa: R$ {returnedLoan.CalculateFine(today):F2}");
        }
    }

    private void DeleteBook()
    {
        var deleteTitle = Prompt("Remover um livro: ");
        _bookManager.DeleteBook(deleteTitle);
    }

    private void UpdateBook()
    {
        var updateTitle = Prompt("Editar um livro: ");
        var newTitle = Prompt("Novo título do livro: ");
        var newAuthor = Prompt("Novo autor do livro: ");
        var newGenre = Prompt("Novo gênero do livro: ");
        _bookManager.UpdateBook(updateTitle, newTitle, newAuthor, newGenre);
    }

    private void RegisterUser()
    {
        Console.WriteLine("Cadastrar um usuário");
        var userName = Prompt("Nome do usuário: ");
        var userEmail = Prompt("Email do usuário: ");
        var isAdmin = Prompt("É administrador? (s/n): ");

        var user = string.Equals(isAdmin.Trim(), "s", StringComparison.OrdinalIgnoreCase)
            ? new Admin { Name = userName, Email = userEmail }
            : new User { Name = userName, Email = userEmail };

        _userManager.AddUser(user);
    }

    private void ShowUsers()
    {
        Console.WriteLine("Lista de usuários");
        _userManager.ListUsers();
    }

    private void ShowActiveLoans(DateOnly today)
    {
        Console.WriteLine("Empréstimos ativos");
        var activeLoans = _loanManager.GetActiveLoans();

        if (activeLoans.Count == 0)
        {
            Console.WriteLine("Nenhum empréstimo ativo");
        }

        foreach (var activeLoan in activeLoans)
        {
            Console.WriteLine(
                $"{activeLoan.User.Name} está com o livro {activeLoan.Book.Title} até {activeLoan.DueDate}{OverdueText(activeLoan, today)}");
        }
    }

    private void ShowUserLoans(DateOnly today)
    {
        Console.WriteLine("Empréstimos de um usuário");
        var loansEmail = Prompt("Email do usuário: ");
        var loansUser = _userManager.SearchUser(loansEmail);

        if (loansUser == null)
        {
            Console.WriteLine("Usuário não encontrado");
            return;
        }

        var userLoans = _loanManager.GetActiveLoans(loansUser);

        if (userLoans.Count == 0)
        {
            Console.WriteLine($"{loansUser.Name} não tem nenhum livro emprestado");
        }

        foreach (var userLoan in userLoans)
        {
            Console.WriteLine($"{userLoan.Book.Title} até {userLoan.DueDate}{OverdueText(userLoan, today)}");
        }
    }
}
