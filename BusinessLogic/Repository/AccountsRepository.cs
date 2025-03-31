using DataAccess.Context;
using Models;
using Models.Exceptions;

namespace DataAccess.Repository;

public class AccountsRepository
{
    private ApplicationDbContext _database;

    public AccountsRepository(ApplicationDbContext database)
    {
        _database = database;
    }

    public void AddGeneralAccount(GeneralAccount newGeneralAccount , int userId)
    {
        if (GeneralAccountAlreadyExists(newGeneralAccount,userId))
        {
            GeneralAccountAlreadyExistsException();
        }

        AddNewGeneralAccountToGeneralAccountsTable(newGeneralAccount,userId);
    }

    private void AddNewGeneralAccountToGeneralAccountsTable(GeneralAccount newGeneralAccount, int userId)
    {
        newGeneralAccount.UserId = userId;
        _database.Accounts.Add(newGeneralAccount);

        _database.SaveChanges();
    }
    public void AddAccount(Account account, int userId)
    {
        account.UserId = userId;
        _database.Accounts.Add(account);

        _database.SaveChanges();
    }
    private void GeneralAccountAlreadyExistsException()
    {
        throw new RepositoryExceptions("General Account Already Exists");
    }

    public bool GeneralAccountAlreadyExists(GeneralAccount generalAccount, int userId)
    {
        return _database.GeneralAccounts.Any(account => account.Name == generalAccount.Name && account.UserId == userId);
    }



    public List<GeneralAccount> GetGeneralAccounts(int userId)
    {
        User user = _database.Users.FirstOrDefault(u => u.Id == userId);
        return _database.GeneralAccounts.Where(c => c.UserId == userId).ToList();
    }

    public GeneralAccount FindGeneralAccountById(int id)
    {
        GeneralAccount generalAccount = _database.GeneralAccounts.FirstOrDefault(acc => acc.Id == id);
        return generalAccount;
    }

    public GeneralAccount FindGeneralAccountByName(string name, int userId)
    {
        GeneralAccount generalAccount = _database.GeneralAccounts.FirstOrDefault(acc => acc.Name == name && acc.UserId == userId);
        return generalAccount;
    }

    public void UpdateGeneralAccount(GeneralAccount generalAccount)
    {
        var dbCategory = _database.GeneralAccounts.FirstOrDefault(acc => acc.Id == generalAccount.Id);
        if (dbCategory != null)
        {
            dbCategory.Name = generalAccount.Name;
            _database.SaveChanges();
        }
    }


    public void DeleteGeneralAccount(string name, int userId)
    {
        var generalAccount = _database.GeneralAccounts.FirstOrDefault(acc => acc.Name == name && acc.UserId== userId);
        if (generalAccount != null)
        {
            _database.Accounts.Remove(generalAccount);
            _database.SaveChanges();
        }
    }

    public void AddCreditCardAccount(CreditCardAccount newCreditCardAccount, int userId)
    {
        if (CreditCardAccountAlreadyExists(newCreditCardAccount,userId))
        {
            CreditCardAccountAlreadyExistsException();
        }

        AddNewCreditCardAccountToCreditCardAccountsTable(newCreditCardAccount,userId);
    }

    private void AddNewCreditCardAccountToCreditCardAccountsTable(CreditCardAccount newCreditCardAccount, int userId)
    {
        newCreditCardAccount.UserId = userId;
        _database.Accounts.Add(newCreditCardAccount);
        _database.CreditCardAccounts.Add(newCreditCardAccount);

        _database.SaveChanges();
    }

    private void CreditCardAccountAlreadyExistsException()
    {
        throw new RepositoryExceptions("Credit Card Account Already Exists");
    }

    public bool CreditCardAccountAlreadyExists(CreditCardAccount newCreditCardAccount, int userId)
    {
        return _database.CreditCardAccounts.Any(account => account.IssuingBank == newCreditCardAccount.IssuingBank && account.LastFourDigits == newCreditCardAccount.LastFourDigits && account.UserId == userId);
    }

    public List<CreditCardAccount> GetCreditCardAccounts(int userId)
    {
        User user = _database.Users.FirstOrDefault(u => u.Id == userId);
        List<CreditCardAccount> RET = _database.CreditCardAccounts.Where(ac => ac.UserId == userId).ToList();
        return RET;
    }

    public CreditCardAccount FindCreditCardAccountById(int id)
    {
        CreditCardAccount creditCardAccount = _database.CreditCardAccounts.FirstOrDefault(acc => acc.Id == id);
        return creditCardAccount;
    }

    public Account FindAccountById(int id)
    {
        Account account = _database.Accounts.FirstOrDefault(acc => acc.Id == id);
        return account;
    }

    public void UpdateCreditCardAccount(CreditCardAccount creditCardAccount, int userId)
    {
        var dbCreditCardAccount = _database.CreditCardAccounts.FirstOrDefault(acc => acc.Id == creditCardAccount.Id && acc.UserId == userId);
        if (dbCreditCardAccount != null)
        {
            dbCreditCardAccount.IssuingBank = creditCardAccount.IssuingBank;
            dbCreditCardAccount.LastFourDigits = creditCardAccount.LastFourDigits;
            dbCreditCardAccount.AccountCurrency = creditCardAccount.AccountCurrency;
            dbCreditCardAccount.ClosingDate = creditCardAccount.ClosingDate;
            dbCreditCardAccount.Name = creditCardAccount.IssuingBank;
            _database.SaveChanges();
        }
    }
    
    public void DeleteCreditCardAccount(int id, int userId)
    {
        var creditCardAccount = _database.CreditCardAccounts.FirstOrDefault(acc => acc.Id == id && userId == acc.UserId);
        if (creditCardAccount != null)
        {
            _database.Accounts.Remove(creditCardAccount);
            _database.SaveChanges();
        }
    }
    
    public List<Account> GetAccounts(int userId)
    {
        User user = _database.Users.FirstOrDefault(u => u.Id == userId);
        List<Account> accounts = _database.Accounts.Where(ac => ac.UserId == userId).ToList();
        return accounts;
    }
}