# Stock Manager

Sistema de controle de estoque com movimentações auditáveis.
O saldo de um produto não é armazenado: é derivado do histórico
imutável de entradas, saídas e ajustes.

Projeto de estudo — desenvolvido sem assistência de IA na escrita do código.

## Stack

- **Backend:** C# / ASP.NET Core / Entity Framework Core
- **Frontend:** React
- **Banco:** MySQL

## Documentação

- [Requisitos](docs/requisitos.md)
- [Regras de integridade e domínio](docs/regras-integridade.md)
- [Diagrama entidade-relacionamento](docs/der.png)

## Como rodar

### Pré-requisitos

- .NET SDK 8
- MySQL 8.0 ou superior
- Node.js _(a partir da etapa de front-end)_

### Banco de dados

Antes de rodar a aplicação, crie o schema vazio:

```sql
CREATE DATABASE stockmanager;
```

As tabelas são criadas pelas migrations do EF Core — não crie nada manualmente.

| Configuração | Valor padrão |
|---|---|
| Host | localhost |
| Porta | 3306 |
| Schema | stockmanager |

### Configuração local

A connection string não é versionada. Após clonar o repositório,
configure-a via User Secrets:

```bash
cd src/backend/StockManager
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=stockmanager;User=seu_usuario;Password=sua_senha;"
```

O arquivo `appsettings.Example.json` mostra a estrutura esperada
da configuração. Ele serve apenas como referência — não é lido
pela aplicação.

### API

```bash
cd src/backend/StockManager
dotnet run
```

A API sobe em `https://localhost:7286`.
A documentação Swagger fica na raiz, disponível apenas em desenvolvimento.

### Front-end

_(a preencher)_

## Convenções

### Branches

`feature/est-XX-descricao-curta` — uma branch por card do Trello.

### Commits

Conventional Commits: `feat:`, `fix:`, `docs:`, `refactor:`, `test:`, `chore:`.

## Status

Em desenvolvimento — Sprint 1.