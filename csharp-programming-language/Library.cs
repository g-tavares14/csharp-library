namespace csharp_programming_language;

public class Library
{
    private BookManager bookManager;
    private UserManager userManager = new UserManager();
    private LoanManager loanManager = new LoanManager();

    public Library()
    {
        bookManager = new BookManager(loanManager);
    }

    public void Run()
    {
        int option;

        do
        {
            Console.WriteLine("1 - Cadastrar | 2 - Listar | 3 - Buscar | 4 - Emprestar | 5 - Devolver | 6 - Remover | 7 - Editar | 8 - Cadastrar usuário | 9 - Listar usuários | 10 - Empréstimos ativos | 11 - Empréstimos de um usuário | 0 - Sair");

            while (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Digite um número válido.");
            }

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

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
        string title = Prompt("Título do livro: ");
        string author = Prompt("Autor do livro: ");
        string genre = Prompt("Gênero do livro: ");

        Book book = new Book
        {
            Title = title,
            Author = author,
            Genre = genre,
        };

        bookManager.AddBook(book);
    }

    private void ShowBooks()
    {
        Console.WriteLine("Listar um livro");
        bookManager.ListBooks();
    }

    private void SearchBook()
    {
        string bookTitle = Prompt(" Buscar um livro: ");
        Book? found = bookManager.SearchBook(bookTitle);

        if (found == null)
        {
            Console.WriteLine("Nenhum livro encontrado");
        }
        else
        {
            Console.WriteLine(found.Title);
        }
    }

    private void BorrowBook()
    {
        string borrowTitle = Prompt("Emprestar um livro: ");
        string borrowEmail = Prompt("Email do usuário: ");

        Book? bookToBorrow = bookManager.SearchBook(borrowTitle);
        User? borrower = userManager.SearchUser(borrowEmail);

        if (bookToBorrow == null)
        {
            Console.WriteLine("Livro não encontrado");
        }
        else if (borrower == null)
        {
            Console.WriteLine("Usuário não encontrado");
        }
        else if (loanManager.BorrowBook(bookToBorrow, borrower))
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
        string returnTitle = Prompt("Devolver um livro: ");

        Book? bookToReturn = bookManager.SearchBook(returnTitle);

        if (bookToReturn == null)
        {
            Console.WriteLine("Livro não encontrado");
            return;
        }

        Loan? returnedLoan = loanManager.ReturnBook(bookToReturn);

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
        string deleteTitle = Prompt("Remover um livro: ");
        bookManager.DeleteBook(deleteTitle);
    }

    private void UpdateBook()
    {
        string updateTitle = Prompt("Editar um livro: ");
        string newTitle = Prompt("Novo título do livro: ");
        string newAuthor = Prompt("Novo autor do livro: ");
        string newGenre = Prompt("Novo gênero do livro: ");
        bookManager.UpdateBook(updateTitle, newTitle, newAuthor, newGenre);
    }

    private void RegisterUser()
    {
        Console.WriteLine("Cadastrar um usuário");
        string userName = Prompt("Nome do usuário: ");
        string userEmail = Prompt("Email do usuário: ");
        string isAdmin = Prompt("É administrador? (s/n): ");

        User user;

        if (isAdmin.Trim().ToLower() == "s")
        {
            user = new Admin { Name = userName, Email = userEmail };
        }
        else
        {
            user = new User { Name = userName, Email = userEmail };
        }

        userManager.AddUser(user);
    }

    private void ShowUsers()
    {
        Console.WriteLine("Lista de usuários");
        userManager.ListUsers();
    }

    private void ShowActiveLoans(DateOnly today)
    {
        Console.WriteLine("Empréstimos ativos");
        List<Loan> activeLoans = loanManager.GetActiveLoans();

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
        string loansEmail = Prompt("Email do usuário: ");
        User? loansUser = userManager.SearchUser(loansEmail);

        if (loansUser == null)
        {
            Console.WriteLine("Usuário não encontrado");
            return;
        }

        List<Loan> userLoans = loanManager.GetActiveLoans(loansUser);

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
