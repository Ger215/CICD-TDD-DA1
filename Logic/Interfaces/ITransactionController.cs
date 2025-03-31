using Logic.DTOs;
using Models;

namespace Logic.Interfaces;

public interface ITransactionController
{
    public Transaction CreateTransaction(TransactionDto transactionDto);
    public void AddTransaction(TransactionDto transactionDto,int userId, int accountId, int categoryId, int exchangeRateId);
    public List<TransactionDto> GetTransactions(int userId);
    public TransactionDto GetTransactionById(int id);
    public void ChangeTransactionCurrency(Transaction transaction, TransactionDto transactionDto);
    public void ChangeTransactionAmount(Transaction transaction, TransactionDto transactionDto);
    public void ChangeTransactionCategory(Transaction transaction, TransactionDto transactionDto);
    public void UpdateTransaction(TransactionDto transactionDto);
    public void UpdateTransactionCategory(TransactionDto transactionDto,CategoryDto categoryDto);
    public void UpdateTransactionExchange(TransactionDto transactionDto, ExchangeRateDto exchangeDto);

}