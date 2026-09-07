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

_(a preencher)_

### Pré-requisitos

### Banco de dados

**Requisito:** MySQL 8.0 ou superior.

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

A connection string não é versionada. Veja a seção de configuração local.

### API

### Front-end

## Convenções

### Branches

`feature/est-XX-descricao-curta` — uma branch por card do Trello.

### Commits

Conventional Commits: `feat:`, `fix:`, `docs:`, `refactor:`, `test:`, `chore:`.

## Status

Em desenvolvimento — Sprint 1.