# ProjetoOff API

Esta é uma API robusta, segura e desenvolvida com foco em facilitar a manutenção por ferramentas de Inteligência Artificial. Construída com C# e .NET Core, ela adota as melhores práticas de arquitetura e design de software.

## 🚀 Tecnologias

*   **.NET 10** (Minimal APIs)
*   **C#**
*   **Entity Framework Core** (In-Memory Database para fácil inicialização e testes)
*   **FluentValidation** (Validação de entrada de dados)
*   **JWT (JSON Web Tokens)** (Autenticação e Autorização)
*   **xUnit & FluentAssertions** (Testes de integração)
*   **Swagger/OpenAPI** (Documentação da API)

## 🏗️ Arquitetura

O projeto utiliza a **Vertical Slice Architecture** (Arquitetura de Cortes Verticais).

Em vez de dividir o código por camadas técnicas (Controllers, Services, Repositories), o código é organizado por **Funcionalidades (Features)**. Isso significa que toda a lógica relacionada a um domínio específico (como `Produtos` ou `Clientes`) reside no mesmo diretório.

Isso traz diversos benefícios:
*   **Fácil Manutenção por IA:** A IA (como o Claude ou o Gemini) não precisa navegar por dezenas de arquivos e pastas para entender ou alterar um CRUD. Toda a lógica está agrupada.
*   **Coesão Alta:** O que muda junto, vive junto.
*   **Menos Boilerplate:** Redução drástica de interfaces e injeção de dependências desnecessárias.

### Estrutura de Diretórios
```
ProjetoOff.Api/
├── Features/               # Onde residem os "Cortes Verticais"
│   ├── Clients/            # CRUD de Clientes (Model, DTOs, Validation, Endpoints)
│   └── Products/           # CRUD de Produtos (Model, DTOs, Validation, Endpoints)
├── Infrastructure/         # Configurações globais e infraestrutura técnica
│   ├── Data/               # Entity Framework DbContext
│   └── Security/           # Serviços de geração e validação de JWT
├── Program.cs              # Ponto de entrada, injeção de dependências e configuração do pipeline
```

## 🛠️ Como Executar

### Pré-requisitos
*   [.NET SDK 10.0+](https://dotnet.microsoft.com/download) instalado.

### Passos
1.  Abra o terminal na pasta raiz do projeto (`c:\Projetos\ProjetoOff\api`).
2.  Restaure as dependências e compile o projeto (opcional, o `run` faz isso automaticamente):
    ```bash
    dotnet build
    ```
3.  Execute o projeto:
    ```bash
    dotnet run --project ProjetoOff.Api
    ```
4.  Acesse a documentação do Swagger para testar os endpoints:
    *   `https://localhost:<porta>/swagger` (A porta será informada no terminal durante a execução).

## 🧪 Como Executar os Testes

O projeto contém testes de integração que validam o comportamento dos endpoints em memória.

Para rodar os testes:
1. Abra o terminal na pasta raiz do projeto.
2. Execute o comando:
   ```bash
   dotnet test
   ```

## 🔒 Segurança

A API inclui um endpoint de simulação de login (`POST /api/auth/login`) que gera um token JWT. Em um ambiente real, este endpoint validaria credenciais no banco de dados. Os endpoints de CRUD estão configurados, na estrutura do projeto, para poderem ser facilmente protegidos pelo middleware de autorização do ASP.NET Core (`app.UseAuthentication()` e `app.UseAuthorization()`).

## CORS

A API está configurada para aceitar requisições de origens específicas, atualmente permitindo chamadas do front-end Angular local (`http://localhost:4200`).
