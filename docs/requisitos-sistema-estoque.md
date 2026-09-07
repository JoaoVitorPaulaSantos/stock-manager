# Sistema de Controle de Estoque — Documento de Requisitos (MVP)

> Versão 0.1 — base para modelagem. Este documento descreve **o quê** o sistema faz,
> nunca **como** ele é implementado. Nenhum requisito aqui menciona tabela, coluna ou
> endpoint de propósito: isso é decisão da fase de modelagem.

---

## 1. Visão do produto

Sistema web para controle de estoque de uma pequena operação (loja, oficina ou
almoxarifado). O saldo de cada produto nunca é digitado diretamente: ele é
consequência das movimentações registradas. O objetivo é que o histórico seja
sempre auditável — a qualquer momento é possível responder "por que este produto
tem 14 unidades?".

**Persona única do MVP:** *Operador de estoque* — pessoa que dá entrada em
mercadoria recebida, dá baixa em mercadoria vendida/consumida e acompanha o que
está acabando.

---

## 2. Requisitos Funcionais

### Autenticação e conta

| ID | Requisito |
|---|---|
| RF01 | O sistema deve permitir que um usuário se autentique com e-mail e senha. |
| RF02 | O sistema deve manter a sessão do usuário autenticado por tempo determinado, exigindo nova autenticação após a expiração. |
| RF03 | O sistema deve impedir o acesso a qualquer funcionalidade de estoque sem autenticação. |
| RF04 | O sistema deve permitir que o usuário encerre a sessão. |

### Cadastros de apoio

| ID | Requisito |
|---|---|
| RF05 | O sistema deve permitir cadastrar, consultar, editar e inativar **categorias** de produto. |
| RF06 | O sistema deve permitir cadastrar, consultar, editar e inativar **fornecedores**. |
| RF07 | O sistema deve impedir a inativação de uma categoria ou fornecedor que esteja vinculado a produtos ativos. |

### Produtos

| ID | Requisito |
|---|---|
| RF08 | O sistema deve permitir cadastrar um produto com, no mínimo: código interno (SKU), nome, unidade de medida, categoria e estoque mínimo. |
| RF09 | O sistema deve garantir que o código interno (SKU) seja único. |
| RF10 | O sistema deve permitir editar os dados cadastrais de um produto. |
| RF11 | O sistema deve permitir inativar um produto, mantendo seu histórico de movimentações acessível. |
| RF12 | O sistema deve impedir a exclusão definitiva de um produto que possua movimentações registradas. |
| RF13 | O sistema deve listar produtos com paginação, busca por nome ou SKU e filtro por categoria e por situação (ativo/inativo). |
| RF14 | O sistema deve exibir, na listagem e no detalhe do produto, o saldo atual em estoque. |

### Movimentações — núcleo do sistema

| ID | Requisito |
|---|---|
| RF15 | O sistema deve permitir registrar uma **entrada** de estoque, informando produto, quantidade, data/hora, fornecedor e observação opcional. |
| RF16 | O sistema deve permitir registrar uma **saída** de estoque, informando produto, quantidade, data/hora, motivo e observação opcional. |
| RF17 | O sistema deve permitir registrar um **ajuste** de inventário, informando produto, quantidade contada, data/hora e justificativa obrigatória. |
| RF18 | O sistema deve rejeitar qualquer movimentação com quantidade menor ou igual a zero. |
| RF19 | O sistema deve rejeitar uma saída cuja quantidade seja superior ao saldo disponível do produto naquele momento. |
| RF20 | O sistema deve registrar, em toda movimentação, qual usuário a realizou e o momento exato do registro. |
| RF21 | O sistema não deve permitir editar nem excluir uma movimentação já registrada. |
| RF22 | O sistema deve permitir **estornar** uma movimentação, gerando um novo registro que anula o efeito do original e mantém ambos visíveis no histórico. |
| RF23 | O sistema deve impedir o estorno de uma movimentação que já tenha sido estornada. |
| RF24 | O sistema deve impedir um estorno que resulte em saldo negativo para o produto. |
| RF25 | O sistema deve exibir o extrato de movimentações de um produto, em ordem cronológica, com o saldo resultante após cada movimentação. |
| RF26 | O sistema deve permitir consultar todas as movimentações com filtro por período, tipo e produto. |

