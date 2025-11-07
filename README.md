# MotoAPI

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
- **Controllers**: expõem endpoints RESTful versionados (`/api/v{version}`) com paginação, HATEOAS, validações e integração com ML.NET.
- **Services**: camada de aplicação responsável por regras de negócio, previsões de demanda e acesso ao `DbContext`.
- **Data**: `MotoDbContext` com Entity Framework Core (InMemory para facilitar testes).
- **Common/SwaggerExamples**: helpers para paginação, hiperlinks e exemplos usados no Swagger.
- **Middleware**: proteção de todas as rotas (exceto `/health` e Swagger) com API Key.

Essa organização separa responsabilidades, facilita testes e mantém a API alinhada às boas práticas REST (métodos HTTP, status codes, hiperlinks).

## Requisitos Funcionais Implementados
- CRUD completo para motos, clientes e pedidos.
- Paginação via `page` e `pageSize` com metadados e links de navegação.
- HATEOAS em todos os recursos e coleções.
- Versionamento de API (`/api/v{version}`) com documentação atualizada no Swagger.
- Endpoint de health check em `/health`.
- Segurança via API Key (`X-API-KEY`) documentada e validada pelo middleware dedicado, com configuração centralizada na seção `ApiKey` do `appsettings`.
- Endpoint de previsão de demanda com ML.NET.
- Swagger/OpenAPI com exemplos de requisição/resposta e schemas gerados por XML docs.
- Testes unitários (serviços) e de integração (controllers).

## Executando o Projeto
```bash
# Restaurar pacotes
dotnet restore

# Rodar a API
dotnet run
```
A API utiliza banco InMemory, portanto não requer configuração adicional. Configure a chave de acesso em `appsettings.json` ou `appsettings.Development.json`, na seção `ApiKey:Value` (padrão `local-dev-api-key`), para autenticar requisições.

## Documentação Swagger
Após iniciar a aplicação, acesse:
```
http://localhost:5016/swagger
```

## Exemplos de Uso
Todas as chamadas autenticadas devem enviar o cabeçalho `X-API-KEY` com o valor configurado (por padrão `local-dev-api-key`).

Criar moto:
```bash
curl -X POST https://localhost:5001/api/v1/motos \
  -H "Content-Type: application/json" \
  -H "X-API-KEY: local-dev-api-key" \
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
curl "https://localhost:5001/api/v1/motos?page=1&pageSize=5" \
  -H "X-API-KEY: local-dev-api-key"
```
Criar cliente:
```bash
curl -X POST https://localhost:5001/api/v1/clientes \
  -H "Content-Type: application/json" \
  -H "X-API-KEY: local-dev-api-key" \
  -d '{
        "nome": "Maria Silva",
        "email": "maria.silva@email.com"
      }'
```
Criar pedido:
```bash
curl -X POST https://localhost:5001/api/v1/pedidos \
  -H "Content-Type: application/json" \
  -H "X-API-KEY: local-dev-api-key" \
  -d '{
        "clienteId": 1,
        "motoId": 1,
        "dataRetirada": "2025-05-25T10:00:00Z",
        "dataDevolucao": "2025-05-28T10:00:00Z",
        "valorTotal": 559.70,
        "status": "Reservado"
      }'
```

Previsão de demanda com ML.NET:
```bash
curl -X POST https://localhost:5001/api/v1/analises/previsao-demanda \
  -H "Content-Type: application/json" \
  -H "X-API-KEY: local-dev-api-key" \
  -d '{
        "valorDiaria": 120,
        "mediaDiasUsoPorCliente": 9,
        "quilometragemMediaMensal": 160
      }'
```

Health check (não requer chave):
```bash
curl https://localhost:5001/health
```

## Testes
```bash
# Testes unitários e de integração
dotnet test
```

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
├── MotoAPI.Tests/          # Testes unitários e de integração
└── Program.cs              # Configuração da aplicação
```
