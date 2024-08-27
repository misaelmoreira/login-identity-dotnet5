# Aplica��o de Estudos Web .NET 5 com Identity e EF Core 

Esta aplica��o web � desenvolvida em .NET 5 e utiliza o Entity Framework Core com banco de dados SQLite. A arquitetura da aplica��o segue o padr�o MVC (Model-View-Controller) e implementa autentica��o de usu�rios com o ASP.NET Identity. Al�m disso, a aplica��o est� configurada para enviar e-mails para recupera��o de senha utilizando Gmail, SendGrid e Sendinblue.

## Tecnologias Utilizadas

- **.NET 5**: Framework para desenvolvimento da aplica��o.
- **ASP.NET Identity**: Sistema de autentica��o e gerenciamento de usu�rios.
- **Entity Framework Core**: ORM para intera��o com o banco de dados.
- **SQLite**: Banco de dados leve e f�cil de configurar.
- **Gmail, SendGrid e Sendinblue**: Servi�os de envio de e-mails para recupera��o de senha.

## Estrutura do Projeto

- **Controllers/**: Cont�m os controladores da aplica��o que gerenciam as requisi��es.
- **Models/**: Cont�m as classes de modelo (entidades) utilizadas pelo Entity Framework e Identity.
- **Views/**: Cont�m as p�ginas de visualiza��o (views) da aplica��o.
- **Data/**: Cont�m o contexto do banco de dados e configura��es do Entity Framework.
- **Services/**: Cont�m os servi�os de envio de e-mail e outras funcionalidades adicionais.

## Como Executar

1. **Clone este reposit�rio**:

    ```bash
    git clone https://github.com/seu-usuario/nome-do-repositorio.git
    ```

2. **Navegue at� o diret�rio do projeto**:

    ```bash
    cd nome-do-repositorio
    ```

3. **Restaure as depend�ncias do projeto**:

    ```bash
    dotnet restore
    ```

4. **Restaure as depend�ncias das lib**:

    ```bash
    libman restore
    ```

5. **Configure o banco de dados**:
   - O projeto j� est� configurado para usar o SQLite. A string de conex�o est� definida no arquivo `appsettings.json`:

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Data Source=app.db"
      }
    }
    ```

6. **Configure os servi�os de e-mail**:
   - Abra o arquivo `appsettings.json` e adicione as configura��es para Gmail, SendGrid e Sendinblue:

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

   - Configure os servi�os de envio de e-mail em `Startup.cs` ou `Program.cs` conforme necess�rio.

7. **Crie e aplique as migra��es**:

    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

8. **Execute o projeto**:

    ```bash
    dotnet run
    ```

## Funcionalidades Implementadas

- **Autentica��o de Usu�rios**: Registro, login, logout e gerenciamento de contas usando ASP.NET Identity.
- **Recupera��o de Senha**: Envio de e-mails para recupera��o de senha configurado com Gmail, SendGrid e Sendinblue.
- **Arquitetura MVC**: Estrutura do projeto baseada no padr�o Model-View-Controller.

## Contribui��es

Este � um projeto de exemplo, ent�o **sugest�es e contribui��es** s�o bem-vindas!

---
