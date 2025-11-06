# Exemplo Padrão Saga - Microserviços de Proposta e Contratação

Este projeto demonstra uma arquitetura de microserviços utilizando .NET Core, com dois serviços principais: `PropostaService` e `ContratacaoService`. Ele exemplifica a comunicação entre serviços e a gestão de dados distribuídos.

## Arquitetura do Sistema

A aplicação é composta por dois microserviços principais: `PropostaService` e `ContratacaoService`.

```mermaid
graph TD
    A[Cliente/Frontend] -->|Requisições HTTP| B(PropostaService API)
    A -->|Requisições HTTP| C(ContratacaoService API)

    B -->|Gerencia Propostas| D[PropostaService Database (SQL Server)]
    C -->|Gerencia Contratações| E[ContratacaoService Database (SQL Server)]

    C -->|Aprova Proposta via HTTP| B
```

### Descrição dos Componentes

*   **Cliente/Frontend**: Representa qualquer aplicação cliente (web, mobile, etc.) que interage com os microserviços através de suas APIs REST.

*   **PropostaService API**:
    *   Microserviço responsável por gerenciar propostas.
    *   Expõe endpoints para criar, consultar, aprovar e recusar propostas.
    *   Persiste dados no `PropostaService Database`.

*   **ContratacaoService API**:
    *   Microserviço responsável por gerenciar contratações.
    *   Expõe endpoints para contratar propostas.
    *   Comunica-se com o `PropostaService` para aprovar uma proposta antes de criar uma contratação.
    *   Persiste dados no `ContratacaoService Database`.

*   **PropostaService Database (SQL Server)**:
    *   Banco de dados dedicado ao `PropostaService`.
    *   Armazena informações sobre as propostas.

*   **ContratacaoService Database (SQL Server)**:
    *   Banco de dados dedicado ao `ContratacaoService`.
    *   Armazena informações sobre as contratações.

### Fluxo de Interação Exemplo: Contratação de uma Proposta

1.  O **Cliente/Frontend** envia uma requisição para o `ContratacaoService API` para contratar uma proposta, fornecendo o ID da proposta.
2.  O `ContratacaoService API` recebe a requisição.
3.  Para processar a contratação, o `ContratacaoService API` faz uma chamada HTTP para o `PropostaService API` para aprovar a proposta correspondente.
4.  O `PropostaService API` atualiza o status da proposta para "Aprovada" no `PropostaService Database` e retorna o status atualizado para o `ContratacaoService API`.
5.  Se a proposta for aprovada, o `ContratacaoService API` cria um novo registro de contratação no `ContratacaoService Database`.
6.  O `ContratacaoService API` retorna a confirmação da contratação para o **Cliente/Frontend**.

## Configuração do Banco de Dados

Ambos os microserviços utilizam SQL Server. As strings de conexão estão configuradas nos arquivos `appsettings.json` de cada projeto API.

**PropostaService:**
`PropostaService/PropostaService.Api/appsettings.json`
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PropostaServiceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

**ContratacaoService:**
`ContratacaoService/ContratacaoService.Api/appsettings.json`
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ContratacaoServiceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

Para criar e aplicar as migrações do banco de dados, certifique-se de ter o SQL Server Express LocalDB instalado e as ferramentas do Entity Framework Core (`dotnet ef`) instaladas globalmente.

```bash
dotnet tool install --global dotnet-ef
```

Em seguida, execute os seguintes comandos para cada serviço a partir da raiz do projeto (`c:/Git/projetos/ExemploPadraoSaga`):

**Para PropostaService:**
```bash
dotnet ef migrations add InitialCreate -p PropostaService/PropostaService.Infrastructure/PropostaService.Infrastructure.csproj -s PropostaService/PropostaService.Api/PropostaService.Api.csproj -o Data/Migrations
dotnet ef database update -p PropostaService/PropostaService.Infrastructure/PropostaService.Infrastructure.csproj -s PropostaService/PropostaService.Api/PropostaService.Api.csproj
```

**Para ContratacaoService:**
```bash
dotnet ef migrations add InitialCreate -p ContratacaoService/ContratacaoService.Infrastructure/ContratacaoService.Infrastructure.csproj -s ContratacaoService/ContratacaoService.Api/ContratacaoService.Api.csproj -o Data/Migrations
dotnet ef database update -p ContratacaoService/ContratacaoService.Infrastructure/ContratacaoService.Infrastructure.csproj -s ContratacaoService/ContratacaoService.Api/ContratacaoService.Api.csproj
```

## Como Construir e Executar

### Pré-requisitos

*   .NET SDK 9.0
*   SQL Server Express LocalDB (para desenvolvimento local)
*   Docker (opcional, para execução em contêineres)

### Execução Local

1.  **Restaurar dependências:**
    ```bash
    dotnet restore
    ```

2.  **Construir os projetos:**
    ```bash
    dotnet build
    ```

3.  **Executar os microserviços:**

    Abra dois terminais separados na raiz do projeto.

    **Terminal 1 (PropostaService):**
    ```bash
    dotnet run --project PropostaService/PropostaService.Api/PropostaService.Api.csproj
    ```

    **Terminal 2 (ContratacaoService):**
    ```bash
    dotnet run --project ContratacaoService/ContratacaoService.Api/ContratacaoService.Api.csproj
    ```

    Os serviços estarão disponíveis em:
    *   `PropostaService`: `https://localhost:7001` (ou porta configurada)
    *   `ContratacaoService`: `https://localhost:7002` (ou porta configurada)

### Execução com Docker (Bônus)

Dockerfiles foram criados para cada microserviço.

1.  **Construir as imagens Docker:**

    **Para PropostaService:**
    ```bash
    docker build -t propostaservice -f PropostaService/Dockerfile .
    ```

    **Para ContratacaoService:**
    ```bash
    docker build -t contratacaoservice -f ContratacaoService/Dockerfile .
    ```

2.  **Executar os contêineres Docker:**

    Você pode executar os contêineres individualmente ou usar Docker Compose para orquestrá-los (um `docker-compose.yml` não está incluído, mas seria o próximo passo para orquestração).

    **Para PropostaService:**
    ```bash
    docker run -p 7001:80 --name propostaservice_container propostaservice
    ```

    **Para ContratacaoService:**
    ```bash
    docker run -p 7002:80 --name contratacaoservice_container contratacaoservice
    ```

    *Nota: As portas podem precisar ser ajustadas se houver conflitos ou se você estiver usando HTTPS dentro do contêiner.*

## Testes Unitários

Testes unitários foram implementados para as camadas de Domínio e Aplicação de ambos os microserviços.

Para executar os testes, navegue até a raiz do projeto e execute:

```bash
dotnet test
```

Ou execute os testes para cada projeto de teste individualmente:

```bash
dotnet test PropostaService/PropostaService.Domain.Tests/PropostaService.Domain.Tests.csproj
dotnet test PropostaService/PropostaService.Application.Tests/PropostaService.Application.Tests.csproj
dotnet test ContratacaoService/ContratacaoService.Domain.Tests/ContratacaoService.Domain.Tests.csproj
dotnet test ContratacaoService/ContratacaoService.Application.Tests/ContratacaoService.Application.Tests.csproj