using AuditService.Application.Account.GetAccountById;
using AuditService.Application.Common;
using AuditService.Domain.Account;
using ExpenseTracker.Application.Common.Exceptions;
using Moq;

namespace AuditService.UnitTests.Application.GetAccountById;

public class GetAccountByIdTests
{
    [Fact]
    public async Task GetAccountById_ShouldReturnAccountWhenExists()
    {
        var accountId = Guid.NewGuid();
        var query = new GetAccountByIdQuery(accountId);
        var expectedAccount = new AccountReadModel { Id = accountId, Name = "Test Account" };
        var cacheService = new Mock<ICacheService>();
        var accountQueryRepository = new Mock<IAccountQueryRepository>();
        cacheService.Setup(cacheService => cacheService.GetAsync<AccountReadModel>(It.IsAny<string>(), CancellationToken.None))
        .ReturnsAsync((AccountReadModel)null!);
        accountQueryRepository.Setup(r => r.GetAccountByIdAsync(accountId, CancellationToken.None))
                      .ReturnsAsync(expectedAccount);

        var result = await GetAccountByIdQueryHandler.Handle(query, accountQueryRepository.Object, cacheService.Object, CancellationToken.None);

        Assert.Equal(expectedAccount, result);
        accountQueryRepository.Verify(r => r.GetAccountByIdAsync(accountId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetAccountById_ShouldThrowWhenNotFound()
    {
        var accountId = Guid.NewGuid();
        var query = new GetAccountByIdQuery(accountId);
        var cacheService = new Mock<ICacheService>();
        var accountQueryRepository = new Mock<IAccountQueryRepository>();
        accountQueryRepository.Setup(accountQueryRepository => accountQueryRepository.GetAccountByIdAsync(accountId, CancellationToken.None))
                      .Throws(new NotFoundException());

        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await GetAccountByIdQueryHandler.Handle(query, accountQueryRepository.Object, cacheService.Object, CancellationToken.None));

        accountQueryRepository.Verify(accountQueryRepository => accountQueryRepository.GetAccountByIdAsync(accountId, CancellationToken.None), Times.Once);
    }
}