### Painel e alertas

| ID | Requisito |
|---|---|
| RF27 | O sistema deve exibir um painel com: total de produtos ativos, quantidade de produtos abaixo do estoque mínimo e as movimentações mais recentes. |
| RF28 | O sistema deve sinalizar visualmente os produtos cujo saldo esteja igual ou abaixo do estoque mínimo. |
| RF29 | O sistema deve permitir listar todos os produtos em situação de estoque crítico. |

---

## 3. Requisitos Não Funcionais

### Qualidade de software

| ID | Requisito |
|---|---|
| RNF01 | O cálculo de saldo deve ser consistente: consultar o saldo de um produto duas vezes sem movimentação intermediária deve retornar sempre o mesmo valor. |
| RNF02 | Duas saídas simultâneas do mesmo produto não podem, juntas, levar o saldo abaixo de zero. |
| RNF03 | Uma movimentação deve ser registrada de forma atômica: ou todos os efeitos são persistidos, ou nenhum é. |
| RNF04 | As regras de negócio devem ser cobertas por testes automatizados, com atenção especial às regras de saldo e estorno. |

### Segurança

| ID | Requisito |
|---|---|
| RNF05 | Senhas devem ser armazenadas exclusivamente na forma de hash com algoritmo apropriado e salt. |
| RNF06 | Toda comunicação entre cliente e servidor deve ocorrer sobre HTTPS em ambiente produtivo. |
| RNF07 | Credenciais e strings de conexão não devem estar versionadas no repositório. |
| RNF08 | Toda entrada recebida pela API deve ser validada no servidor, independentemente de validação existente na interface. |

### Desempenho e uso

| ID | Requisito |
|---|---|
| RNF09 | Listagens devem responder em até 2 segundos com base de até 5.000 produtos e 50.000 movimentações. |
| RNF10 | Listagens não devem retornar a coleção completa: a paginação é obrigatória no servidor. |
| RNF11 | A interface deve ser utilizável em telas a partir de 360px de largura. |
| RNF12 | Mensagens de erro devem ser compreensíveis para o operador, sem expor detalhes técnicos internos. |

### Operação

| ID | Requisito |
|---|---|
| RNF13 | O sistema deve estar publicamente acessível na internet, com front-end e API hospedados e banco de dados gerenciado. |
| RNF14 | A estrutura do banco deve ser versionada por migrations, permitindo recriar o schema do zero. |
| RNF15 | Erros não tratados devem ser registrados em log com informação suficiente para diagnóstico. |
| RNF16 | O sistema deve expor um endpoint de verificação de saúde da aplicação. |

---

## 4. Regras de Negócio

| ID | Regra |
|---|---|
| RN01 | O saldo de um produto é sempre derivado do conjunto de suas movimentações, nunca informado diretamente pelo usuário. |
| RN02 | Movimentação é imutável: registrada, jamais é alterada ou apagada. Correção se faz por estorno. |
| RN03 | O saldo de um produto nunca pode ser negativo. |
| RN04 | Entrada aumenta o saldo; saída reduz o saldo. |
| RN05 | Ajuste de inventário reconcilia o saldo do sistema com a contagem física: o efeito do ajuste é a diferença entre o contado e o saldo atual. |
| RN06 | Ajuste exige justificativa textual obrigatória. |
| RN07 | Estorno gera uma movimentação de sentido oposto, vinculada à movimentação original. |
| RN08 | Uma movimentação só pode ser estornada uma única vez. |
| RN09 | Produto inativo não aceita novas movimentações, mas mantém saldo e histórico consultáveis. |
| RN10 | Um produto está em estado crítico quando seu saldo é menor ou igual ao seu estoque mínimo. |
| RN11 | Toda movimentação é atribuída ao usuário autenticado que a registrou. |
| RN12 | A data da movimentação pode ser retroativa, mas nunca futura. |

---

## 5. Histórias de Usuário

Formato: *Como \<papel\>, quero \<ação\>, para \<benefício\>.*
Critérios de aceite em Gherkin — use-os depois como base dos seus testes.

