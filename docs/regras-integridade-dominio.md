# Regras de Integridade e Domínio

> Complementa o DER. Aqui ficam apenas as regras que **não cabem no schema**:
> domínios fechados, obrigatoriedade condicional, imutabilidade e unicidade de negócio.
> Tipos, tamanhos e índices são decididos direto na migration (EST-08/EST-09).

---

## 1. Domínios fechados (Enums)

Valores a serem persistidos em MAIÚSCULO, sem acento.

### Tipo_movimentacao
| Valor | Significado | Efeito no saldo |
|---|---|---|
| `ENTRADA` | Recebimento de mercadoria | Positivo |
| `SAIDA` | Baixa de mercadoria | Negativo |
| `AJUSTE` | Reconciliação com contagem física | Positivo ou negativo |
| `ESTORNO` | Anulação de uma movimentação anterior | Oposto ao da original |

### Motivo_saida
| Valor | Significado |
|---|---|
| `VENDA` | Saída por venda ao cliente |
| `CONSUMO_INTERNO` | Uso próprio da operação |
| `PERDA` | Avaria, quebra, vencimento |
| `DEVOLUCAO_FORNECEDOR` | Retorno de mercadoria ao fornecedor |

### Unidade_medida
| Valor | Significado |
|---|---|
| `UN` | Unidade |
| `CX` | Caixa |
| `PC` | Pacote |

> **Decisão:** o sistema trabalha apenas com itens contáveis inteiros. Unidades
> fracionáveis (KG, L, M) estão fora do MVP. Por isso `Quantidade` e
> `Estoque_minimo` são inteiros, e nenhuma unidade do domínio admite fração.

### Status (Usuario, Produto, Categoria, Fornecedor)
| Valor | Significado |
|---|---|
| `ATIVO` | Disponível para uso e vínculo |
| `INATIVO` | Preservado para histórico, bloqueado para novo uso |

---

## 2. Obrigatoriedade condicional

O banco não expressa bem estas regras com `NOT NULL`. Elas vivem na camada de
aplicação; opcionalmente reforçadas por `CHECK` no MySQL 8.

| # | Coluna | Regra |
|---|---|---|
| IC01 | `Motivo_saida` | Obrigatório quando `Tipo_movimentacao = SAIDA`. Deve ser nulo nos demais tipos. |
| IC02 | `Id_fornecedor` | Obrigatório quando `Tipo_movimentacao = ENTRADA`. Deve ser nulo nos demais tipos. |
| IC03 | `Id_movimentacao_estornada` | Obrigatório quando `Tipo_movimentacao = ESTORNO`. Deve ser nulo nos demais tipos. |
| IC04 | `Desc_movimentacao` | Obrigatório quando `Tipo_movimentacao = AJUSTE` (justificativa — RF17). Opcional nos demais. |

**Atenção:** cada regra tem duas metades. "Obrigatório quando X" e "proibido
quando não-X". Implementar só a primeira permite uma saída com fornecedor
preenchido — dado sem sentido que ninguém percebe até virar bug de relatório.

---

## 3. Imutabilidade

| # | Alvo | Regra |
|---|---|---|
| IM01 | Movimentação | Nenhuma coluna pode ser alterada após a inserção. Correção se faz por estorno (RN02). |
| IM02 | Movimentação | Nunca é excluída fisicamente. |
| IM03 | `Data_registro` | Preenchida pelo sistema no instante da inserção. Nunca informada pelo usuário. |
| IM04 | `Saldo_anterior` | Calculado no momento da inserção e congelado. Nunca recalculado. |
| IM05 | Produto | Não pode ser excluído fisicamente se possuir movimentações (RF12). |

**Consequência prática:** não deve existir endpoint de UPDATE nem DELETE para
movimentação. A ausência do endpoint é a garantia mais forte que existe.

---

## 4. Unicidade de negócio

| # | Alvo | Regra |
|---|---|---|
| UN01 | `Produtos.Sku` | Único em toda a tabela, inclusive entre produtos inativos. |
| UN02 | `Usuarios.Email` | Único. |
| UN03 | `Usuarios.CPF` | Único. |
| UN04 | `Categorias.Nome_categoria` | Único. |
| UN05 | `Fornecedor.CNPJ_fornecedor` | Único. |
| UN06 | `Movimentacoes.Id_movimentacao_estornada` | Único quando preenchido — é o que garante a RN08 (uma movimentação só é estornada uma vez). |

---

## 5. Regras de saldo

| # | Regra |
|---|---|
| SL01 | O saldo de um produto é a soma dos efeitos de todas as suas movimentações. |
| SL02 | `Quantidade` guarda sempre o **efeito no saldo**, nunca a quantidade digitada pelo operador em um ajuste. |
| SL03 | Em um ajuste, o operador informa a contagem física; a aplicação calcula a diferença e persiste o efeito. |
| SL04 | O saldo nunca pode ser negativo, em nenhum tipo de movimentação, inclusive estorno (RN03, RF24). |
| SL05 | A verificação de saldo e a inserção da movimentação ocorrem na mesma transação, com bloqueio que impeça duas saídas concorrentes de furarem o limite (RNF02, RNF03). |
| SL06 | O extrato é ordenado por `Data_registro`, não por `Data_movimentacao`. Isso mantém `Saldo_anterior` sempre coerente com a ordem exibida, mesmo com lançamentos retroativos. |

---

## 6. Outras validações

| # | Regra |
|---|---|
| VL01 | `Quantidade` informada pelo operador deve ser maior que zero (RF18). |
| VL02 | `Data_movimentacao` não pode ser futura (RN12). Pode ser retroativa. |
| VL03 | Produto inativo não aceita novas movimentações (RN09). |
| VL04 | Categoria e fornecedor não podem ser inativados se houver produto ativo vinculado (RF07). |
| VL05 | `Estoque_minimo` não pode ser negativo. |
| VL06 | Produto está em situação crítica quando saldo ≤ `Estoque_minimo` (RN10). Produtos inativos não entram na lista. |
| VL07 | Toda movimentação registra o usuário autenticado que a criou (RN11). |

---

## 7. Pendências para a migration

Decidir ao escrever o schema, não antes:

- Tamanho de cada `Varchar`
- Enum nativo do MySQL ou coluna de texto com `CHECK` — pesar a dificuldade de alterar depois
- Índices nas colunas usadas em filtro e ordenação: `Sku`, `Nome_produto`, `Id_categoria`, `Status_produto`, `Id_produto` + `Data_registro` em movimentações
- Comportamento das FKs em exclusão (`RESTRICT` é o esperado aqui)
