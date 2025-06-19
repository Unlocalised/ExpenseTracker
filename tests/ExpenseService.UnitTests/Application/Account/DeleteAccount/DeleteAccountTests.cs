using ExpenseService.Application.Account.DeleteAccount;
using ExpenseTracker.Application.Common.Exceptions;
using ExpenseService.Application.Common;
using ExpenseTracker.Contracts.Account;
using ExpenseService.Domain.Account;
using Moq;

namespace ExpenseService.UnitTests.Application.Account.DeleteAccount;

public class DeleteAccountTests
{
    [Fact]
    public async Task DeleteAccount_ShouldLoadAndDeleteAccountAndAppendEventAndPublishEventAndCommit()
    {
        var accountId = Guid.NewGuid();
        var accountAggregate = new AccountAggregate();
        var unitOfWork = new Mock<IUnitOfWork>();
        var accountRepository = new Mock<IAccountRepository>();
        accountRepository.Setup(accountRepository => accountRepository.LoadAsync(accountId, CancellationToken.None)).ReturnsAsync(accountAggregate);
        var command = new DeleteAccountCommand(accountId, 2);
        unitOfWork.Setup(unitOfWork => unitOfWork.Accounts).Returns(accountRepository.Object);
        await DeleteAccountCommandHandler.Handle(command, unitOfWork.Object, CancellationToken.None);

        unitOfWork.Verify(unitOfWork => unitOfWork.Accounts.LoadAsync(accountId, CancellationToken.None), Times.Once);
        unitOfWork.Verify(unitOfWork => unitOfWork.Accounts.SaveAsync(accountAggregate, command.ExpectedVersion, CancellationToken.None), Times.Once);
        unitOfWork.Verify(unitOfWork => unitOfWork.PublishAsync(It.IsAny<AccountDeletedIntegrationEvent>()), Times.Once);
        unitOfWork.Verify(unitOfWork => unitOfWork.CommitAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task DeleteAccount_ShouldThrowExceptionWhenAccountNotFound()
    {
        var accountId = Guid.NewGuid();
        var unitOfWork = new Mock<IUnitOfWork>();
        var accountRepository = new Mock<IAccountRepository>();
        accountRepository.Setup(accountRepository => accountRepository.LoadAsync(accountId, CancellationToken.None)).ReturnsAsync((AccountAggregate?)null);
        var command = new DeleteAccountCommand(accountId, 2);
        unitOfWork.Setup(unitOfWork => unitOfWork.Accounts).Returns(accountRepository.Object);

        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await DeleteAccountCommandHandler.Handle(command, unitOfWork.Object, CancellationToken.None));

        unitOfWork.Verify(unitOfWork => unitOfWork.Accounts.LoadAsync(accountId, CancellationToken.None), Times.Once);
        unitOfWork.Verify(unitOfWork => unitOfWork.Accounts.SaveAsync(It.IsAny<AccountAggregate>(), command.ExpectedVersion, CancellationToken.None), Times.Never);
        unitOfWork.Verify(unitOfWork => unitOfWork.PublishAsync(It.IsAny<AccountDeletedIntegrationEvent>()), Times.Never);
        unitOfWork.Verify(unitOfWork => unitOfWork.CommitAsync(CancellationToken.None), Times.Never);
    }
}