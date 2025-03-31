using DataAccess.Context;
using DataAccess.Repository;
using Logic.DTOs;
using Models;
using Logic.Interfaces;
using Models.Enums;

namespace Logic
{
    public class ReportController : IReportByCategory, IReportGoalByMonth, IReportExpenses, IReportIncomeOutcome, IReportCreditCardExpenses,
                                    IReportGeneralAccountBalance
    {
        public UsersRepository _userRepository;
        public CategoryRepository _categoryRepository;
        public AccountsRepository _accountsRepository;
        public TransactionsRepository _transactionsRepository;
        public ExchangeRatesRepository _exchangeRatesRepository;
        public SpendingGoalsRepository _spendingGoalsRepository;

        public ReportController(ApplicationDbContext context)
        {
            _userRepository = new UsersRepository(context);
            _categoryRepository = new CategoryRepository(context);
            _accountsRepository = new AccountsRepository(context);
            _transactionsRepository = new TransactionsRepository(context);
            _exchangeRatesRepository = new ExchangeRatesRepository(context);
            _spendingGoalsRepository = new SpendingGoalsRepository(context);
        }

        public List<ReportGoalByCategoryDto> GenerateReportByCategories(int userId, DateTime month)
        {
            List<Category> categories = _categoryRepository.GetCategories(userId);
            List<Transaction> transactions = _transactionsRepository.GetTransactions(userId);
            List<ExchangeRate> exchangeRates = _exchangeRatesRepository.GetExchangeRates(userId);
            List<ReportGoalByCategoryDto> report = new List<ReportGoalByCategoryDto>();
            int totalSpentInMonth = _transactionsRepository.GetTotalSpentInMonth(userId, month);

            foreach (var category in categories)
            {
                double totalSpentByCategory = 0;
                foreach (var transaction in transactions)
                {
                    if (category.TypeOf == CategoryType.ExpensesCategory)
                    {
                        if(transaction.CategoryAssociated.Id == category.Id && transaction.Date.Month == month.Month && transaction.Date.Year == month.Year)
                        {
                            foreach (var exchangeRate in exchangeRates)
                            {
                                if(transaction.ExchangeRateAssociated.Id == exchangeRate.Id)
                                {
                                    totalSpentByCategory += transaction.Amount * exchangeRate.Value;
                                }
                            }
                        }
                    }
                }

                if (category.TypeOf == CategoryType.ExpensesCategory)
                {
                    int percentageSpent = (int)((totalSpentByCategory / totalSpentInMonth) * 100);
                    report.Add(new ReportGoalByCategoryDto(category, totalSpentByCategory, percentageSpent));
                }
            }

            return report;
        }
        
        public List<ReportGeneralAccountBalanceDto> GenerateReportGeneralAccountBalance(GeneralAccountDto generalAccountDto, int userId)
        {
            try
            {
                List<Transaction> transactions = _transactionsRepository.GetTransactions(userId);
                List<ReportGeneralAccountBalanceDto> report = new List<ReportGeneralAccountBalanceDto>();

                double income = 0;
                double expense = 0;
                foreach (var transaction in transactions)
                {
                    if (transaction.AccountAssociated.Id == generalAccountDto.Id)
                    {
                        if (transaction.CategoryAssociated.TypeOf == CategoryType.ExpensesCategory)
                        {
                            expense += transaction.Amount;
                        }
                        else if (transaction.CategoryAssociated.TypeOf == CategoryType.IncomeCategory)
                        {
                            income += transaction.Amount;
                        }
                    }
                }

                double balance = generalAccountDto.InitialAmmount + income - expense;
                report.Add(new ReportGeneralAccountBalanceDto(generalAccountDto.Name, balance,
                    generalAccountDto.AccountCurrency));

                return report;
            }
            catch (Exception e)
            {
                throw new Exception("Please Select a General Account");
            }
        }

        public List<ReportCreditCardExpensesDto> GenerateReportCreditCardExpenses(CreditCardAccountDto creditCardAccountDto, int userId, DateTime date)
        {
            try
            {
                CreditCardAccount creditCardAccount =
                    _accountsRepository.FindCreditCardAccountById(creditCardAccountDto.Id);
                List<Transaction> transactions = _transactionsRepository.GetTransactions(userId);
                double totalSpent = 0;

                List<ReportCreditCardExpensesDto> report = new List<ReportCreditCardExpensesDto>();

                foreach (var transaction in transactions)
                {

                    if (transaction.AccountAssociated.Id == creditCardAccount.Id &&
                        transaction.CategoryAssociated.TypeOf == CategoryType.ExpensesCategory &&
                        DateInRange(creditCardAccount.ClosingDate, transaction.Date))
                    {


                        totalSpent += transaction.Amount * transaction.ExchangeRateAssociated.Value;
                    }
                }

                report.Add(new ReportCreditCardExpensesDto(creditCardAccount, totalSpent));

                return report;
            }
            catch (Exception e)
            {
                throw new Exception("Please Select a CreditCard Account");
            }
        }

        private bool DateInRange(DateTime fechaCierre, DateTime fecha2)
        {
            DateTime fechaMesActualQueCierra = new DateTime(fecha2.Year, fecha2.Month, fechaCierre.Day);
            DateTime inicioRango = fechaMesActualQueCierra.AddDays(1).AddMonths(-1);
            return fecha2 >= inicioRango && fecha2 < fechaMesActualQueCierra;
        }
        
        public List<ReportGoalByMonthDto> GenerateReportGoalsByMonth(int userId, DateTime date)
        {
            List<SpendingGoal> spendingGoals = _spendingGoalsRepository.GetSpendingGoals(userId);
            List<Transaction> transactions = _transactionsRepository.GetTransactions(userId);

            List<ReportGoalByMonthDto> report = new List<ReportGoalByMonthDto>();
            foreach(var goal in spendingGoals)
            {

                double totalSpent = 0;
                foreach (var cat in  goal.Categories) 
                {
                    foreach (var transaction in transactions)
                    {
                        if (cat.TypeOf == CategoryType.ExpensesCategory)
                        {
                            if(transaction.CategoryAssociated.Id == cat.Id && transaction.Date.Day<= date.Day && transaction.Date.Month == date.Month && transaction.Date.Year == date.Year)
                            {
                                 totalSpent += transaction.Amount;
                            }
                        }
                    }       
                }
                ReportGoalByMonthDto reportGoal = new ReportGoalByMonthDto(goal, totalSpent, totalSpent <= goal.MaximumAmount, date);
                report.Add(reportGoal);
            }
            return report;
        }
        public List<TransactionDto> GetTransactionsFiltered(DateTime inital, DateTime final, CategoryDto category,AccountDto accountDto,int userId)
        {
            try
            {
                Category cat = _categoryRepository.FindCategoryById(category.Id);
                Account acc = _accountsRepository.FindAccountById(accountDto.Id);

                List<Transaction> transactionsList = _transactionsRepository.GetTransactions(userId);
                List<TransactionDto> transactions = new List<TransactionDto>();
                foreach (var transaction in transactionsList)
                {
                    if (transaction.Date >= inital && transaction.Date <= final &&
                        transaction.CategoryAssociated.Id == cat.Id && transaction.AccountAssociated.Id == acc.Id && transaction.CategoryAssociated.TypeOf == CategoryType.ExpensesCategory)
                    {
                        ExchangeRate rate =
                            _exchangeRatesRepository.FindExchangeRateById((int)transaction.ExchangeRateId);
                        Account account = _accountsRepository.FindAccountById((int)transaction.AccountId);
                        transactions.Add(new TransactionDto(transaction.Id, transaction.Title, transaction.Date,
                            transaction.Amount, rate, transaction.CategoryAssociated, account));
                    }
                }

                return transactions;
            }
            catch (Exception e)
            {
                throw new Exception("Please Select all Fields");
            }
        }

        public List<ReportIncomeOutcomeDto> GetIncomeOutcome(DateTime date, int userId)
        {
            List<Transaction> transactions = _transactionsRepository.GetTransactions(userId);
            List<ReportIncomeOutcomeDto> rep = new List<ReportIncomeOutcomeDto>();
            for(int i = 1; i<=31; i++)
            {
                rep.Add(new ReportIncomeOutcomeDto(i, 0, 0));
            }

            foreach (var t in transactions)
            {
                foreach(var r in rep)
                {
                    if(t.Date.Month == date.Month && t.Date.Year == date.Year)
                    {
                        if (t.CategoryAssociated.TypeOf == CategoryType.IncomeCategory)
                        {
                            if (t.Date.Day == r.Day)
                            {
                                r.Income += t.Amount * t.ExchangeRateAssociated.Value;
                            }
                        }
                        else if (t.CategoryAssociated.TypeOf == CategoryType.ExpensesCategory)
                        {
                            if (t.Date.Day == r.Day)
                            {
                                r.Outcome += t.Amount * t.ExchangeRateAssociated.Value;
                            }
                        }
                    }
                }
            }
            return rep;
        }


    }
}
