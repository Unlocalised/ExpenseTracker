using ExpenseService.Application.Account.CreateAccount;
using ExpenseService.Application.Common;
using ExpenseService.Domain.Transaction;
using ExpenseTracker.Contracts.Account;
using ExpenseService.Domain.Account;
using Moq;

namespace ExpenseService.UnitTests.Application.Account.CreateAccount;

public class CreateAccountTests
{
    [Fact]
    public async Task CreateAccount_ShouldCreateAccountAndPublishEventAndCommit()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var accountRepository = new Mock<IAccountRepository>();
        var command = new CreateAccountCommand { Name = "test" };
        unitOfWork.Setup(unitOfWork => unitOfWork.Accounts).Returns(accountRepository.Object);
        var result = await CreateAccountCommandHandler.Handle(command, unitOfWork.Object, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result.AccountId);
        Assert.Equal(1, result.NewVersion);
        unitOfWork.Verify(unitOfWork => unitOfWork.Accounts.Create(It.IsAny<AccountAggregate>()), Times.Once);
        unitOfWork.Verify(unitOfWork => unitOfWork.PublishAsync(It.IsAny<AccountCreatedIntegrationEvent>()), Times.Once);
        unitOfWork.Verify(unitOfWork => unitOfWork.CommitAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task CreateAccount_ShouldCreateAccountAndCreateTransactionAndPublishEventAndCommitWithOpeningBalance()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var accountRepository = new Mock<IAccountRepository>();
        var transactionRepository = new Mock<ITransactionRepository>();
        var command = new CreateAccountCommand { Name = "test", OpeningBalance = 100 };
        unitOfWork.Setup(unitOfWork => unitOfWork.Accounts).Returns(accountRepository.Object);
        unitOfWork.Setup(unitOfWork => unitOfWork.Transactions).Returns(transactionRepository.Object);
        var result = await CreateAccountCommandHandler.Handle(command, unitOfWork.Object, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result.AccountId);
        Assert.Equal(2, result.NewVersion);
        unitOfWork.Verify(unitOfWork => unitOfWork.Accounts.Create(It.IsAny<AccountAggregate>()), Times.Once);
        unitOfWork.Verify(unitOfWork => unitOfWork.Transactions.Create(It.IsAny<TransactionAggregate>()), Times.Once);
        unitOfWork.Verify(unitOfWork => unitOfWork.PublishAsync(It.IsAny<AccountCreatedIntegrationEvent>()), Times.Once);
        unitOfWork.Verify(unitOfWork => unitOfWork.CommitAsync(CancellationToken.None), Times.Once);
    }
}