
<h1>
	<img alt="OnlyMessage" src=".github/Banner.svg" width="100%" />
</h1>

## Introdução
Essa aplicação tem como proposito mostrar parte do conhecimento que obtive nesses anos de programação e servir de portifólio para que as empresas possam me avaliar. A aplicação será uma *API* de um *virtual chat* com a possibilidade de adicionar amigos e conversar com pessoas. Também será possivel se registrar, logar e criar grupos.

## Techs
- Dotnet 6
- Swagger
- SignalR
- FastEndpoint
- MediatR
- AutoMapper
- Entity Framework
- Postgres/SqLite

### Arquitetura DDD (Domain Driven Design)
DDD Não é nada alem de uma arquitetura que aborda uma modelagem de software com um conjunto de boas práticas com objetivo de facilitar a implementação regras e processos de negócios que são tratados como dominio.

### Download para versão de desenvolvimento
será necessario ter o docker e o git instalados na maquina.
```bash
	git clone https://github.com/Adeilton-Ro/OnlyMessage.git
	cd OnlyMessage
	dotnet restore
	docker build -t backend -f Dockerfile .
	docker run --name onlymessage backend
```

quer saber mais sobre a aplicação? <a href=".github/Regras.md">Clique aqui!</a>