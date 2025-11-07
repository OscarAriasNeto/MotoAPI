# MotoAPI -- Ler até o final para entender o funcionamento

API RESTful para gestão de locação de motocicletas, clientes e pedidos, desenvolvida em ASP.NET Core 8.0.413.
=======

## Integrantes
- Oscar Arias Neto - RM556936
- Julia Martins Rebelles - RM554516
- Nicolas Souza dos Santos - RM555571

## Domínio e Entidades
O domínio representa uma locadora de motocicletas. As três entidades principais refletem responsabilidades do negócio:
- **Moto**: catálogo de motocicletas com preço de diária e estado operacional.
- **Cliente**: usuários cadastrados que realizam locações.
- **Pedido**: pedidos de locação que relacionam cliente e motocicleta, com status e valores.

## Arquitetura
A solução segue uma arquitetura em camadas simples:
- **Controllers**: expõem endpoints RESTful versionados (`/api/v1`) com paginação, HATEOAS e validações.
- **Services**: camada de aplicação responsável por regras de negócio e acesso ao `DbContext`.
- **Data**: `MotoDbContext` com Entity Framework Core (InMemory para facilitar testes).
- **Common/SwaggerExamples**: helpers para paginação, hiperlinks e exemplos usados no Swagger.

Essa organização separa responsabilidades, facilita testes e mantém a API alinhada às boas práticas REST (métodos HTTP, status codes, hiperlinks).

## Requisitos Funcionais Implementados
- CRUD completo para motos, clientes e pedidos.
- Paginação via `page` e `pageSize` com metadados e links de navegação.
- HATEOAS em todos os recursos e coleções.
- Swagger/OpenAPI com exemplos de requisição/resposta e schemas gerados por XML docs.
- Testes unitários (serviços) e de integração (controllers).

## Executando o Projeto
### Pré-requisitos
- [.NET SDK 8.0](https://dotnet.microsoft.com/download) instalado localmente.
- Variável de ambiente `ASPNETCORE_ENVIRONMENT` opcional para alternar entre configurações (`Development` é o padrão recomendado para testes locais).

### Passo a passo
1. **Restaurar dependências**
   ```bash
   dotnet restore
   ```

2. **Configurar a API Key**
   - Por padrão, o middleware de segurança verifica o header `X-API-KEY`.
   - A chave esperada é definida na seção `Security:ApiKey` do `appsettings.json` (ou `appsettings.Development.json`).
   - Para sobrepor via variável de ambiente utilize `Security__ApiKey`. Exemplo (Linux/macOS):
     ```bash
     export Security__ApiKey="chave-super-secreta"
     ```

3. **Executar a aplicação**
   ```bash
   dotnet run
   ```

A API utiliza banco InMemory, portanto não requer configuração adicional de persistência. Insira dados pelos endpoints autenticando-se com o header `X-API-KEY` configurado no passo anterior.

## Documentação Swagger
Após iniciar a aplicação, acesse:
```
http://localhost:5016/swagger
```

## Exemplos de Uso
Criar moto:
```bash
curl -X POST https://localhost:5001/api/v1/motos \
  -H "Content-Type: application/json" \
  -d '{
        "modelo": "Honda CB 500X",
        "anoFabricacao": 2024,
        "placa": "ABC1234",
        "valorDiaria": 189.90,
        "estado": "Pronta"
      }'
```
Listar motos (pagina 1 com 5 itens):
```bash
curl "https://localhost:5001/api/v1/motos?page=1&pageSize=5"
```
Criar cliente:
```bash
curl -X POST https://localhost:5001/api/v1/clientes \
  -H "Content-Type: application/json" \
  -d '{
        "nome": "Maria Silva",
        "email": "maria.silva@email.com"
      }'
```
Criar pedido:
```bash
curl -X POST https://localhost:5001/api/v1/pedidos \
  -H "Content-Type: application/json" \
  -d '{
        "clienteId": 1,
        "motoId": 1,
        "dataRetirada": "2025-05-25T10:00:00Z",
        "dataDevolucao": "2025-05-28T10:00:00Z",
        "valorTotal": 559.70,
        "status": "Reservado"
      }'
```

## Testes
Os testes ficam no projeto `MotoAPI.Tests` e cobrem duas camadas:

- **Unitários**: validam a lógica dos serviços com `DbContext` em memória (ex.: paginação e verificação de placas da `MotoService`).
- **Integração**: exercitam os endpoints reais via `WebApplicationFactory`, já configurando a `X-API-KEY` de teste para passar pelo middleware de segurança.

Para executar toda a suíte utilize:

```bash
dotnet test
```

Se preferir, é possível filtrar apenas uma categoria utilizando traits do xUnit, por exemplo `dotnet test --filter FullyQualifiedName~Integration` para rodar apenas os testes de integração.
O motoAPI.Tests está dentro do MotoAPI pois não encontrei uma maneira melhor para enviar pelo github, mas o código só funciona retirarndo as partes de teste dentro do MotoAPI

## Estrutura do Repositório
```
MotoAPI/
├── Common/                 # Helpers de paginação e HATEOAS
├── Controllers/            # Controllers RESTful versionados
├── Data/                   # DbContext EF Core
├── DTOs/                   # DTOs de entrada/saída
├── Models/                 # Entidades de domínio
├── Services/               # Camada de serviços
├── SwaggerExamples/        # Exemplos utilizados no Swagger
└── Program.cs              # Configuração da aplicação
MotoAPI.Tests/          # Testes unitários e de integração
```
