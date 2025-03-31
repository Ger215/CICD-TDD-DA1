using Models.Enums;
using Models.Exceptions;

namespace Models
{
    public class Account
    {
        private string _accountName = "";

        public int? UserId { get; set; }
        
        public ICollection<Transaction> _transactions { get; set; }

        public string Name
        {
            get => _accountName;
            set
            {

                _accountName = value;

                if (AccountNameIsEmpty())
                {
                    EmptyAccountNameException();
                }

            }
        }

        public Currency AccountCurrency { get; set; }
        public DateTime CreationDate { get; set; }
        public int Id { get; set; }

        public Account() { }
        public Account(string accountName, Currency accountCurrency, DateTime accountCreationDate)
        {
            Name = accountName;
            AccountCurrency = accountCurrency;
            CreationDate = accountCreationDate;
        }



        private void EmptyAccountNameException()
        {
            throw new AccountExceptions("The Account name can't be an empty one");
        }

        private bool AccountNameIsEmpty()
        {
            return string.IsNullOrEmpty(_accountName);
        }

    }
}
