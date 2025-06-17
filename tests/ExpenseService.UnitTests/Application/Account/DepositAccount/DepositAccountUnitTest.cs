using ExpenseService.Application.Account.DepositAccount;
using ExpenseTracker.Application.Common.Exceptions;
using ExpenseService.Application.Common;
using ExpenseService.Domain.Transaction;
using ExpenseTracker.Contracts.Account;
using ExpenseService.Domain.Account;
using Moq;

namespace ExpenseService.UnitTests.Application.Account.DepositAccount;

public class DepositAccountUnitTest
{
    [Fact]
    public async Task DepositAccount_ShouldLoadAndDepositAccountAndAppendEventAndPublishEventAndCommit()
    {
        var accountId = Guid.NewGuid();
        var accountAggregate = new AccountAggregate(accountId, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        var unitOfWork = new Mock<IUnitOfWork>();
        var accountRepository = new Mock<IAccountRepository>();
        accountRepository.Setup(accountRepository => accountRepository.LoadAsync(accountId, CancellationToken.None)).ReturnsAsync(accountAggregate);
        var transactionRepository = new Mock<ITransactionRepository>();
        var command = new DepositAccountCommand(accountId, accountAggregate.Version, 100);
        unitOfWork.Setup(unitOfWork => unitOfWork.Accounts).Returns(accountRepository.Object);
        unitOfWork.Setup(unitOfWork => unitOfWork.Transactions).Returns(transactionRepository.Object);

        var result = await DepositAccountCommandHandler.Handle(command, unitOfWork.Object, CancellationToken.None);

        Assert.Equal(accountId, result.AccountId);
        Assert.Equal(3, result.NewVersion);
        unitOfWork.Verify(unitOfWork => unitOfWork.Accounts.LoadAsync(accountId, CancellationToken.None), Times.Once);
        unitOfWork.Verify(unitOfWork => unitOfWork.Accounts.SaveAsync(accountAggregate, command.ExpectedVersion, CancellationToken.None), Times.Once);
        unitOfWork.Verify(unitOfWork => unitOfWork.Transactions.Create(It.IsAny<TransactionAggregate>()), Times.Once);
        unitOfWork.Verify(unitOfWork => unitOfWork.PublishAsync(It.IsAny<AccountBalanceUpdatedIntegrationEvent>()), Times.Once);
        unitOfWork.Verify(unitOfWork => unitOfWork.CommitAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task DepositAccount_ShouldThrowExceptionWhenAccountNotFound()
    {
        var accountId = Guid.NewGuid();
        var unitOfWork = new Mock<IUnitOfWork>();
        var accountRepository = new Mock<IAccountRepository>();
        accountRepository.Setup(accountRepository => accountRepository.LoadAsync(accountId, CancellationToken.None)).ReturnsAsync((AccountAggregate?)null);
        var command = new DepositAccountCommand(accountId, 2, 100);
        unitOfWork.Setup(unitOfWork => unitOfWork.Accounts).Returns(accountRepository.Object);
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await DepositAccountCommandHandler.Handle(command, unitOfWork.Object, CancellationToken.None));

        unitOfWork.Verify(unitOfWork => unitOfWork.Accounts.LoadAsync(accountId, CancellationToken.None), Times.Once);
        unitOfWork.Verify(unitOfWork => unitOfWork.Accounts.SaveAsync(It.IsAny<AccountAggregate>(), command.ExpectedVersion, CancellationToken.None), Times.Never);
        unitOfWork.Verify(unitOfWork => unitOfWork.PublishAsync(It.IsAny<AccountBalanceUpdatedIntegrationEvent>()), Times.Never);
        unitOfWork.Verify(unitOfWork => unitOfWork.CommitAsync(CancellationToken.None), Times.Never);
    }
}