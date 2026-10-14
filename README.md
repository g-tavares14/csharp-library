# Sistema de Biblioteca em C#

Projeto de estudo para praticar a linguagem **C#** e lógica de programação. É um sistema de biblioteca que roda no console, com livros, usuários, empréstimos com prazo e multa, consultas com LINQ, dados salvos em arquivos JSON e testes automáticos.

> Este repositório registra o meu progresso aprendendo C#. O código muda conforme eu aprendo.

## Funcionalidades

- Cadastrar, listar, buscar, editar e remover livros (título, autor e gênero), sem títulos repetidos
- Cadastrar usuários comuns e administradores, sem e-mails repetidos
- Emprestar e devolver livros (um empréstimo ativo por livro, prazo de 7 dias)
- Multa de R$ 2,00 por dia de atraso, mostrada nas listas e na devolução
- Listar os empréstimos ativos, os de um usuário e a contagem por usuário
- Filtrar livros por título, autor ou gênero
- Estatísticas (total, emprestados e disponíveis) e livros agrupados por título, autor ou gênero
- Os dados são salvos em arquivos JSON e voltam quando o programa é aberto de novo
- Menu em loop com validação da opção digitada

## Conceitos praticados

- Classes, propriedades (`get`/`set`/`init`), `required` e herança (`User` e `Admin`)
- `Dictionary<TKey, TValue>` e `Guid` como identificador
- Entidades (só dados) separadas dos gerenciadores (regras): `BookManager`, `UserManager` e `LoanManager`
- Construtores primários, campos `readonly` e `DateOnly`
- LINQ: `Where`, `OrderBy`, `Any`, `FirstOrDefault`, `SingleOrDefault`, `Count`, `GroupBy` e `Select`
- Lambdas, tuplas nomeadas, `enum` e `switch` como expressão
- Tipos anuláveis (`Book?`, `DateOnly?`) e os operadores `?`, `??` e `?.`
- Persistência com `System.Text.Json`, incluindo polimorfismo (`Admin` volta como administrador)
- Tratamento de arquivo corrompido: o arquivo é guardado com a extensão `.corrompido` e o programa começa vazio
- Testes automáticos com **xUnit**: `[Fact]`, `[Theory]`, `Assert` e o padrão Arrange, Act, Assert

## Estrutura

```
csharp-programming-language/            # o programa
├── Program.cs        # ponto de entrada
├── Library.cs        # menu: lê a entrada e chama os gerenciadores
├── BookManager.cs    # regras dos livros (cadastro, busca, filtro, estatísticas)
├── UserManager.cs    # regras dos usuários
├── LoanManager.cs    # regras dos empréstimos e devoluções
├── DataStore.cs      # leitura e gravação dos arquivos JSON
├── Book.cs           # entidade Book
├── User.cs           # entidades User e Admin
├── Loan.cs           # entidade Loan (prazo, atraso e multa)
├── LoanData.cs       # formato do empréstimo no arquivo (só os Ids)
└── BookFilter.cs     # enum com os campos de filtro

csharp-programming-language.Tests/      # os testes (xUnit)
├── LoanTests.cs
├── LoanManagerTests.cs
├── BooksTests.cs
├── BookReportsTests.cs
├── UserTests.cs
└── DataStoreTests.cs
```

## Como executar

Requer o [.NET SDK](https://dotnet.microsoft.com/download) (o projeto usa o .NET 10).

```bash
git clone https://github.com/g-tavares14/csharp-programming-language.git
cd csharp-programming-language
dotnet run --project csharp-programming-language
```

Os arquivos `books.json`, `users.json` e `loans.json` são criados na mesma pasta do programa compilado (`bin/Debug/net10.0`). Para começar do zero, basta apagá-los.

## Como rodar os testes

```bash
dotnet test
```

## Próximos passos

- Limitar quantos livros cada usuário pode ter emprestados ao mesmo tempo (fazendo os testes antes, no estilo TDD)
- Renovar um empréstimo
- Usar o perfil de administrador para restringir ações, como remover livros

## Sobre o uso de IA

Este é um projeto de aprendizado, e eu uso o Claude (IA da Anthropic) como **tutor**:

- A IA me ajuda a **entender os conceitos e os algoritmos**, tira dúvidas de sintaxe e revisa o que eu escrevi, apontando erros e melhorias.
- O código do programa foi **escrito por mim**. As decisões de como organizar o programa também são minhas.
- Em alguns pontos a IA aplicou edições pequenas e mecânicas a meu pedido: a ligação das opções no menu, a correção de textos da tela, a renomeação dos identificadores para inglês e a ligação da leitura e gravação dos arquivos na `Library`.
- Nos **testes**, escrevi os primeiros com a orientação da IA. O restante da suíte (a maior parte dos 62 testes) a IA escreveu a meu pedido. Ela também verificou se os testes falham quando as regras são quebradas de propósito.
