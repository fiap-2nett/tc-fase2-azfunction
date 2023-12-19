# TechChallenge - OrderService Fn

...

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

...

## Modelagem de dados

...

## Como executar

A OrderService Fn utiliza como banco de dados o SQL Server 2019 ou superior, toda a infraestrtura necessária para execução do projeto
pode ser provisionada automaticamente através do Docker.

No diretório raíz do projeto, existem os arquivos docker-compose.yml que contém toda a configuração necessária para provisionamento
dos serviços de infraestrutura, caso opte por executar o SQL Server através de container execute o seguinte comando na raíz do projeto:

```sh
$ docker compose up -d techchallenge.db
```

O comando acima irá fazer o download da imagem do SQL Server 2019 e criará automaticamente um container local com o serviço em execução.
Este comando irá configurar o container de SQL Server, todo o processo de criação do banco de dados e carregamento de tabelas padrões será
realizado pelo Entity Framework no momento da execução do projeto.

Caso não queira utilizar o SQL Server através de container Docker, lembre-se de alterar a ConnectionString no arquivo local.settings.json existente
no projeto TechChallenge.FunctionApp.
