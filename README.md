# CQRS
CQRS library which includes behaviors, validators and authorization boilerplates.
This library is based on MediatR.

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


## Query and Command Validation
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

