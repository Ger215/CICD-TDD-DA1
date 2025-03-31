using Models.Enums;

namespace Models
{
    public class GeneralAccount : Account
    {
        public double InitialAmmount { get; set; }

        public GeneralAccount() { }
        public GeneralAccount(string accountName, Currency accountCurrency, DateTime accountCreationDate, double accountInitialAmmount) 
                             : base(accountName, accountCurrency, accountCreationDate)
        {
            Name = accountName;
            AccountCurrency = accountCurrency;
            CreationDate = accountCreationDate;
            InitialAmmount = accountInitialAmmount;
        }

    }
}
