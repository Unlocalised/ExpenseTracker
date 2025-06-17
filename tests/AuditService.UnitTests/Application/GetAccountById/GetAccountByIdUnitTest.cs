using AuditService.Application.Account.GetAccountById;
using AuditService.Domain.Account;
using ExpenseTracker.Application.Common.Exceptions;
using Moq;

namespace AuditService.UnitTests.Application.GetAccountById;

public class GetAccountByIdUnitTest
{
    [Fact]
    public async Task GetAccountById_ShouldReturnAccountWhenExists()
    {
        var accountId = Guid.NewGuid();
        var query = new GetAccountByIdQuery(accountId);
        var expectedAccount = new AccountReadModel { Id = accountId, Name = "Test Account" };
        var accountQueryRepository = new Mock<IAccountQueryRepository>();
        accountQueryRepository.Setup(r => r.GetAccountByIdAsync(accountId, CancellationToken.None))
                      .ReturnsAsync(expectedAccount);

        var result = await GetAccountByIdQueryHandler.Handle(query, accountQueryRepository.Object, CancellationToken.None);

        Assert.Equal(expectedAccount, result);
        accountQueryRepository.Verify(r => r.GetAccountByIdAsync(accountId, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetAccountById_ShouldThrowWhenNotFound()
    {
        var accountId = Guid.NewGuid();
        var query = new GetAccountByIdQuery(accountId);
        var accountQueryRepository = new Mock<IAccountQueryRepository>();
        accountQueryRepository.Setup(accountQueryRepository => accountQueryRepository.GetAccountByIdAsync(accountId, CancellationToken.None))
                      .Throws(new NotFoundException());

        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await GetAccountByIdQueryHandler.Handle(query, accountQueryRepository.Object, CancellationToken.None));

        accountQueryRepository.Verify(accountQueryRepository => accountQueryRepository.GetAccountByIdAsync(accountId, CancellationToken.None), Times.Once);
    }
}