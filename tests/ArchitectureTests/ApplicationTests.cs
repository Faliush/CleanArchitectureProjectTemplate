using Application.Abstractions.Messaging;
using ArchitectureTests.Base;
using FluentAssertions;
using NetArchTest.Rules;

namespace ArchitectureTests;

public class ApplicationTests : BaseTests
{
    [Fact]
    public void CommandHandler_Should_HaveNameEndingWith_CommandHandler()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should()
            .HaveNameEndingWith("CommandHandler")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void QueryHandler_Should_HaveNameEndingWith_QueryHandler()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .HaveNameEndingWith("QueryHandler")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Handlers_Should_BeSealed()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Or()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Handlers_Should_BeInternal()
    {
        var handlerTypes = Types
           .InAssembly(ApplicationAssembly)
           .That()
           .ImplementInterface(typeof(ICommandHandler<,>))
           .Or()
           .ImplementInterface(typeof(IQueryHandler<,>))
           .GetTypes();

        var nonInternalTypes = handlerTypes.Where(x => x.IsClass && x.IsPublic);

        nonInternalTypes.Should().BeEmpty();
    }
}
