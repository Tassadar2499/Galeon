using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace GaleonServer.ContractTests._Common;

[CollectionDefinition(nameof(IntegrationTestsCollection))]
public class IntegrationTestsCollection: ICollectionFixture<IntegrationTestWebApplicationFactory>
{
}

public class IntegrationTestWebApplicationFactory : WebApplicationFactory<Program>
{ 
}

public class IntegrationTestWebApplicationFactory<T> : WebApplicationFactory<T> where T : class
{
}