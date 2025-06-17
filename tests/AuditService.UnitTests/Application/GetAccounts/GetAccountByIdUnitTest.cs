using AuditService.Application.Account.GetAccounts;
using AuditService.Domain.Account;
using Moq;

namespace AuditService.UnitTests.Application.GetAccounts;

public class GetAccountsUnitTest
{
    [Fact]
    public async Task GetAccounts_ShouldReturnAccounts()
    {
        var accountId = Guid.NewGuid();
        var query = new GetAccountsQuery();
        List<AccountReadModel> expectedAccounts = [new AccountReadModel { Id = accountId, Name = "Test Account" }];
        var accountQueryRepository = new Mock<IAccountQueryRepository>();
        accountQueryRepository.Setup(r => r.GetAccountsAsync(CancellationToken.None))
                      .ReturnsAsync(expectedAccounts);

        var result = await GetAccountsQueryHandler.Handle(query, accountQueryRepository.Object, CancellationToken.None);

        Assert.Equal(expectedAccounts, result);
        accountQueryRepository.Verify(r => r.GetAccountsAsync(CancellationToken.None), Times.Once);
    }
}