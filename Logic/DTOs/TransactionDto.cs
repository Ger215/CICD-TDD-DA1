using Models;

namespace Logic.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public double Amount { get; set; }
        public ExchangeRate ExchangeRateAssociated { get; set; }
        public Category CategoryAssociated { get; set; }
        public Account AccountAssociated { get; set; }

        public TransactionDto()
        {
            AccountAssociated = new Account();
            CategoryAssociated = new Category();
            ExchangeRateAssociated = new ExchangeRate();
        }

        public TransactionDto(int id, string transactionTitle, DateTime transactionCreationDate, 
                              double transactionAmount,ExchangeRate exchangeRateAssociated,Category categoryAssociated, Account transactionAccount)
        {
            Id = id;
            Title = transactionTitle;
            CreationDate = transactionCreationDate;
            Amount = transactionAmount;
            ExchangeRateAssociated = exchangeRateAssociated;
            CategoryAssociated = categoryAssociated;
            AccountAssociated = transactionAccount;
        }

    }
} 