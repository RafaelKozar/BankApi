# BankApi

API REST simples para operações bancárias (depósito, saque, transferência e consulta de saldo), construída em **.NET 10** seguindo o padrão **CQRS** com **MediatR**.

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) ou superior.

Para verificar se já está instalado e qual a versão:

```powershell
dotnet --version
```

Se o comando não for reconhecido ou a versão for anterior à 10, baixe e instale o SDK no link acima antes de continuar.

## Arquitetura

- **BankApi.Api** — projeto principal (Web API, ASP.NET Core).
- **BankApi.Tests** — projeto de testes unitários (xUnit + Moq).

### Padrões utilizados

- **CQRS (MediatR)**: cada operação é modelada como um `Command` ou `Query` com seu respectivo `Handler`.
- **Result Pattern**: os handlers retornam `Result<T>` (sucesso ou `Error`) em vez de lançar exceções para fluxos de erro esperados (ex.: conta não encontrada).
- **Repositório em memória**: `IAccountRepository` é implementado por `InMemoryAccountRepository`, que usa `ConcurrentDictionary` e `SemaphoreSlim` para garantir thread-safety em operações concorrentes na mesma conta (e evitar deadlock em transferências, ordenando os locks pelo menor/maior ID).

### Estrutura de pastas

```
src/BankApi.Api/
├── Controllers/          # Endpoints HTTP
├── Domain/
│   ├── Commands/         # Commands + Handlers (Deposit, Withdraw, Transfer, Reset, Event)
│   ├── Query/             # Queries + Handlers (GetAccount)
│   ├── DTOs/              # Objetos de transferência de dados
│   ├── Models/             # Entidades de domínio (Account)
│   ├── Repositories/      # Contratos (IAccountRepository)
│   └── Results/            # Result<T>, Error, ErrorType
└── Infrastructure/
    ├── InMemoryAccountRepository.cs
    └── Results/            # Extensões para converter Result<T> em ActionResult
```

## Endpoints

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/event` | Executa uma operação genérica (`deposit`, `withdraw` ou `transfer`) via `EventAcountCommand` |
| `GET` | `/balance?account_id={id}` | Consulta o saldo de uma conta |
| `POST` | `/reset` | Reseta o estado do repositório (remove todas as contas) |

## Como executar

```powershell
dotnet run --project src/BankApi.Api/BankApi.Api.csproj
```

A documentação Swagger fica disponível em `/swagger` durante o desenvolvimento.

## Como rodar os testes

```powershell
dotnet test src/BankApi.Tests/BankApi.Tests.csproj
```

Os testes cobrem:

- **Domain/Models**: regras do `Account` (depósito e saque).
- **Domain/Commands** e **Domain/Query**: handlers com `IAccountRepository` mockado (Moq).
- **Infrastructure**: comportamento real do `InMemoryAccountRepository`, incluindo cenários de concorrência (depósitos/saques/transferências simultâneas).
