using Application.Abstractions.Messaging;
using ArchitectureTests.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetArchTest.Rules;

namespace ArchitectureTests;

public class InfrastructureTests : BaseTests
{
    [Fact]
    public void EntityConfigurations_Should_BeInternal()
    {
        var entityConfigurationTypes = Types
           .InAssembly(InfrastructureAssembly)
           .That()
           .ImplementInterface(typeof(ICommandHandler<,>))
           .Or()
           .ImplementInterface(typeof(IQueryHandler<,>))
           .GetTypes();

        var nonInternalTypes = entityConfigurationTypes.Where(x => x.IsClass && x.IsPublic);

        nonInternalTypes.Should().BeEmpty();
    }

    [Fact]
    public void EntityConfigurations_Should_BeSealed()
    {
        var result = Types
            .InAssembly(InfrastructureAssembly)
            .That()
            .ImplementInterface(typeof(IEntityTypeConfiguration<>))
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
