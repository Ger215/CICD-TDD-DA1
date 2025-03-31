using DataAccess.Context;
using DataAccess.Repository;
using Logic.DTOs;
using Logic.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.Enums;
using Models.Exceptions;


namespace Logic;

public class ApplicationController : IUserAuthentication, IUserModification, ICategoryController, IAccountController ,IGeneralAccountController, 
                                     ICreditCardAccountController, ITransactionController, IExchangeRateController, ISpendingGoalController
{
    public UsersRepository _userRepository;
    public CategoryRepository _categoryRepository;
    public AccountsRepository _accountsRepository;
    public TransactionsRepository _transactionsRepository;
    public ExchangeRatesRepository _exchangeRatesRepository;
    public SpendingGoalsRepository _spendingGoalsRepository;

    public ApplicationController(ApplicationDbContext context)
    {
        _userRepository = new UsersRepository(context);
        _categoryRepository = new CategoryRepository(context);
        _accountsRepository = new AccountsRepository(context);
        _transactionsRepository = new TransactionsRepository(context);
        _exchangeRatesRepository = new ExchangeRatesRepository(context);
        _spendingGoalsRepository = new SpendingGoalsRepository(context);
    }
    public User CreateUser(UserDto userDto)
    {
        if (userDto.Password.IsNullOrEmpty())
        {
            throw new UserExceptions("Password cannot be empty");
        }
        return new User(userDto.Email, userDto.Name, userDto.Surname, userDto.Password, userDto.Address);
    }

    public void AddUser(UserDto userDto)
    {
        User newUser = CreateUser(userDto);

        _userRepository.AddUser(newUser);
    }

    public bool CheckUserLogin(string email, string password)
    {
        return _userRepository.CheckLogin(email, password);
    }

    public User FindUserByEmail(string email)
    {
        return _userRepository.FindUserByEmail(email);
    }
    public void ChangeUserName(User user, UserDto userDto)
    {
        user.Name = userDto.Name;

        _userRepository.UpdateUser(user);
    }

    public void ChangeUserSurname(User user, UserDto userDto)
    {
        user.Surname = userDto.Surname;

        _userRepository.UpdateUser(user);
    }

    public void ChangeUserAddress(User user, UserDto userDto)
    {
        user.AddressDirection = userDto.Address;

        _userRepository.UpdateUser(user);
    }

    public void ChangeUserPassword(User user, UserDto userDto)
    {
        user.Password = userDto.Password;

        _userRepository.UpdateUser(user);
    }
    
    public Category CreateCategory(CategoryDto categoryDto, int userId)
    {
        Category category = FindCategoryByName(categoryDto.Name, userId);
        if (category != null)
        {
            return category;
        }
        
        return new Category(categoryDto.Name, categoryDto.CreationDate, categoryDto.TypeOf, categoryDto.Status);
    }

    public void AddCategory(CategoryDto categoryDto, int userId)
    {
        Category newCategory = CreateCategory(categoryDto, userId);

        _categoryRepository.AddCategory(newCategory, userId);
    }

    public Category FindCategoryByName(string name, int userId)
    {
        return _categoryRepository.FindCategoryByName(name, userId);
    }

    public void ChangeCategoryName(Category category, CategoryDto categoryDto)
    {
        category.Name = categoryDto.Name;

        _categoryRepository.UpdateCategory(category);
    }

    public void ChangeCategoryType(Category category, CategoryDto categoryDto)
    {
        category.TypeOf = categoryDto.TypeOf;

        _categoryRepository.UpdateCategory(category);
    }

    public void ChangeCategoryStatus(Category category, CategoryDto categoryDto)
    {
        category.Status = categoryDto.Status;

        _categoryRepository.UpdateCategory(category);
    }

    public void ChangeCategoryCreationDate(Category category, CategoryDto categoryDto)
    {
        category.CreationDate = categoryDto.CreationDate;

        _categoryRepository.UpdateCategory(category);
    }

    public List<CategoryDto> GetAllCategories(int userId)
    {
        List<Category> categories = _categoryRepository.GetCategories(userId);

        List<CategoryDto> categoriesDto = new List<CategoryDto>();

        foreach (var category in categories)
        {
            categoriesDto.Add(new CategoryDto(category.Id, category.Name, category.CreationDate, category.TypeOf, category.Status));
        }

        return categoriesDto;
    }

    public List<CategoryDto> GetAllCategoriesForSpending(int userId)
    {
        List<Category> categories = _categoryRepository.GetCategories(userId);
        
        List<CategoryDto> categoriesDto = new List<CategoryDto>();

        foreach (var category in categories)
        {
            if(category.Status == StatusType.Active && category.TypeOf == CategoryType.ExpensesCategory)
            {
                categoriesDto.Add(new CategoryDto(category.Id, category.Name, category.CreationDate, category.TypeOf, category.Status));
            }
        }

        return categoriesDto;

    }

    public void DeleteCategory(CategoryDto categoryDto, int userId)
    {
        Category category = _categoryRepository.FindCategoryByName(categoryDto.Name, userId);
        if(category.Transactions.Count > 0)
        {
            throw new RepositoryExceptions("The category has transactions associated");
        }

        _categoryRepository.DeleteCategory(category.Name, userId);
    }

    public CategoryDto GetCategoryById(int id)
    {
        Category category = _categoryRepository.FindCategoryById(id);

        return new CategoryDto(category.Id, category.Name, category.CreationDate, category.TypeOf, category.Status);
    }

    public void UpdateCategory(CategoryDto element,int userId)
    {
        Category category = _categoryRepository.FindCategoryById(element.Id);
        List<Category> userCategories = _categoryRepository.GetCategories(userId);
        foreach (var c in userCategories)
        {
            if(element.Name == c.Name && element.Status == c.Status)
            {
                throw new RepositoryExceptions("The category already exists");
            }
        }

        category.Name = element.Name;
        category.Status = element.Status;

        _categoryRepository.UpdateCategory(category);
    }
    
    public GeneralAccount CreateGeneralAccount(GeneralAccountDto generalAccountDto,int userId)
    {
        GeneralAccount generalAccount = FindGeneralAccountByName(generalAccountDto.Name, userId);
        if (generalAccount != null)
        {
            return generalAccount;
        }
        return new GeneralAccount(generalAccountDto.Name,generalAccountDto.AccountCurrency,generalAccountDto.CreationDate,generalAccountDto.InitialAmmount);
    }

    public void AddGeneralAccount(GeneralAccountDto generalAccountDto,int userId)
    {
        GeneralAccount newGeneralAccount = CreateGeneralAccount(generalAccountDto,userId);

        _accountsRepository.AddGeneralAccount(newGeneralAccount,userId);
    }

    public GeneralAccount FindGeneralAccountByName(string name, int userId)
    {
        return _accountsRepository.FindGeneralAccountByName(name,userId);
    }

    public List<GeneralAccountDto> GetGeneralAccounts(int userId)
    {
        List<GeneralAccount> generalAccounts = _accountsRepository.GetGeneralAccounts(userId);

        List<GeneralAccountDto> generalAccountsDto = new List<GeneralAccountDto>();

        foreach (var generalAccount in generalAccounts)
        {
            generalAccountsDto.Add(new GeneralAccountDto(generalAccount.Id, generalAccount.Name, generalAccount.InitialAmmount, generalAccount.AccountCurrency,generalAccount.CreationDate));
        }

        return generalAccountsDto;
    }

    public GeneralAccountDto GetGeneralAccountById(int id)
    {
        GeneralAccount generalAccount = _accountsRepository.FindGeneralAccountById(id);

        return new GeneralAccountDto(generalAccount.Id, generalAccount.Name, generalAccount.InitialAmmount, generalAccount.AccountCurrency,generalAccount.CreationDate);
    }

    public void ChangeGeneralAccountName(GeneralAccount generalAccount, GeneralAccountDto generalAccountDto)
    {
        generalAccount.Name = generalAccountDto.Name;

        _accountsRepository.UpdateGeneralAccount(generalAccount);
    }
    
    public void UpdateGeneralAccount(GeneralAccountDto element, int userId)
    {
        GeneralAccount generalAccount = _accountsRepository.FindGeneralAccountById(element.Id);
        List<GeneralAccount> generalAccounts = _accountsRepository.GetGeneralAccounts(userId);
        foreach (var g in generalAccounts)
        {
            if(element.Name == g.Name)
            {
                throw new RepositoryExceptions("The account already exists");
            }
        }

        generalAccount.Name = element.Name;

        _accountsRepository.UpdateGeneralAccount(generalAccount);
    }

    public void DeleteGeneralAccount(int id, int userId)
    {
        GeneralAccount generalAccountToDelete = _accountsRepository.FindGeneralAccountById(id);
        List<Transaction> transactions= _transactionsRepository.GetTransactions(userId);
        foreach(var t in transactions)
        {
            if( t.AccountId == generalAccountToDelete.Id)
            {
                throw new RepositoryExceptions("The account has transactions associated");
            }
        }

        _accountsRepository.DeleteGeneralAccount(generalAccountToDelete.Name, userId);
    }
    
    public CreditCardAccount CreateCreditCardAccount(CreditCardAccountDto creditCardAccountDto)
    {
        CreditCardAccount creditCardAccount = _accountsRepository.FindCreditCardAccountById(creditCardAccountDto.Id);
        if (creditCardAccount != null)
        {
            return creditCardAccount;
        }
        return new CreditCardAccount(creditCardAccountDto.IssuingBank,creditCardAccountDto.LastFourDigits,creditCardAccountDto.AccountCurrency,
                                     creditCardAccountDto.AvailableBalance,creditCardAccountDto.CreationDate,creditCardAccountDto.ClosingDate);
    }

    public void AddCreditCardAccount(CreditCardAccountDto creditCardAccountDto, int userId)
    {
        CreditCardAccount newCreditCardAccount = CreateCreditCardAccount(creditCardAccountDto);

        _accountsRepository.AddCreditCardAccount(newCreditCardAccount, userId);
    }
    
    public List<CreditCardAccountDto> GetCreditCardAccounts(int userId)
    {
        List<CreditCardAccount> creditCardAccounts = _accountsRepository.GetCreditCardAccounts(userId);

        List<CreditCardAccountDto> creditCardAccountDtos = new List<CreditCardAccountDto>();

        foreach (var creditCardAccount in creditCardAccounts)
        {
            creditCardAccountDtos.Add(new CreditCardAccountDto(creditCardAccount.Id, creditCardAccount.IssuingBank, creditCardAccount.LastFourDigits, creditCardAccount.AccountCurrency,
                                                               creditCardAccount.AvailableBalance, creditCardAccount.CreationDate, creditCardAccount.ClosingDate));
        }

        return creditCardAccountDtos;
    }
    
    public CreditCardAccountDto GetCreditCardAccountById(int id)
    {
        CreditCardAccount creditCardAccount = _accountsRepository.FindCreditCardAccountById(id);

        return new CreditCardAccountDto(creditCardAccount.Id, creditCardAccount.IssuingBank, creditCardAccount.LastFourDigits, creditCardAccount.AccountCurrency,
                                        creditCardAccount.AvailableBalance, creditCardAccount.CreationDate, creditCardAccount.ClosingDate);
        
    }
    
    public void ChangeCreditCardAccountIssuingBank(CreditCardAccount creditCardAccount, CreditCardAccountDto creditCardAccountDto, int userId)
    {
        creditCardAccount.IssuingBank = creditCardAccountDto.IssuingBank;

        _accountsRepository.UpdateCreditCardAccount(creditCardAccount,userId);
    }
    
    public void ChangeCreditCardAccountLastFourDigits(CreditCardAccount creditCardAccount, CreditCardAccountDto creditCardAccountDto, int userId)
    {
        creditCardAccount.LastFourDigits = creditCardAccountDto.LastFourDigits;

        _accountsRepository.UpdateCreditCardAccount(creditCardAccount,userId);
    }
    
    public void ChangeCreditCardAccountCurrency(CreditCardAccount creditCardAccount, CreditCardAccountDto creditCardAccountDto, int userId)
    {
        creditCardAccount.AccountCurrency = creditCardAccountDto.AccountCurrency;

        _accountsRepository.UpdateCreditCardAccount(creditCardAccount,userId);
    }
    
    public void ChangeCreditCardAccountClosingDate(CreditCardAccount creditCardAccount, CreditCardAccountDto creditCardAccountDto, int userId)
    {
        creditCardAccount.ClosingDate = creditCardAccountDto.ClosingDate;

        _accountsRepository.UpdateCreditCardAccount(creditCardAccount,userId);
    }
    
    public void UpdateCreditCardAccount(CreditCardAccountDto element, int userId)
    {
        CreditCardAccount creditCardAccount = _accountsRepository.FindCreditCardAccountById(element.Id);
        List<CreditCardAccount> userCredits = _accountsRepository.GetCreditCardAccounts(userId);

        foreach (var c in userCredits)
        {
            if(c.IssuingBank == element.IssuingBank && c.LastFourDigits == element.LastFourDigits)
            {
                throw new RepositoryExceptions("The account already exists");
            }
        }

        creditCardAccount.IssuingBank = element.IssuingBank;
        creditCardAccount.LastFourDigits = element.LastFourDigits;
        creditCardAccount.AccountCurrency = element.AccountCurrency;
        creditCardAccount.ClosingDate = element.ClosingDate;

        _accountsRepository.UpdateCreditCardAccount(creditCardAccount, userId);
    }
    
    public void DeleteCreditCardAccount(int id, int userId)
    {
        CreditCardAccount creditCardAccountToDelete = _accountsRepository.FindCreditCardAccountById(id);
        List<Transaction> transactions = _transactionsRepository.GetTransactions(userId);
        foreach (var t in transactions)
        {
            if (t.AccountId == creditCardAccountToDelete.Id)
            {
                throw new RepositoryExceptions("The account has transactions associated");
            }
        }

        _accountsRepository.DeleteCreditCardAccount(creditCardAccountToDelete.Id, userId);
    }
    
    public List<AccountDto> GetAccounts(int userId)
    {
        List<Account> accounts = _accountsRepository.GetAccounts(userId);

        List<AccountDto> accountDtos = new List<AccountDto>();

        foreach (var account in accounts)
        {
            accountDtos.Add(new AccountDto(account.Id));
        }

        return accountDtos;
    }
    
    public AccountDto GetAccountById(int id)
    {
        Account account = _accountsRepository.FindAccountById(id);

        return new AccountDto(account.Id);
    }
    
    public Transaction CreateTransaction(TransactionDto transactionDto)
    {
        return new Transaction(transactionDto.Title,transactionDto.CreationDate,transactionDto.Amount,transactionDto.ExchangeRateAssociated,
                               transactionDto.CategoryAssociated,transactionDto.AccountAssociated);
    }

    public void AddTransaction(TransactionDto transactionDto,int userId, int accountId, int categoryId, int exchangeRateId)
    {
        Transaction newTransaction = CreateTransaction(transactionDto);

        _transactionsRepository.AddTransaction(newTransaction,userId,accountId,categoryId,exchangeRateId);
    }
    
    public List<TransactionDto> GetTransactions(int userId)
    {
        List<Transaction> transactions = _transactionsRepository.GetTransactions(userId);

        List<TransactionDto> transactionsDto = new List<TransactionDto>();

        foreach (var transaction in transactions)
        {
            Category category = _categoryRepository.FindCategoryById((int)transaction.CategoryId);
            Account account = _accountsRepository.FindAccountById((int)transaction.AccountId);
            ExchangeRate exchangeRate = _exchangeRatesRepository.FindExchangeRateById((int)transaction.ExchangeRateId);
            transactionsDto.Add(new TransactionDto(transaction.Id, transaction.Title, transaction.Date, transaction.Amount, exchangeRate,
                                                   category, transaction.AccountAssociated));
        }

        return transactionsDto;
    }
    
    public TransactionDto GetTransactionById(int id)
    {
        Transaction transaction = _transactionsRepository.FindTransactionById(id);

        return new TransactionDto(transaction.Id, transaction.Title, transaction.Date, transaction.Amount, transaction.ExchangeRateAssociated,
                                  transaction.CategoryAssociated, transaction.AccountAssociated);
        
    }
    
    public void ChangeTransactionCurrency(Transaction transaction, TransactionDto transactionDto)
    {
        transaction.ExchangeRateAssociated = transactionDto.ExchangeRateAssociated;

        _transactionsRepository.UpdateTransaction(transaction);
    }
    
    public void ChangeTransactionAmount(Transaction transaction, TransactionDto transactionDto)
    {
        transaction.Amount = transactionDto.Amount;

        _transactionsRepository.UpdateTransaction(transaction);
    }
    
    public void ChangeTransactionCategory(Transaction transaction, TransactionDto transactionDto)
    {
        transaction.CategoryAssociated = transactionDto.CategoryAssociated;

        _transactionsRepository.UpdateTransaction(transaction);
    }

    public void UpdateTransaction(TransactionDto transactionDto)
    {
        Transaction transaction = _transactionsRepository.FindTransactionById(transactionDto.Id);

        transaction.Amount = transactionDto.Amount;
    
        _transactionsRepository.UpdateTransaction(transaction);
    }
    public void UpdateTransactionCategory(TransactionDto transactionDto,CategoryDto categoryDto)
    {
        Transaction transaction = _transactionsRepository.FindTransactionById(transactionDto.Id);
        Category category = _categoryRepository.FindCategoryById(categoryDto.Id);
        transaction.CategoryAssociated = category;
        _transactionsRepository.UpdateTransaction(transaction);
    }
    public void UpdateTransactionExchange(TransactionDto transactionDto, ExchangeRateDto exchangeDto)
    {
        Transaction transaction = _transactionsRepository.FindTransactionById(transactionDto.Id);
        ExchangeRate rate = _exchangeRatesRepository.FindExchangeRateById(exchangeDto.Id);
        transaction.ExchangeRateAssociated = rate;
        _transactionsRepository.UpdateTransactionRate(transaction);
    }


    public ExchangeRate CreateExchangeRate(ExchangeRateDto exchangeRateDto, int userId)
    {
        ExchangeRate exchangeRate = _exchangeRatesRepository.FindExchangeRateById(exchangeRateDto.Id);
        if (exchangeRate != null)
        {
            return exchangeRate;
        }
        return new ExchangeRate(exchangeRateDto.Date,exchangeRateDto.Value,exchangeRateDto.RateCurrency);
    }

    public void AddExchangeRate(ExchangeRateDto exchangeRateDto, int userId)
    {
        ExchangeRate newExchangeRate = CreateExchangeRate(exchangeRateDto, userId);

        _exchangeRatesRepository.AddExchangeRate(newExchangeRate, userId);
    }

    public List<ExchangeRateDto> GetExchangeRates(int userId)
    {
        List<ExchangeRate> exchangeRates = _exchangeRatesRepository.GetExchangeRates(userId);

        List<ExchangeRateDto> exchangeRateDtos = new List<ExchangeRateDto>();

        foreach (var exchangeRate in exchangeRates)
        {
            exchangeRateDtos.Add(new ExchangeRateDto(exchangeRate.Id, exchangeRate.Date, exchangeRate.Value, exchangeRate.RateCurrency));
        }

        return exchangeRateDtos;
    }

    public void ChangeExchangeRateValue(ExchangeRate exchangeRate, ExchangeRateDto exchangeRateDto)
    {
        exchangeRate.Value = exchangeRateDto.Value;

        _exchangeRatesRepository.UpdateExchangeRate(exchangeRate);
    }

    public void UpdateExchangeRate(ExchangeRateDto exchangeRateDto)
    {
        ExchangeRate exchangeRate = _exchangeRatesRepository.FindExchangeRateById(exchangeRateDto.Id);
    
        exchangeRate.Value = exchangeRateDto.Value;

        _exchangeRatesRepository.UpdateExchangeRate(exchangeRate);
    }

    public ExchangeRateDto GetExchangeRateById(int id)
    {
        ExchangeRate exchangeRate = _exchangeRatesRepository.FindExchangeRateById(id);

        return new ExchangeRateDto(exchangeRate.Id, exchangeRate.Date, exchangeRate.Value, exchangeRate.RateCurrency);
    }

    public void DeleteExchangeRate(ExchangeRateDto exchangeRateDto, int id)
    {
        ExchangeRate exchangeRateToDelete = _exchangeRatesRepository.FindExchangeRateById(id);

        _exchangeRatesRepository.DeleteExchangeRate(exchangeRateToDelete.Id);
    }
    
    public SpendingGoal CreateSpendingGoal(SpendingGoalDto spendingGoalDto)
    {
        SpendingGoal spendingGoal = _spendingGoalsRepository.FindSpendingGoalById(spendingGoalDto.Id);
        if (spendingGoal != null)
        {
            return spendingGoal;
        }
        
        return new SpendingGoal(spendingGoalDto.Title,spendingGoalDto.MaxAmount,spendingGoalDto.GoalCurrency,spendingGoalDto.Categories);
    }
    
    public void AddSpendingGoal(SpendingGoalDto spendingGoalDto, int userId)
    {
        SpendingGoal newSpendingGoal = CreateSpendingGoal(spendingGoalDto);

        _spendingGoalsRepository.AddSpendingGoal(newSpendingGoal, userId);
    }

    public SpendingGoalDto GetSpendingGoalById(int id)
    {
        SpendingGoal spendingGoal = _spendingGoalsRepository.FindSpendingGoalById(id);

        return new SpendingGoalDto(spendingGoal.Id, spendingGoal.Title, spendingGoal.MaximumAmount, spendingGoal.GoalCurrency,spendingGoal.Categories);
    }
    
    public List<SpendingGoalDto> GetSpendingGoals(int userId)
    {
        List<SpendingGoal> spendingGoals = _spendingGoalsRepository.GetSpendingGoals(userId);

        List<SpendingGoalDto> spendingGoalDtos = new List<SpendingGoalDto>();

        foreach (var spendingGoal in spendingGoals)
        {
            spendingGoalDtos.Add(new SpendingGoalDto(spendingGoal.Id, spendingGoal.Title, spendingGoal.MaximumAmount, spendingGoal.GoalCurrency,spendingGoal.Categories));
        }

        return spendingGoalDtos;
    }
    
    public void ChangeSpendingGoalMaxAmount(SpendingGoal spendingGoal, SpendingGoalDto spendingGoalDto ,int userId)
    {
        spendingGoal.MaximumAmount = spendingGoalDto.MaxAmount;

        _spendingGoalsRepository.UpdateSpendingGoal(spendingGoal, userId);
    }
    
    public void ChangeSpendingGoalCategories(SpendingGoal spendingGoal, SpendingGoalDto spendingGoalDto, int userId)
    {
        spendingGoal.Categories = spendingGoalDto.Categories;

        _spendingGoalsRepository.UpdateSpendingGoal(spendingGoal,userId);
    }
    
    public void UpdateSpendingGoal(SpendingGoalDto spendingGoalDto, int userId)
    {
        SpendingGoal spendingGoal = _spendingGoalsRepository.FindSpendingGoalById(spendingGoalDto.Id);
    
        spendingGoal.MaximumAmount = spendingGoalDto.MaxAmount;
        spendingGoal.Categories = spendingGoalDto.Categories;

        _spendingGoalsRepository.UpdateSpendingGoal(spendingGoal, userId);
    }
}