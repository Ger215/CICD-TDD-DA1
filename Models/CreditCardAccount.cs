using Models.Enums;
using Models.Exceptions;

namespace Models
{
    public class CreditCardAccount : Account
    {
        private string _issuingBank = "";

        private string _lastFourDigits = "";

        public string LastFourDigits
        {
            get => _lastFourDigits;
            set
            {
                _lastFourDigits = value;
                if (StringIsEmpty(value) || _lastFourDigits.Length != 4)
                {
                    LastFourDigitsModificationError();
                }
            }
        }

        public string IssuingBank
        {
            get => _issuingBank;
            set
            {
                _issuingBank = value;
                if (StringIsEmpty(value))
                {
                    IssuingBankModificationError();
                }
            }
        }

        public double AvailableBalance { get; set; }
        public DateTime ClosingDate { get; set; }

        public CreditCardAccount() { }
        public CreditCardAccount(string accountName, Currency accountCurrency, DateTime accountCreationDate, 
                                 string accountIssuingBank, string accountLastFourDigits, double accountAvailableBalance, DateTime accountClosingDate) 
                                : base(accountName, accountCurrency, accountCreationDate)
        {
            Name = accountName;
            AccountCurrency = accountCurrency;
            CreationDate = accountCreationDate;
            IssuingBank = accountIssuingBank;
            LastFourDigits = accountLastFourDigits;
            AvailableBalance = accountAvailableBalance;
            ClosingDate = accountClosingDate;
        }
        
        public CreditCardAccount(string accountIssuingBank, string accountLastFourDigits, Currency accountCurrency ,double accountAvailableBalance, 
                                 DateTime accountCreationDate, DateTime accountClosingDate) : base(accountIssuingBank,accountCurrency, accountCreationDate)
        {
            IssuingBank = accountIssuingBank;
            LastFourDigits = accountLastFourDigits;
            AccountCurrency = accountCurrency;
            AvailableBalance = accountAvailableBalance;
            CreationDate = accountCreationDate;
            ClosingDate = accountClosingDate;
        }



        private void IssuingBankModificationError()
        {
            throw new AccountExceptions("The Issuing Bank can't be an empty one");
        }

        private void LastFourDigitsModificationError()
        {
            throw new AccountExceptions("The last fours digits must have 4 numbers");
        }

        private bool StringIsEmpty(string value)
        {
            return String.IsNullOrEmpty(value);
        }


    }
}
