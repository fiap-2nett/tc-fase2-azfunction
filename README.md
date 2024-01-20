# TechChallenge - Order AzFunction

O Order AzFunction é um Azure Durable Function que implementa o fluxo de criação e aprovação de pedidos.
Este modelo possibilita que sejam emitidas requisições de criação de um pedido, este por sua vez irá passar
por todas as etapas de validação de negócio.

Caso o pedido possua todos os dados válidos, o mesmo será criado com status de Novo e após a reserva dos itens
dos produtos no estoque, o mesmo passará para o status de Processamento. A partir deste momento, iniciará o fluxo
de aprovação, por padrão a função irá aguardar por até 3 minutos a requisição de aprovação do pedido, caso esse tempo
seja atingido ou seje recebido o evento de Rejeição o pedido será rejeito e as devidas quantidades do produtos serão
estornadas ao estoque.

Todo o processo descrito acima de aprovação, rejeição, consumo e retorno de produtos ao estoque ocorre através de disparo
de eventos de domínio, seguindo o padrão arquitetural CQRS & Event-Driven.

## Colaboradores

- [Ailton Alves de Araujo](https://www.linkedin.com/in/ailton-araujo-b4ba0520/) - RM350781
- [Bruno Fecchio Salgado](https://www.linkedin.com/in/bfecchio/) - RM350780
- [Cecília Gonçalves Wlinger](https://www.linkedin.com/in/cec%C3%ADlia-wlinger-6a5459100/) - RM351312
- [Cesar Julio Spaziante](https://www.linkedin.com/in/cesar-spaziante/) - RM351311
- [Paulo Felipe do Nascimento de Sousa](https://www.linkedin.com/in/paulo-felipe06/) - RM351707

## Tecnologias utilizadas

- .NET 6.0
- MediatR 12.0
- FluentValidation 11.8
- Entity Framework Core 3.2
- SqlServer 2019
- Docker 24.0.5
- Docker Compose 2.20

## Arquitetura, Padrões Arquiteturais e Convenções

- REST Api
- Domain-Driven Design
- EF Code-first
- CQRS
- Event-Driven
- Service Pattern
- Repository Pattern & Unit Of Work

## Definições técnicas

| Projeto                               | Descrição                                                                                    |
|---------------------------------------|----------------------------------------------------------------------------------------------|
| _TechChallenge.FunctionApp_           | Contém a implementação da Azure Durable Function.                                            |
| _TechChallenge.Application_           | Contém a implementação dos componentes de negócios (commands, handlers, validators e events. |
| _TechChallenge.Domain_                | Contém a implementação das entidades e interfaces do domínio da aplicação.                   |
| _TechChallenge.Persistence_           | Contém a implementação dos componentes relacionados a consulta e persistência de dados.      |

## Modelagem de dados

O Order AzFunction utiliza o paradigma de CodeFirst através dos recursos disponibilizados pelo Entity Framework, no entanto para melhor
entendimento da modelagem de dados apresentamos a seguir o MER e suas respectivas definições:

![orderitems](https://github.com/fiap-2nett/tc-fase2-azfunction/assets/57924071/13d2f0e7-d227-45a2-abcf-d25ec98c72fe)

Com base na imagem acima iremos detalhar as tabelas e os dados contidos em cada uma delas:

| Schema | Tabela       | Descrição                                                                |
|--------|--------------|--------------------------------------------------------------------------|
| dbo    | products     | Tabela que contém os dados referentes aos produtos da plataforma.        |
| dbo    | orders       | Tabela que contém os dados referentes aos pedidos da plataforma.         |
| dbo    | orderitems   | Tabela que contém os dados referentes aos itens de pedido da plataforma. |

## Como executar

### Inicializando o Banco de Dados

A Order AzFunction utiliza como banco de dados o SQL Server 2019 ou superior, toda a infraestrutura necessária para execução do projeto
pode ser provisionada automaticamente através do Docker.

No diretório raíz do projeto, existem os arquivos docker-compose.yml que contém toda a configuração necessária para provisionamento
dos serviços de infraestrutura, caso opte por executar o SQL Server através de container execute o seguinte comando na raíz do projeto:

```sh
$ docker compose up -d techchallenge.db
```
O comando acima irá fazer o download da imagem do SQL Server 2019 e criará automaticamente um container local com o serviço em execução.
Este comando irá configurar o container de SQL Server, todo o processo de criação do banco de dados e carregamento de tabelas padrões será
realizado pelo Entity Framework no momento da execução do projeto.

Caso não queira utilizar o SQL Server através de container Docker, lembre-se de alterar a ConnectionString no arquivo local.settings.json
existente no projeto TechChallenge.FunctionApp.

### Carga inicial de dados

Após a inicialização do banco de dados e execução do Order AzFunction, será inserido automaticamente no banco de dados alguns produtos para
facilitar a utilização e testes da plataforma, abaixo segue os dados inseridos automaticamente:

| ProductId |Name                                           | Price  | Quantity |
|-----------|-----------------------------------------------|--------|----------|
| 1000      | Camiseta Dragon’s Treasure – Black Edition    | 54.90  | 3        |
| 1001      | Camiseta Angra – Cycles Of Pain               | 54.90  | 3        |
| 1002      | Camiseta Raccoon City                         | 54.90  | 3        |
| 1003      | Camiseta Voyager Black Edition                | 54.90  | 3        |
| 1004      | Camiseta Necronomicon Black Edition           | 54.90  | 3        |
| 1005      | Camiseta &#193;rvore de Gondor – Gold Edition | 54.90  | 3        |
| 1006      | Camiseta Lovecraft                            | 54.90  | 3        |
| 1007      | Camiseta Dark Side                            | 54.90  | 3        |
| 1008      | Camiseta de R’lyeh                            | 54.90  | 3        |
| 1009      | Camiseta Upside Down                          | 54.90  | 3        |
| 1010      | Camiseta Miskatonic University                | 54.90  | 3        |

### Executando o projeto

Ao executar o projeto será aberto uma instância do terminal identificando os endpoints disponíveis para utilização, são eles:

![image](https://github.com/fiap-2nett/tc-fase2-azfunction/assets/57924071/0441b7ab-9257-4b31-b9f7-2f63267ff193)

### Criando um pedido

Para realizar a criação de um pedido, basta realizar uma requisição HTTP do tipo POST para o endpoint
`Order: [POST] http://localhost:7066/api/Order`, veja um exemplo:

```curl
curl --location 'http://localhost:7066/api/Order' \
--header 'Content-Type: application/json' \
--data-raw '{ 
    "Items": [ 
        { 
            "ProductId": 1001, 
            "Quantity": 2 
        }, 
        { 
            "ProductId": 1002,
            "Quantity": 1 
        } 
    ],
    "CustomerEmail": "cliente@techchallenge.app"
}'
```

### Aprovar um pedido

Para aprovar um pedido, utilize o endpoint sendEventPostUri listado no corpo de resposta de criação do pedido substituindo o
parâmetro da URL `{eventName}` pelo valor `AcceptOrder` e no corpo da requisição informe o valor `true`, veja um exemplo abaixo:

```curl
curl --location 'http://localhost:7066/runtime/webhooks/durabletask/instances/2b0122651b0b4372a1034a18e1ad9510/raiseEvent/AcceptOrder?taskHub=TestHubName&connection=Storage&code=3sw1zqCdUnPK-i8NFNAIWQ6BfRpIqa43CnrANVKXfQmOAzFuuff12A%3D%3D' \
--header 'Content-Type: application/json' \
--data 'true'
```

### Reprovar um pedido

Para reprovar um pedido, utilize o endpoint `sendEventPostUri` listado no corpo de resposta de criação do pedido substituindo
o parâmetro da URL `{eventName}` pelo valor `AcceptOrder` e no corpo da requisição informe o valor `false`, veja um exemplo abaixo:

```curl
curl --location 'http://localhost:7066/runtime/webhooks/durabletask/instances/2b0122651b0b4372a1034a18e1ad9510/raiseEvent/AcceptOrder?taskHub=TestHubName&connection=Storage&code=3sw1zqCdUnPK-i8NFNAIWQ6BfRpIqa43CnrANVKXfQmOAzFuuff12A%3D%3D' \
--header 'Content-Type: application/json' \
--data 'false'
```

### Cancelar um pedido

Para cancelar um pedido, utilize o endpoint `terminatePostUri` listado no corpo de resposta de criação do pedido substituindo
o parâmetro da URL `{text}` pelo motivo que o pedido será cancelado, vide exemplo abaixo:

```curl
curl --location --request POST 'http://localhost:7066/runtime/webhooks/durabletask/instances/e36ba21c16d2450aa8338991fe8e4c28/terminate?reason=Usuario%20pediu%20cancelamento&taskHub=TestHubName&connection=Storage&code=eHoKkaae5Ozug1tNo8axoLeyIR7mYBarW_ipCRBXn8t4AzFuEkfavw%3D%3D'
```

### Deixar um Pedido Suspenso

Para deixar um pedido suspenso, utilize o endpoint `suspendPostUri` listado no corpo de resposta de criação do pedido substituindo
o parâmetro da URL `{text}` pelo motivo que o pedido ficará suspenso, vide exemplo abaixo:

```curl
curl --location --request POST 'http://localhost:7066/runtime/webhooks/durabletask/instances/e7bc9a38c32f4600ac775b0dc65576fd/suspend?reason=Usuario%20pediu%20que%20a%20compra%20fosse%20debitada%20somente%20apos%20o%20dia%2015&taskHub=TestHubName&connection=Storage&code=eHoKkaae5Ozug1tNo8axoLeyIR7mYBarW_ipCRBXn8t4AzFuEkfavw%3D%3D'
``` 

### Retornar um Pedido Suspenso para Ativo

Para retornar um pedido  que estava suspenso para ativo, utilize o endpoint `resumePostUri` listado no corpo de resposta de criação do pedido substituindo
o parâmetro da URL `{text}` pelo motivo que o pedido retornará para ativo, vide exemplo abaixo:

```curl
curl --location --request POST 'http://localhost:7066/runtime/webhooks/durabletask/instances/e7bc9a38c32f4600ac775b0dc65576fd/resume?reason=O%20usuario%20mudou%20de%20ideia%20e%20pediu%20pra%20debitar%20o%20mais%20rapido%20possivel&taskHub=TestHubName&connection=Storage&code=eHoKkaae5Ozug1tNo8axoLeyIR7mYBarW_ipCRBXn8t4AzFuEkfavw%3D%3D'
```

### Obter detalhes do pedido

Para obter detalhes de um pedido, utilize o endpoint `statusQueryGetUri` listado no corpo de resposta de criação do pedido, veja
um exemplo abaixo:

```curl
curl --location 'http://localhost:7066/runtime/webhooks/durabletask/instances/12669ec261f64ec2816edd7cda345896?taskHub=TestHubName&connection=Storage&code=3sw1zqCdUnPK-i8NFNAIWQ6BfRpIqa43CnrANVKXfQmOAzFuuff12A%3D%3D'
```

### Deletar Pedido

Para deletar um pedido, utilize o endpoint `purgeHistoryDeleteUri` listado no corpo de resposta de criação do pedido, veja
um exemplo abaixo:

```curl
curl --location --request DELETE 'http://localhost:7066/runtime/webhooks/durabletask/instances/e7bc9a38c32f4600ac775b0dc65576fd?taskHub=TestHubName&connection=Storage&code=eHoKkaae5Ozug1tNo8axoLeyIR7mYBarW_ipCRBXn8t4AzFuEkfavw%3D%3D'
```


