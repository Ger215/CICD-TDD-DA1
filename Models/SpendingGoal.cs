using Models.Exceptions;
using Models.Enums;

namespace Models
{
    public class SpendingGoal
    {

        private string _title= "";
        
        public int? UserId { get; set; }
        
        public double MaximumAmount { get; set; }
        
        public Currency GoalCurrency { get; set; }
        public ICollection<Category> Categories { get; set; }
        public int Id { get; set; }
        
        public string Title
        {
            get => _title;
            set
            {

                _title = value;

                if (SpendingGoalTitleIsEmpty())
                {
                    EmptySpendingGoalTitleException();
                }

            }
        }

        public SpendingGoal() { }
        public SpendingGoal(string title, double maximumAmount, Currency goalCurrency, ICollection<Category> categories)
        {
            Title = title;
            MaximumAmount = maximumAmount;
            GoalCurrency = goalCurrency;
            Categories = categories;
        }

        private static void EmptySpendingGoalTitleException()
        {
            throw new SpendingGoalExceptions("The Spending Goal title can't be an empty one");
        }

        private bool SpendingGoalTitleIsEmpty()
        {
            return string.IsNullOrEmpty(_title);
        }
        

    }
}
