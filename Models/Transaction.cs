using Models.Enums;
using Models.Exceptions;

namespace Models
{
    public class Transaction
    {

        private const double MinimumValidAmount = 0.0;

        private string _title = "";

        private double _amount = 0.0;

        private Account _accountAssociated = null;

        private Category _categoryAssociated = null;

        private ExchangeRate _exchangeRate = null;



        public DateTime Date { get; set; }

        public int? AccountId { get; set; }

        public Account AccountAssociated { 
            get => _accountAssociated;
            set{
                _accountAssociated = value;
                if (!SameAccountCurrency(value))
                {
                    throw new TransactionExceptions("The Transaction account must have the same currency as the Transaction");
                }
            }
        }


        public int? UserId { get; set; }
        public int? CategoryId { get; set; }


        public Category CategoryAssociated { 
            get => _categoryAssociated;
            set
            {
                _categoryAssociated = value;
                if (CategoryStatusInactive())
                {
                    throw new TransactionExceptions("The Transaction category can't be inactive");
                }
            }
        }



        public int? ExchangeRateId { get; set; }

        public ExchangeRate ExchangeRateAssociated {
            get => _exchangeRate;
            set {
                _exchangeRate = value;
                if (!ExistsRateInDate(value))
                {
                    throw new TransactionExceptions("The Transaction exchange rate has to exist in the Transactions Date");
                }
            }
        }


        public int Id { get; set; }
        
        public double Amount
        {
            get => _amount;
            set
            {

                _amount = value;

                if (TransactionAmountIsLowerThanZero())
                {
                    TransactionAmountException();
                }

            }
        }

        public string Title
        {
            get => _title;
            set
            {

                _title = value;

                if (TransactionTitleIsEmpty())
                {
                    EmptyTransactionTitleException();
                }

            }
        }

        public Transaction() { }
        public Transaction(string title, DateTime date, double amount, ExchangeRate exchangeRateAssociated,Category categoryAssociated, 
                            Account account)
        {
            Title = title;
            Date = date;
            Amount = amount;
            ExchangeRateAssociated = exchangeRateAssociated;
            CategoryAssociated = categoryAssociated;
            AccountAssociated = account;
        }


        private bool ExistsRateInDate(ExchangeRate value)
        {
            return value.Date == this.Date;
        }

        private bool CategoryStatusInactive()
        {
            return CategoryAssociated.Status == StatusType.Inactive;
        }

        private bool SameAccountCurrency(Account value)
        {
            return value.AccountCurrency == this.ExchangeRateAssociated.RateCurrency; 
        }
        private static void EmptyTransactionTitleException()
        {
            throw new TransactionExceptions("The Transaction title can't be an empty one");
        }

        private bool TransactionTitleIsEmpty()
        {
            return string.IsNullOrEmpty(_title);
        }
        
        private void TransactionAmountException()
        {
            throw new TransactionExceptions("The Transaction amount can't be lower than 0.0");

        }

        private bool TransactionAmountIsLowerThanZero()
        {
            return this._amount < MinimumValidAmount;
        }

        public Transaction DuplicateTransaction()
        {
            string title = this.Title;
            double amount = this.Amount;
            ExchangeRate exchangeRateAssociated = this.ExchangeRateAssociated;
            Category categoryAssociated = this.CategoryAssociated;
            Account accountAssociated = this.AccountAssociated;
            Transaction transaction = new Transaction(title, this.Date, amount,exchangeRateAssociated, categoryAssociated, 
                                                       accountAssociated);
            return transaction;
        }

    }
}
