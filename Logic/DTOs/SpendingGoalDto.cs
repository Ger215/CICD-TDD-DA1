using Models;
using Models.Enums;

namespace Logic.DTOs;

public class SpendingGoalDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public double MaxAmount { get; set; }
    
    public Currency GoalCurrency { get; set; }
    public ICollection<Category> Categories { get; set; }

    public SpendingGoalDto()
    {
        Categories = new List<Category>();
    }

    public SpendingGoalDto(int id, string title, double maxAmount, Currency goalCurrency,ICollection<Category> categories)
    {
        Id = id;
        Title = title;
        MaxAmount = maxAmount;
        GoalCurrency = goalCurrency;
        Categories = categories;
    }
}