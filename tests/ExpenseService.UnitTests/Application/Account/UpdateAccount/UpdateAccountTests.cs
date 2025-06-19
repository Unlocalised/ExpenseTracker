using ExpenseService.Application.Account.UpdateAccount;
using ExpenseTracker.Application.Common.Exceptions;
using ExpenseService.Application.Common;
using ExpenseTracker.Contracts.Account;
using ExpenseService.Domain.Account;
using Moq;

namespace ExpenseService.UnitTests.Application.Account.UpdateAccount;

public class UpdateAccountTests
{
    [Fact]
    public async Task UpdateAccount_ShouldUpdateAccountAndPublishEventAndCommit()
    {
        var accountId = Guid.NewGuid();
        var accountAggregate = new AccountAggregate(accountId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        var unitOfWork = new Mock<IUnitOfWork>();
        var accountRepository = new Mock<IAccountRepository>();
        accountRepository.Setup(accountRepository => accountRepository.LoadAsync(accountId, CancellationToken.None)).ReturnsAsync(accountAggregate);
        var command = new UpdateAccountCommand(accountId, accountAggregate.Version, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false);
        unitOfWork.Setup(unitOfWork => unitOfWork.Accounts).Returns(accountRepository.Object);
        
        var result = await UpdateAccountCommandHandler.Handle(command, unitOfWork.Object, CancellationToken.None);

        Assert.Equal(accountId, result.AccountId);
        Assert.Equal(3, result.NewVersion);
        unitOfWork.Verify(unitOfWork => unitOfWork.Accounts.LoadAsync(accountId, CancellationToken.None), Times.Once);
        unitOfWork.Verify(unitOfWork => unitOfWork.Accounts.SaveAsync(accountAggregate, command.ExpectedVersion, CancellationToken.None), Times.Once);
        unitOfWork.Verify(unitOfWork => unitOfWork.PublishAsync(It.IsAny<AccountUpdatedIntegrationEvent>()), Times.Once);
        unitOfWork.Verify(unitOfWork => unitOfWork.CommitAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task UpdateAccount_ShouldThrowExceptionWhenAccountNotFound()
    {
        var accountId = Guid.NewGuid();
        var unitOfWork = new Mock<IUnitOfWork>();
        var accountRepository = new Mock<IAccountRepository>();
        accountRepository.Setup(accountRepository => accountRepository.LoadAsync(accountId, CancellationToken.None)).ReturnsAsync((AccountAggregate?)null);
        var command = new UpdateAccountCommand(accountId, 2, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false);
        unitOfWork.Setup(unitOfWork => unitOfWork.Accounts).Returns(accountRepository.Object);
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await UpdateAccountCommandHandler.Handle(command, unitOfWork.Object, CancellationToken.None));

        unitOfWork.Verify(unitOfWork => unitOfWork.Accounts.LoadAsync(accountId, CancellationToken.None), Times.Once);
        unitOfWork.Verify(unitOfWork => unitOfWork.Accounts.SaveAsync(It.IsAny<AccountAggregate>(), command.ExpectedVersion, CancellationToken.None), Times.Never);
        unitOfWork.Verify(unitOfWork => unitOfWork.PublishAsync(It.IsAny<AccountUpdatedIntegrationEvent>()), Times.Never);
        unitOfWork.Verify(unitOfWork => unitOfWork.CommitAsync(CancellationToken.None), Times.Never);
    }
}