---

### US01 — Autenticar no sistema
**Como** operador de estoque,
**quero** entrar no sistema com meu e-mail e senha,
**para** que apenas pessoas autorizadas movimentem o estoque.

**Critérios de aceite**
```gherkin
Cenário: Credenciais válidas
  Dado que estou na tela de login
  Quando informo e-mail e senha corretos
  Então sou autenticado e direcionado ao painel

Cenário: Credenciais inválidas
  Dado que estou na tela de login
  Quando informo uma senha incorreta
  Então recebo uma mensagem genérica de credenciais inválidas
  E não é revelado se o e-mail existe ou não

Cenário: Acesso sem sessão
  Dado que não estou autenticado
  Quando tento acessar a lista de produtos
  Então sou redirecionado para o login
```

---

### US02 — Cadastrar produto
**Como** operador de estoque,
**quero** cadastrar um produto com SKU, nome, unidade, categoria e estoque mínimo,
**para** poder controlar sua entrada e saída.

**Critérios de aceite**
```gherkin
Cenário: Cadastro válido
  Dado que informei todos os campos obrigatórios
  Quando salvo o produto
  Então ele passa a aparecer na listagem
  E seu saldo inicial é zero

Cenário: SKU duplicado
  Dado que já existe um produto com o SKU "CX-100"
  Quando tento cadastrar outro produto com o mesmo SKU
  Então o cadastro é rejeitado com mensagem informando a duplicidade

Cenário: Estoque mínimo negativo
  Quando informo um estoque mínimo menor que zero
  Então o cadastro é rejeitado
```

---

### US03 — Registrar entrada de mercadoria
**Como** operador de estoque,
**quero** registrar a entrada de produtos recebidos de um fornecedor,
**para** que o saldo reflita o que chegou.

**Critérios de aceite**
```gherkin
Cenário: Entrada válida
  Dado que o produto "CX-100" tem saldo 10
  Quando registro uma entrada de 25 unidades
  Então o saldo passa a ser 35
  E a movimentação aparece no extrato com meu usuário e a data/hora

Cenário: Quantidade inválida
  Quando registro uma entrada com quantidade zero
  Então a movimentação é rejeitada

Cenário: Data futura
  Quando informo uma data de movimentação posterior a hoje
  Então a movimentação é rejeitada
```

---

### US04 — Registrar saída de mercadoria
**Como** operador de estoque,
**quero** dar baixa em produtos vendidos ou consumidos,
**para** que o saldo reflita o que saiu.

**Critérios de aceite**
```gherkin
Cenário: Saída válida
  Dado que o produto "CX-100" tem saldo 35
  Quando registro uma saída de 5 unidades
  Então o saldo passa a ser 30

Cenário: Saída maior que o saldo
  Dado que o produto "CX-100" tem saldo 30
  Quando registro uma saída de 31 unidades
  Então a movimentação é rejeitada
  E o saldo permanece 30

Cenário: Saída que zera o estoque
  Dado que o produto "CX-100" tem saldo 30
  Quando registro uma saída de 30 unidades
  Então o saldo passa a ser 0
  E a movimentação é aceita

Cenário: Produto inativo
  Dado que o produto "CX-100" está inativo
  Quando tento registrar uma saída
  Então a movimentação é rejeitada
```

---

### US05 — Ajustar estoque após inventário
**Como** operador de estoque,
**quero** informar a quantidade física contada de um produto,
**para** corrigir divergências entre o sistema e a prateleira.

**Critérios de aceite**
```gherkin
Cenário: Contagem menor que o sistema
  Dado que o produto "CX-100" tem saldo 30
  Quando informo contagem física de 28 com justificativa "avaria"
  Então o saldo passa a ser 28
  E é registrada uma movimentação de ajuste de -2

Cenário: Contagem maior que o sistema
  Dado que o produto "CX-100" tem saldo 28
  Quando informo contagem física de 31 com justificativa "recontagem"
  Então o saldo passa a ser 31

Cenário: Justificativa ausente
  Quando informo a contagem sem preencher a justificativa
  Então o ajuste é rejeitado
```

---

