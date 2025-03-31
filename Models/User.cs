using Models.Exceptions;
using System.Text.RegularExpressions;

namespace Models
{
    public class User
    {
        private string _name="";
        private string _surname = "";
        private string _email = "";
        private string _password = "";
        private const int MinumumPasswordLength = 10;
        private const int MaximumPasswordLength = 30;
        public ICollection<ExchangeRate> ExchangeRates { get; set;}
        public ICollection<SpendingGoal> Goals { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<GeneralAccount> UserGeneralAccounts { get; set; }
        public ICollection<CreditCardAccount> UserCreditCards { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public string Name {
            get => _name;
            set
            {
                _name = value;
                if (StringIsEmpty(value))
                {
                    NameModificationError();
                }
            }
        }

        public string Surname {
            get => _surname;
            set
            {
                _surname = value;
                if (StringIsEmpty(value))
                {
                    SurnameModificationError();
                }
            }
        }
        public string Email { 
            get => _email;
            set
            {
                _email = value;
                if (StringIsEmpty(value)||!EmailValidationPattern())
                {
                    EmailInvalidError();
                }
            }
        }
        public string Password{
            get => _password;
            set
            {
                _password = value;
                if (!ValidatePassword())
                {
                    PasswordInvalidError();
                }
            }
        }
        
        public string AddressDirection { get; set; }
        
        public int Id { get; set; }

        public User()
        {

        }
        public User(string userEmail, string userName, string userSurname, string userPassword)
        {
            Email = userEmail;
            Name = userName;
            Surname = userSurname;
            Password = userPassword;
            Goals = new List<SpendingGoal>();
            Categories = new List<Category>();
            UserGeneralAccounts = new List<GeneralAccount>();
            UserCreditCards = new List<CreditCardAccount>();
            ExchangeRates = new List<ExchangeRate>();
            Transactions = new List<Transaction>();
        }

        public User(string userEmail, string userName, string userSurname, string userPassword, string userAddressDirection)
        {
            Email = userEmail;
            Name = userName;
            Surname = userSurname;
            Password = userPassword;
            AddressDirection = userAddressDirection;
            Goals = new List<SpendingGoal>();
            Categories = new List<Category>();
            UserGeneralAccounts = new List<GeneralAccount>();
            UserCreditCards = new List<CreditCardAccount>();
            ExchangeRates = new List<ExchangeRate>();
            Transactions = new List<Transaction>();

        }

        private static void EmailInvalidError()
        {
            throw new UserExceptions("The email is not valid");
        }
        private static void SurnameModificationError()
        {
            throw new UserExceptions("The surname can't be an empty one");
        }
        private static void NameModificationError()
        {
            throw new UserExceptions("The name can't be an empty one");
        }
        private static void PasswordInvalidError()
        {
            throw new UserExceptions("The password is not valid");
        }
        private static bool StringIsEmpty(string x)
        {
            return String.IsNullOrEmpty(x);
        }
        public bool ValidatePassword()
        {
           return HasCorrectNumberOfDigits() && PasswordHasAtLeastOneUppercaseLetter();
        }
        private bool HasCorrectNumberOfDigits()
        {
           return !PasswordMaximumLengthIsNotValid() && !PasswordMinimumLengthIsNotValid();
        }
        private bool PasswordHasAtLeastOneUppercaseLetter()
        {
            bool hasAtLeastOneUppercaseLetter = false;

            for(int letterPosition=0; letterPosition < PasswordLength() && !hasAtLeastOneUppercaseLetter; letterPosition++)
            {
                if (IsAnUppercaseLetter(letterPosition))
                {
                    hasAtLeastOneUppercaseLetter = true;
                }
            }

            return hasAtLeastOneUppercaseLetter;
        }

        private bool IsAnUppercaseLetter(int letterPosition)
        {
            return char.IsUpper(this.Password[letterPosition]);
        }

        private bool PasswordMaximumLengthIsNotValid()
        {
            return PasswordLength() > MaximumPasswordLength;
        }
        private bool PasswordMinimumLengthIsNotValid()
        {
            return PasswordLength() < MinumumPasswordLength;
        }
        private int PasswordLength()
        {
            return this.Password.Length;
        }
        public bool EmailValidationPattern()
        {
            string pattern = @"^[a-zA-Z0-9.%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return (Regex.IsMatch(this.Email, pattern));
        }
        public void AreEqual(User user2)
        {
            if (Equals(user2))
            {
                throw new UserExceptions("The User already Existss");
            }
        }

        public override bool Equals(object obj)
        {
            User user = (User) obj;
            if (user == null)
            {
                return false;
            }

            return string.Equals(this.Email, user.Email);
        }
    }



}
