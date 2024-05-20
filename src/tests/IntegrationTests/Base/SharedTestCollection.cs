namespace IntegrationTests.Base;

[CollectionDefinition("Test collection")]
public class SharedTestCollection : ICollectionFixture<CustomWebApplicationFactory>
{
}
