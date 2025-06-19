using AuditService.IntegrationTests.Setup;

namespace AuditService.IntegrationTests.Handlers;

public class CreateAccountTests : IntegrationTest, IClassFixture<CustomWebApplicationFactory<Program>>
{
    public CreateAccountTests(CustomWebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public void Test1()
    {
    }
}