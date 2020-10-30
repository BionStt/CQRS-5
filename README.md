# CQRS
CQRS library based on [MediatR](https://github.com/jbogard/MediatR) which includes behaviors, validators and authorization boilerplates.

This library will give you the following functionality out of the box:
- Unhandled exception logging.
- Request performance logging.
- Request validation behaviour.
- Request authorisation behaviour.

## Dependency Injection
You just need to add the following lines of code to your `Startup.cs` class to use this library.
``` csharp
public void ConfigureServices(IServiceCollection services)
{
    // Add the assembly where all your Query and Command handlers are located.
    services.AddCQRS(typeof(Startup).Assembly);
}
```

## Examples
### Query & Query Handlers
``` csharp
public class TestQuery : IQuery
{
    public int Id { get; set; }
}

public class TestQueryHandler : IQueryHandler<TestQuery>
{
    // Will be injected using dependency injection
    private readonly IDependency dependency;
    public TestQueryHandler(IDependency dependency)
    {
        this.dependency = dependency;
    }

    public async Task<CQRSResponse> Handle(TestQuery query, CancellationToken cancellationToken)
    {
        // Perform your query logic here and return data
        var data = 1;
        return CQRSResponse.Success(data);
    }
}

```


### Command & Command Handlers
``` csharp
public class TestCommand : ICommand
{
    public int Id { get; set; }
}

public class TestCommandHandler : ICommandHandler<TestCommand>
{
    // Will be injected using dependency injection
    private readonly IDependency dependency;
    public TestCommandHandler(IDependency dependency)
    {
        this.dependency = dependency;
    }

    public async Task<CQRSResponse> Handle(TestCommand command, CancellationToken cancellationToken)
    {
        // Perform your command logic here
        // Can return data if need to but not recommended.
        var data = 1;
        return CQRSResponse.Success;
    }
}

```


### Query and Command Validation
``` csharp
public class TestQueryValidator : IValidator<TestQuery>
{
    // Will be injected using dependency injection
    private readonly IDependency dependency;
    public TestQueryValidator(IDependency dependency)
    {
        this.dependency = dependency;
    }

    public async Task<CQRSResponse> Validate(TestQuery query)
    {
        // Perform query validation here
        // return CQRSResponse.Success() if successful
        // return CQRSResponse.Bad() or Unauthorised etc if unsuccessful
        return CQRSResponse.Success();
    }
}
```

### Query and Command Authorisation
#### IAuthorisationRequirement Interface
The interface which needs to be implemented to define an authorisation requirement. A requirement is something which must be meet for the command or query to be authorised.
``` csharp
public class IsAdminRequirement : IAuthorisationRequirement 
{
    // Properties can be added in here.
    // When you add properties here you can use the data inside the authorisation handler logic.
}
```

#### IAuthorisable Interface
The interface the `Command` or `Query` needs to implement. Will define the list of requirements which need to be meet in order for the query or command to execute.
``` csharp
public class GetAllUsersQuery : IQuery, IAuthorisable 
{
    // Create a list of requirements the query must meet
    // If the require add the query data to the authorisation requirement.
    public IEnumerable<IAuthorisationRequirement> Requirements => new List<IAuthorisationRequirement> { new IsAdminRequirement() }
}
```


#### IAuthorisationRequirementHandler Interface
The interface which needs to be implemented to define the authorisation logic for a requirement.
``` csharp
public class IsAdminRequirementHandler : IAuthorisationRequirementHandler<IsAdminRequirement>
{
    // Will be injected using dependency injection
    private readonly IDependency dependency;
    public IsAdminRequirementHandler(IDependency dependency)
    {
        this.dependency = dependency;
    }

    public async Task<CQRSResponse> Handle(IsAdminRequirement requirement, CancellationToken cancellationToken)
    {
        // Perform your authorisation logic here
        return CQRSResponse.Success;
    }
}
```

