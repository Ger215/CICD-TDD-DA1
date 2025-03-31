using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Models.Enums;
using Models;

namespace DataAccess.Repository
{
    public class TransactionsRepository
    {
        private ApplicationDbContext _database;

        public TransactionsRepository(ApplicationDbContext database)
        {
            _database = database;
        }
        
        public void AddTransaction(Transaction newTransaction,int userId, int accountId, int categoryId, int exchangeRateId)
        {
            newTransaction.UserId = userId;
            newTransaction.AccountId = accountId;
            newTransaction.CategoryId = categoryId;
            newTransaction.ExchangeRateId = exchangeRateId;
            _database.Transactions.Add(newTransaction);
            _database.SaveChanges();
        }

        public bool TransactionAlreadyExists(Transaction transaction)
        {
            return _database.Transactions.Any(t => t.Id == transaction.Id);
        }

        public List<Transaction> GetTransactions(int userId)
        {
            User user = _database.Users.FirstOrDefault(u => u.Id == userId);
            return _database.Transactions.Include(s=>s.CategoryAssociated).Include(s =>s.ExchangeRateAssociated).Where(tr => tr.UserId == userId).ToList();
        }

        public Transaction FindTransactionById(int id)
        {
            Transaction transaction = _database.Transactions.FirstOrDefault(tr => tr.Id == id);
            return transaction;
        }

        public void UpdateTransaction(Transaction transaction)
        {
            Transaction dbTransaction = _database.Transactions.FirstOrDefault(tr => tr.Id == transaction.Id);
            if (dbTransaction != null)
            {
                dbTransaction.Amount = transaction.Amount;
                dbTransaction.CategoryAssociated = transaction.CategoryAssociated;
                _database.SaveChanges();
            }
        }

        public void UpdateTransactionRate(Transaction transaction)
        {
            Transaction dbTransaction = _database.Transactions.FirstOrDefault(tr => tr.Id == transaction.Id);
            if (dbTransaction != null)
            {
                dbTransaction.Amount = transaction.Amount;
                dbTransaction.ExchangeRateAssociated = transaction.ExchangeRateAssociated;
                _database.SaveChanges();
            }
        }
        public int GetTotalSpentInMonth(int userId, DateTime month)
        {
            double totalSpent = 0;
            List<Transaction> transactions = GetTransactions(userId);
            foreach (var transaction in transactions)
            {
                if (transaction.CategoryAssociated.TypeOf == CategoryType.ExpensesCategory)
                {
                    if (transaction.Date.Month == month.Month && transaction.Date.Year == month.Year)
                    {
                        totalSpent += transaction.Amount * transaction.ExchangeRateAssociated.Value;
                    }
                }
            }
            return (int) totalSpent;
        }
    }
}
