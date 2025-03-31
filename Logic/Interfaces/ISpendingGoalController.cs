using Logic.DTOs;
using Models;

namespace Logic.Interfaces;

public interface ISpendingGoalController
{
    public SpendingGoal CreateSpendingGoal(SpendingGoalDto spendingGoalDto);
    public void AddSpendingGoal(SpendingGoalDto spendingGoalDto, int userId);
    public SpendingGoalDto GetSpendingGoalById(int i);
    public List<SpendingGoalDto> GetSpendingGoals(int userId);
    public void ChangeSpendingGoalMaxAmount(SpendingGoal spendingGoal, SpendingGoalDto spendingGoalDto, int userId);
    public void ChangeSpendingGoalCategories(SpendingGoal spendingGoal, SpendingGoalDto spendingGoalDto , int userId);
    public void UpdateSpendingGoal(SpendingGoalDto spendingGoalDto, int userId);
}