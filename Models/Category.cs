using Models.Enums;
using Models.Exceptions;

namespace Models
{
    public class Category
    {
        private string _categoryName = "";
        private StatusType _status = StatusType.Active;

        public ICollection<Transaction> _transactions { get; set; }
        
        public ICollection<SpendingGoal> _spendingGoals { get; set; }

        public ICollection<Transaction> Transactions
        {
            get => _transactions;
            set
            {
                _transactions = value;
                if (TransactionsAreNotOfTheSameType())
                {
                    throw new CategoryExceptions("The category type and the transaction type must be the same");
                }
            }
        }
        public int? UserId { get; set; }
        
        public DateTime CreationDate { get; set; }
        public CategoryType TypeOf { get; set; }

        public StatusType Status
        {
            get => _status;
            set
            {
                _status = value;
                if (StatusChangeInactive(value))
                {
                    CategoryModificationError();
                }
            }
        }

        public string Name
        {
            get => _categoryName;
            set
            {

                _categoryName = value;

                if (CategoryNameIsEmpty())
                {
                    EmptyCategoryNameException();
                }

            }
        }

        public int Id { get; set; }
        
        public Category()
        {
            Transactions = new List<Transaction>();

        }
        public Category(string categoryName, DateTime categoryCreationDate, CategoryType categoryType, StatusType statusType)
        {
            Name = categoryName;
            CreationDate = categoryCreationDate;
            TypeOf = categoryType; 
            Transactions = new List<Transaction>();
            Status = statusType;
        }




        private bool TransactionsAreNotOfTheSameType()
        {
            foreach (Transaction transaction in Transactions)
            {
                if (TransactionsTypeAreDifferent(transaction))
                {
                    return true;
                }
            }
            return false;
        }

        private bool TransactionsTypeAreDifferent(Transaction transaction)
        {
            return transaction.CategoryAssociated.TypeOf != TypeOf;
        }

        private static void EmptyCategoryNameException()
        {
            throw new CategoryExceptions("The Category name can't be an empty one");
        }

        private bool CategoryNameIsEmpty()
        {
            return string.IsNullOrEmpty(_categoryName);
        }
        private bool StatusChangeInactive(StatusType value)
        {
            if (value == StatusType.Inactive)
            {
                return this.Transactions.Count > 0;
            }
            return false;
        }

        private void CategoryModificationError()
        {
            throw new CategoryExceptions("The category status can't be inactive, this category has transactions associated");

        }
    }
}
