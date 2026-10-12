using csharp_programming_language;

class Program
{
    static void Main()
    {
        int option;

        Library library = new Library();

        do
        {
            Console.WriteLine("1 - Cadastrar | 2 - Listar | 3 - Buscar | 4 - Emprestar | 5 - Devolver | 0 - Sair");

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

                    if (library.BorrowBook(borrowTitle))
                    {
                        Console.WriteLine("Livro emprestado!");
                    }
                    else
                    {
                        Console.WriteLine("Não foi possível emprestar o livro.");
                    }

                    break;
                case 5:
                    Console.WriteLine("Devolver um livro: ");
                    string returnTitle = Console.ReadLine() ?? "";

                    if (library.ReturnBook(returnTitle))
                    {
                        Console.WriteLine("Livro devolvido!");
                    }
                    else
                    {
                        Console.WriteLine("Não foi possível devolver o livro.");
                    }

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