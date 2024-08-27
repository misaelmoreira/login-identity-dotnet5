# Aplicação de Estudos Web .NET 5 com Identity e EF Core 

Esta aplicação web é desenvolvida em .NET 5 e utiliza o Entity Framework Core com banco de dados SQLite. A arquitetura da aplicação segue o padrão MVC (Model-View-Controller) e implementa autenticação de usuários com o ASP.NET Identity. Além disso, a aplicação está configurada para enviar e-mails para recuperação de senha utilizando Gmail, SendGrid e Sendinblue.

## Tecnologias Utilizadas

- **.NET 5**: Framework para desenvolvimento da aplicação.
- **ASP.NET Identity**: Sistema de autenticação e gerenciamento de usuários.
- **Entity Framework Core**: ORM para interação com o banco de dados.
- **SQLite**: Banco de dados leve e fácil de configurar.
- **Gmail, SendGrid e Sendinblue**: Serviços de envio de e-mails para recuperação de senha.

## Estrutura do Projeto

- **Controllers/**: Contém os controladores da aplicação que gerenciam as requisições.
- **Models/**: Contém as classes de modelo (entidades) utilizadas pelo Entity Framework e Identity.
- **Views/**: Contém as páginas de visualização (views) da aplicação.
- **Data/**: Contém o contexto do banco de dados e configurações do Entity Framework.
- **Services/**: Contém os serviços de envio de e-mail e outras funcionalidades adicionais.

## Como Executar

1. **Clone este repositório**:

    ```bash
    git clone https://github.com/seu-usuario/nome-do-repositorio.git
    ```

2. **Navegue até o diretório do projeto**:

    ```bash
    cd nome-do-repositorio
    ```

3. **Restaure as dependências do projeto**:

    ```bash
    dotnet restore
    ```

4. **Restaure as dependências das lib**:

    ```bash
    libman restore
    ```

5. **Configure o banco de dados**:
   - O projeto já está configurado para usar o SQLite. A string de conexão está definida no arquivo `appsettings.json`:

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Data Source=app.db"
      }
    }
    ```

6. **Configure os serviços de e-mail**:
   - Abra o arquivo `appsettings.json` e adicione as configurações para Gmail, SendGrid e Sendinblue:

    ```json
    {
      "EmailSettings": {
        "Gmail": {
          "SmtpServer": "smtp.gmail.com",
          "Port": 587,
          "Username": "seu-email@gmail.com",
          "Password": "sua-senha"
        },
        "SendGrid": {
          "ApiKey": "sua-api-key-do-sendgrid"
        },
        "Sendinblue": {
          "ApiKey": "sua-api-key-do-sendinblue"
        }
      }
    }
    ```

   - Configure os serviços de envio de e-mail em `Startup.cs` ou `Program.cs` conforme necessário.

7. **Crie e aplique as migrações**:

    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

8. **Execute o projeto**:

    ```bash
    dotnet run
    ```

## Funcionalidades Implementadas

- **Autenticação de Usuários**: Registro, login, logout e gerenciamento de contas usando ASP.NET Identity.
- **Recuperação de Senha**: Envio de e-mails para recuperação de senha configurado com Gmail, SendGrid e Sendinblue.
- **Arquitetura MVC**: Estrutura do projeto baseada no padrão Model-View-Controller.

## Contribuições

Este é um projeto de exemplo, então **sugestões e contribuições** são bem-vindas!

---
