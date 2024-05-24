using ArchitectureTests.Base;
using Carter;
using FluentAssertions;
using NetArchTest.Rules;

namespace ArchitectureTests;

public class WebApiTests : BaseTests
{
    [Fact]
    public void Endpoints_Should_HaveEndingWith_Endpoint()
    {
        var result = Types
            .InAssembly(WebApiAssembly)
            .That()
            .ImplementInterface(typeof(ICarterModule))
            .Should()
            .HaveNameEndingWith("Endpoints")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
