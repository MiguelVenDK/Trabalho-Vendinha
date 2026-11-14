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

Este repositório contém o código-fonte de uma Web API desenvolvida em C# com .NET 8, estruturada em camadas, utilizando Entity Framework Core e banco de dados SQLite para gerenciamento de clientes e dívidas.

## 📋 Pré-requisitos

Para rodar o codigo é preciso uma máquina que possui apenas o VS Code instalado, você precisará instalar os seguintes itens:

1. **Git**: Para clonar o repositório de código-fonte.
2. **.NET 8 SDK**: O kit de desenvolvimento oficial do .NET (necessário para compilar e executar o código C#). Você pode baixar gratuitamente no site oficial da Microsoft: [Baixar .NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0). ou .NET superior.

## 🧩 Extensões Recomendadas para o VS Code

Para visualizar e depurar o código com facilidade no VS Code, instale as seguintes extensões na aba de Extensões (`Ctrl+Shift+X`):
- **C#** (fornecida pela Microsoft) - Suporte a sintaxe, *IntelliSense* e depuração.
- **C# Dev Kit** (fornecida pela Microsoft) - Gerenciamento moderno de projetos C#.

---

## 🚀 Como Executar o Projeto

Siga os passos abaixo no terminal da sua máquina:

1. **Clone o repositório:**
   ```bash
   git clone <url-do-seu-repositorio>
   cd <nome-da-pasta-do-projeto> / Provavelmente vai ser Trabalho-Vendinha.

### Passo a passo rápido:
1. Clone este repositório ou baixe os arquivos para o seu computador.
2. Abra o terminal na pasta do projeto (`VendinhaApi`):
   ```bash
   cd Trabalho-Vendinha -> cd VendinhaApi -> dotnet run -> vai gerar o link http -> copie link gerado e cole no navegador.

# Observação:
- quando for rodar o site e for colocar um CPF para teste, ele precisa de um CPF válido, recomendo entrar em algum site que gere CPFs para fazer o teste.