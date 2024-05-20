using Domain.Core.Primitives;
using System.Reflection;

namespace ArchitectureTests.Base;

public abstract class BaseTests
{
    protected static readonly Assembly DomainAssembly = typeof(Entity).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(Application.DependencyInjection).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(Infrastructure.DependencyInjection).Assembly;
    protected static readonly Assembly WebApiAssembly = typeof(Program).Assembly;

    protected static readonly string DomainNamespace = "Domain";
    protected static readonly string ApplicationNamespace = "Application";
    protected static readonly string InfrastructureNamespace = "Infracstructure";
    protected static readonly string WebApiNamespace = "Web.Api";
}
