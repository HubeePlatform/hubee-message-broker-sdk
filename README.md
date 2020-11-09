# Hubee Message Broker Sdk

![N|Solid](https://media-exp1.licdn.com/dms/image/C4E0BAQHOp41isf2byw/company-logo_200_200/0?e=1611792000&v=beta&t=R627Tkw1cwQgb-LjNTJh_4auJWQsQieuU4wHoyLfIDA)

Hubee Message Broker Sdk é uma biblioteca que faz abstração da implementação do [MassTransit](https://masstransit-project.com/), que facilita a comunicação assíncrona baseada em uma arquitetura orientada a eventos.  A principal ideia desse SDK é abstrair toda a complexidade das configurações e ser adaptável para as mudanças de tecnologias, centralizando toda manutenção e evolução em um único ponto.

## Getting started

Após realizar a instalação do SDK em seu projeto podemos iniciar a configuração para utilizá-lo, segue abaixo a configuração que deve ser realizada na seção "HubeeMessageBrokerConfig" dentro do arquivo appsettings:

```json
  "HubeeMessageBrokerConfig": {
    "MessageBroker": "RabbitMQ",
    "ApplicationName": "name-service",
    "HostName": "localhost",
    "UserName": "guest",
    "Password": "guest"
  }
```

### Message Broker disponíveis

| Message Broker | Observação |
|:----|:----------|
| InMemory | Facilita os testes para a manipulação dos eventos. [Segue a documentação para um melhor entendimento](https://masstransit-project.com/usage/transports/in-memory.html) |
| RabbitMQ |       |

E depois da configuração acima já podemos utilizar o SDK, segue as opções de utilização.

### Publicar eventos

Segue abaixo os passos para realizar uma publicação de um evento:

**No arquivo "Startup.cs" deve-se adicionar:**

```csharp
using Hubee.MessageBroker.Sdk.Extensions;
//(...)

public class Startup
{
  public Startup(IConfiguration configuration)
  {
    Configuration = configuration;
  }

  public IConfiguration Configuration { get; }
  //(...)

  public void ConfigureServices(IServiceCollection services)
  {
    services.AddEventBus(Configuration);
  }
```

**Publicando um evento:**

Os eventos devem ser criados como **interface** por recomendação da própria documentação do [MassTransit](https://masstransit-project.com/usage/messages.html#message-names).

```csharp
public interface EventTest
{
  Guid EventId {get;}
  string Text { get; }
}
```

E para publicar o evento para os seus consumidores:

```csharp
using Hubee.MessageBroker.Sdk.Interfaces;
//(...)

private readonly IEventBusService _eventBusService;

public Sample(IEventBusService eventBusService)
{
  _eventBusService = eventBusService;
}

_eventBusService.Publish<EventTest>(new
{
  EventId = Guid.NewGuid(),
  Text = "Exemplo",
});
```

**Consumindo um evento:**

Para consumir um evento deve-se utilizar o **GenericMessageHandle\<T>**, onde T será o tópico assinado:

```csharp
using Hubee.MessageBroker.Sdk.Core.Handles;
using Hubee.MessageBroker.Sdk.Core.Models.Headers;
//(...)

public class SampleHandle: GenericMessageHandle<EventTest>
{
  public override Task Handle(EventTest message, EventHeader header)
  {
    //(...)
  }
}
```

Depois de criar o manipulador do evento devemos configurá-lo no "Startup.cs" da aplicação:

```csharp
using Hubee.MessageBroker.Sdk.Extensions;
//(...)

public void ConfigureServices(IServiceCollection services)
{
  services.AddEventBus(Configuration, o =>
  {
    o.AddConsumer<SampleHandle>();
  });
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
  app.UseEventBus(o =>
  {
    o.Subscribe<EventTest, SampleHandle>();
  });
}
```

E para garantir o processamento do evento deve-se utilizar o Retry, por padrão serão realizadas duas tentativas para o processamento do evento e caso não conseguir lidar com o mesmo, será enviada à fila de erro para análise.

**Consumindo falhas do evento:**

Para consumir falhas no processamento do evento deve-se utilizar o **GenericMessageFaultHandle\<T>**, onde T será o tópico assinado:

```csharp
using Hubee.MessageBroker.Sdk.Core.Handles;
using Hubee.MessageBroker.Sdk.Core.Models.Headers;
//(...)

public class SampleFaultHandle: GenericMessageFaultHandle<EventTest>
{
  public override Task Handle(EventTest message, EventHeader header)
  {
    //(...)
  }
}
```

Depois de criar o manipulador de falhas do evento devemos configurá-lo no "Startup.cs" da aplicação:

```csharp
using Hubee.MessageBroker.Sdk.Extensions;
//(...)

public void ConfigureServices(IServiceCollection services)
{
  services.AddEventBus(Configuration, o =>
  {
    o.AddConsumer<SampleFaultHandle>();
  });
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
  app.UseEventBus(o =>
  {
    o.SubscribeFault<EventTest, SampleHandle>();
  });
}
```

**Observação**: caso possuir mais de um consumidor para o mesmo evento ambas as falhas irão ser publicadas para o mesmo manipulador.