### US06 — Estornar movimentação incorreta
**Como** operador de estoque,
**quero** estornar uma movimentação lançada por engano,
**para** corrigir o saldo sem apagar o histórico.

**Critérios de aceite**
```gherkin
Cenário: Estorno de entrada
  Dado que registrei uma entrada de 25 unidades e o saldo é 35
  Quando estorno essa entrada
  Então o saldo volta a ser 10
  E as duas movimentações permanecem visíveis no extrato

Cenário: Estorno duplicado
  Dado que uma movimentação já foi estornada
  Quando tento estorná-la novamente
  Então a operação é rejeitada

Cenário: Estorno que deixaria saldo negativo
  Dado que registrei uma entrada de 25 e depois saídas que deixaram o saldo em 3
  Quando tento estornar a entrada de 25
  Então a operação é rejeitada
```

---

### US07 — Consultar extrato de um produto
**Como** operador de estoque,
**quero** ver todas as movimentações de um produto em ordem cronológica,
**para** entender como o saldo atual foi formado.

**Critérios de aceite**
```gherkin
Cenário: Extrato com saldo progressivo
  Dado que o produto teve entrada de 10, saída de 3 e entrada de 5
  Quando abro o extrato do produto
  Então vejo as três movimentações em ordem de data
  E os saldos resultantes 10, 7 e 12 respectivamente

Cenário: Produto sem movimentação
  Quando abro o extrato de um produto recém-cadastrado
  Então vejo uma mensagem de que não há movimentações
  E o saldo exibido é zero
```

---

### US08 — Acompanhar produtos em estoque crítico
**Como** operador de estoque,
**quero** ver quais produtos estão no limite ou abaixo do estoque mínimo,
**para** providenciar reposição antes de faltar.

**Critérios de aceite**
```gherkin
Cenário: Produto atinge o mínimo
  Dado que o produto "CX-100" tem estoque mínimo 5 e saldo 8
  Quando registro uma saída de 3 unidades
  Então o produto passa a constar na lista de estoque crítico

Cenário: Produto sai da situação crítica
  Dado que o produto "CX-100" está em estoque crítico
  Quando registro uma entrada que eleva o saldo acima do mínimo
  Então o produto deixa de constar na lista

Cenário: Produtos inativos
  Dado que existe um produto inativo com saldo abaixo do mínimo
  Quando consulto a lista de estoque crítico
  Então esse produto não é exibido
```

---

### US09 — Localizar produto rapidamente
**Como** operador de estoque,
**quero** buscar produtos por nome ou SKU e filtrar por categoria,
**para** encontrar o item que preciso movimentar sem rolar a lista inteira.

**Critérios de aceite**
```gherkin
Cenário: Busca por trecho do nome
  Dado que existem os produtos "Caixa Plástica" e "Caixa de Papelão"
  Quando busco por "caixa"
  Então ambos são exibidos

Cenário: Busca sem resultado
  Quando busco por um termo inexistente
  Então vejo uma mensagem de nenhum resultado encontrado

Cenário: Paginação
  Dado que existem 120 produtos cadastrados
  Quando abro a listagem
  Então vejo a primeira página com no máximo 20 itens
  E consigo navegar para as páginas seguintes
```

---

## 6. Fora de escopo do MVP

Registrado aqui para não virar tentação no meio do caminho:

- Múltiplos depósitos ou localizações físicas
- Custo médio ponderado, precificação e valorização de estoque
- Perfis de acesso e permissões diferenciadas
- Leitura de código de barras
- Exportação de relatórios em PDF ou Excel
- Upload de imagem de produto
- Controle de lote e validade
- Notificação por e-mail de estoque crítico
- Integração com nota fiscal

---

## 7. Perguntas em aberto

Decida antes de modelar — cada uma muda seu DER:

1. **Ajuste de inventário** — o operador informa a *quantidade contada* ou a *diferença*? O documento assumiu a contagem. Você concorda?
2. **Unidade de medida** — texto livre no produto ou entidade própria com cadastro?
3. **Fornecedor** — obrigatório na entrada ou opcional?
4. **Motivo de saída** — lista fixa (venda, consumo, perda, devolução) ou texto livre?
5. **Um produto pode ter mais de uma categoria?** O documento assumiu que não.
