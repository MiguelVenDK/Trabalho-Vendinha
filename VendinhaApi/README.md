# API Vendinha

Sobre o projeto, ele consiste em uma API desenvolvida em **.NET (C#)** junto com **Entity Framework Core** e **SQLite**, feita para o controle de caderneta e dívidas de uma vendinha local.

A ideia é substituir o caderno por um sistema prático, organizado e com todas as regras de negócio bem protegidas.

---

## O que tem no projeto
- **.NET Core / C#**
- **Entity Framework Core (ORM)** para gerenciar o banco de dados
- **SQLite** para persistir os dados localmente de forma simples[cite: 1, 2]
- **Swagger / Swashbuckle** para você testar todas as rotas visualmente direto no navegador

---

## Como o sistema funciona
O projeto foi feito para cobrir cada detalhe do cenário 1
- **Gestão de Clientes (CRUD completo)**: Dá para cadastrar, consultar, atualizar e remover clientes
- **Dados e Validações Inteligentes**: O sistema valida nome completo, e-mail e garante que o **CPF seja válido** e único ( não da pra cadastrar a mesma pessoa duas vezes!). Além disso, ele calcula a idade automaticamente com base na data de nascimento.
- **Controle de Dívidas**: Cada cliente pode ter registros de débitos. A dívida armazena o valor, a situação (paga ou pendente), a data de criação e a data de pagamento.
- **A Regra da Dívida Única**: Para manter as contas em ordem, o sistema bloqueia novos fiados caso o cliente já tenha **uma dívida em aberto**. Só é possível fazer novas compras após quitar a anterior.
- **Listagem**: A rota de clientes conta com paginação (trazendo de 10 em 10), busca flexível por nome, ordenação automática colocando quem mais deve no topo, e exibe a idade atualizada junto com o total do débito.

---

##  Como rodar o projeto

### O que você precisa ter instalado:
- O [.NET SDK](https://dotnet.microsoft.com/) (versão 8.0 ou superior)

### Passo a passo rápido:
1. Clone este repositório ou baixe os arquivos para o seu computador.
2. Abra o terminal na pasta do projeto (`VendinhaApi`):
   ```bash
   cd VendinhaApi -> dotnet run -> vai gerar o link http -> clique no link e sera redirecionado para o navegador.

# Observação:
- quando for rodar o site e for colocar um CPF para teste, ele precisa de um CPF válido, recomendo entrar em algum site que gere CPFs para fazer o teste.