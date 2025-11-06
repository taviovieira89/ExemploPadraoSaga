# Arquitetura do Sistema

Este documento descreve a arquitetura de alto nível da aplicação, que é composta por dois microserviços principais: `PropostaService` e `ContratacaoService`.

## Diagrama de Componentes

```mermaid
graph TD
    A[Cliente/Frontend] -->|Requisições HTTP| B(PropostaService API)
    A -->|Requisições HTTP| C(ContratacaoService API)

    B -->|Gerencia Propostas| D[PropostaService Database (SQL Server)]
    C -->|Gerencia Contratações| E[ContratacaoService Database (SQL Server)]

    C -->|Aprova Proposta via HTTP| B
```

## Descrição dos Componentes

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

## Fluxo de Interação Exemplo: Contratação de uma Proposta

1.  O **Cliente/Frontend** envia uma requisição para o `ContratacaoService API` para contratar uma proposta, fornecendo o ID da proposta.
2.  O `ContratacaoService API` recebe a requisição.
3.  Para processar a contratação, o `ContratacaoService API` faz uma chamada HTTP para o `PropostaService API` para aprovar a proposta correspondente.
4.  O `PropostaService API` atualiza o status da proposta para "Aprovada" no `PropostaService Database` e retorna o status atualizado para o `ContratacaoService API`.
5.  Se a proposta for aprovada, o `ContratacaoService API` cria um novo registro de contratação no `ContratacaoService Database`.
6.  O `ContratacaoService API` retorna a confirmação da contratação para o **Cliente/Frontend**.