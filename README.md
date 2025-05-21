# Moto Management API 🏍️

API para gestão de motocicletas com controle de estados operacionais e emissão de cores dinâmicas.

## Descrição do Projeto
Sistema completo para gerenciamento de motos com:
- Cadastro com ID personalizado
- Controle de 4 estados operacionais
- Emissão de cor automática por estado
- Banco de dados Oracle integrado
- Documentação Swagger interativa
- Validações customizadas

**Estados/Cores:**
| Estado     | Cor       |
|------------|-----------|
| Pronta     | Verde     |
| Lavagem    | Azul      |
| Sinistro   | Vermelho  |
| Manutencao | Amarelo   |

## Rotas da API 🔄

### Métodos Principais
| Método | Rota                   | Descrição                          |
|--------|------------------------|------------------------------------|
| GET    | /api/motos             | Lista todas as motos com filtros   |
| GET    | /api/motos/{id}        | Busca moto por ID                  |
| GET    | /api/motos/placa/{placa}| Busca moto por placa              |
| POST   | /api/motos             | Cadastra nova moto                 |
| PUT    | /api/motos/{id}        | Atualiza moto por ID               |
| PUT    | /api/motos/placa/{placa}| Atualiza moto por placa           |
| DELETE | /api/motos/{id}        | Remove moto por ID                 |
| DELETE | /api/motos/placa/{placa}| Remove moto por placa             |

### Exemplo de Request/Response
**POST /api/motos**
```json
{
  "id": 1,
  "modelo": "Honda CB 500",
  "anoFabricacao": 2023,
  "placa": "ABC1234",
  "estado": "Pronta"
}
