# Sistema de Biblioteca em C#

Projeto de estudo para praticar a linguagem **C#** e lógica de programação. É um sistema de biblioteca que roda no console, com cadastro, listagem, busca, empréstimo e devolução de livros.

> Este repositório registra o meu progresso aprendendo C#. O código muda conforme eu aprendo.

## Funcionalidades

- Cadastrar livros (título, autor e gênero)
- Listar os livros com o status (disponível ou emprestado)
- Buscar um livro pelo título (ignora maiúsculas e minúsculas)
- Emprestar um livro (a devolução fica prevista para 7 dias depois)
- Devolver um livro
- Menu em loop com validação da opção digitada

## Conceitos praticados

- Classes e propriedades (`get`/`set`)
- `Dictionary<TKey, TValue>` e `Guid` como identificador
- Laços (`do/while`, `foreach`) e `switch`
- Métodos com retorno (`bool`, tipos anuláveis como `Book?`)
- Leitura e validação de entrada com `Console.ReadLine` e `int.TryParse`
- Interpolação de strings e operador ternário
- Organização do código em arquivos separados

## Estrutura

```
csharp-programming-language/
├── Program.cs   # menu e leitura da entrada do usuário
├── Library.cs   # guarda os livros e contém a lógica (adicionar, listar, buscar, emprestar, devolver)
└── Book.cs      # entidade Book (os dados de um livro)
```

## Como executar

Requer o [.NET SDK](https://dotnet.microsoft.com/download) (o projeto usa o .NET 10).

```bash
git clone https://github.com/g-tavares14/csharp-programming-language.git
cd csharp-programming-language
dotnet run --project csharp-programming-language
```

## Próximos passos

- Impedir o cadastro de livros com o mesmo título
- Mostrar a data de devolução na listagem
- Salvar os livros em arquivo para não perdê-los ao fechar o programa

## Sobre o uso de IA

Este é um projeto de aprendizado, e eu uso o Claude (IA da Anthropic) como **tutor**:

- A IA me ajuda a **entender os conceitos e os algoritmos**, tira dúvidas de sintaxe e revisa o que eu escrevi, apontando erros e melhorias.
- O código foi **escrito por mim**. As decisões de como organizar o programa também são minhas.
- Em alguns pontos a IA aplicou edições pequenas e mecânicas a meu pedido: a ligação das opções de empréstimo e devolução no menu, a correção de textos da tela e a renomeação dos identificadores para inglês.
