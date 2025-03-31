using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Exceptions;

namespace DataAccess.Repository;

public class SpendingGoalsRepository
{
    private ApplicationDbContext _database;

    public SpendingGoalsRepository(ApplicationDbContext database)
    {
        _database = database;
    }
    
    public void AddSpendingGoal(SpendingGoal newSpendingGoal,int userId)
    {
        if (SpendingGoalAlreadyExists(newSpendingGoal,userId))
        {
            SpendingGoalAlreadyExistsException();
        }

        AddNewSpendingGoalToSpendingGoalTable(newSpendingGoal, userId);

    }

    private void AddNewSpendingGoalToSpendingGoalTable(SpendingGoal newSpendingGoal, int userId)
    {
        newSpendingGoal.UserId = userId;
        _database.SpendingGoals.Add(newSpendingGoal);
        _database.SaveChanges();
    }

    private void SpendingGoalAlreadyExistsException()
    {
        throw new RepositoryExceptions("The Spending Goal already exists");
    }

    private bool SpendingGoalAlreadyExists(SpendingGoal newSpendingGoal, int userId)
    {
        return _database.SpendingGoals.Any(spendingGoal => spendingGoal.Title == newSpendingGoal.Title && spendingGoal.UserId == userId);
    }

    public SpendingGoal FindSpendingGoalById(int id)
    {
        SpendingGoal spendingGoal = _database.SpendingGoals.FirstOrDefault(sg => sg.Id == id);
        return spendingGoal;
    }

    public List<SpendingGoal> GetSpendingGoals(int userId)
    {
        return _database.SpendingGoals.Include(s=>s.Categories).Where(sp => sp.UserId == userId).ToList();
    }

    public void UpdateSpendingGoal(SpendingGoal spendingGoal, int userId) 
    {
        var dbSpendingGoal = _database.SpendingGoals.FirstOrDefault(sp => sp.Id == spendingGoal.Id && userId == sp.UserId);
        if (dbSpendingGoal != null)
        {
            _database.SpendingGoals.Update(spendingGoal);
            _database.SaveChanges();
        }
    }
}