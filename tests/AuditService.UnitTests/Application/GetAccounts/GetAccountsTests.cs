using System.Collections.Generic;
using AuditService.Application.Account.GetAccounts;
using AuditService.Application.Common;
using AuditService.Domain.Account;
using Moq;

namespace AuditService.UnitTests.Application.GetAccounts;

public class GetAccountsTests
{
    [Fact]
    public async Task GetAccounts_ShouldReturnAccounts()
    {
        var accountId = Guid.NewGuid();
        var query = new GetAccountsQuery();
        List<AccountReadModel> expectedAccounts = [new AccountReadModel { Id = accountId, Name = "Test Account" }];
        var cacheService = new Mock<ICacheService>();
        var accountQueryRepository = new Mock<IAccountQueryRepository>();
        cacheService.Setup(cacheService => cacheService.GetAsync<IReadOnlyList<AccountReadModel>>(It.IsAny<string>(), CancellationToken.None))
              .ReturnsAsync((IReadOnlyList<AccountReadModel>)null!);
        accountQueryRepository.Setup(accountQueryRepository => accountQueryRepository.GetAccountsAsync(CancellationToken.None))
                      .ReturnsAsync(expectedAccounts);

        var result = await GetAccountsQueryHandler.Handle(query, accountQueryRepository.Object, cacheService.Object, CancellationToken.None);

        Assert.Equal(expectedAccounts, result);
        accountQueryRepository.Verify(r => r.GetAccountsAsync(CancellationToken.None), Times.Once);
    }
}