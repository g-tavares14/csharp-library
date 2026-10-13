using csharp_programming_language;

class Program
{
    static void Main()
    {
        int option;

        Library library = new Library();
        UserManager userManager = new UserManager();
        LoanManager loanManager = new LoanManager();

        do
        {
            Console.WriteLine("1 - Cadastrar | 2 - Listar | 3 - Buscar | 4 - Emprestar | 5 - Devolver | 6 - Remover | 7 - Editar | 8 - Cadastrar usuário | 9 - Listar usuários | 0 - Sair");

            while (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Digite um número válido.");
            }

            switch (option)
            {
                case 1:
                    Console.WriteLine("Cadastrar um livro");
                    Console.WriteLine("Título do livro: ");
                    string title = Console.ReadLine() ?? "";
                    Console.WriteLine("Autor do livro: ");
                    string author = Console.ReadLine() ?? "";
                    Console.WriteLine("Gênero do livro: ");
                    string genre = Console.ReadLine() ?? "";

                    Book book = new Book
                    {
                        Title = title,
                        Author = author,
                        Genre = genre,
                    };

                    library.AddBook(book);

                    break;
                case 2:
                    Console.WriteLine("Listar um livro");
                    library.ListBooks();

                    break;
                case 3:
                    Console.WriteLine(" Buscar um livro: ");
                    string bookTitle = Console.ReadLine() ?? "";
                    Book? found = library.SearchBook(bookTitle);

                    if (found == null)
                    {
                        Console.WriteLine("Nenhum livro encontrado");
                    }
                    else
                    {
                        Console.WriteLine(found.Title);
                    }

                    break;
                case 4:
                    Console.WriteLine("Emprestar um livro: ");
                    string borrowTitle = Console.ReadLine() ?? "";
                    Console.WriteLine("Email do usuário: ");
                    string borrowEmail = Console.ReadLine() ?? "";

                    Book? bookToBorrow = library.SearchBook(borrowTitle);
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

                    break;
                case 5:
                    Console.WriteLine("Devolver um livro: ");
                    string returnTitle = Console.ReadLine() ?? "";

                    Book? bookToReturn = library.SearchBook(returnTitle);

                    if (bookToReturn == null)
                    {
                        Console.WriteLine("Livro não encontrado");
                    }
                    else if (loanManager.ReturnBook(bookToReturn))
                    {
                        Console.WriteLine($"Livro {bookToReturn.Title} devolvido!");
                    }
                    else
                    {
                        Console.WriteLine($"O livro {bookToReturn.Title} não está emprestado.");
                    }

                    break;
                case 6:
                    Console.WriteLine("Remover um livro: ");
                    string deleteTitle = Console.ReadLine() ?? "";
                    library.DeleteBook(deleteTitle);

                    break;
                case 7:
                    Console.WriteLine("Editar um livro: ");
                    string updateTitle = Console.ReadLine() ?? "";
                    Console.WriteLine("Novo título do livro: ");
                    string newTitle = Console.ReadLine() ?? "";
                    Console.WriteLine("Novo autor do livro: ");
                    string newAuthor = Console.ReadLine() ?? "";
                    Console.WriteLine("Novo gênero do livro: ");
                    string newGenre = Console.ReadLine() ?? "";
                    library.UpdateBook(updateTitle, newTitle, newAuthor, newGenre);

                    break;
                case 8:
                    Console.WriteLine("Cadastrar um usuário");
                    Console.WriteLine("Nome do usuário: ");
                    string userName = Console.ReadLine() ?? "";
                    Console.WriteLine("Email do usuário: ");
                    string userEmail = Console.ReadLine() ?? "";
                    Console.WriteLine("É administrador? (s/n): ");
                    string isAdmin = Console.ReadLine() ?? "";

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

                    break;
                case 9:
                    Console.WriteLine("Lista de usuários");
                    userManager.ListUsers();

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
}