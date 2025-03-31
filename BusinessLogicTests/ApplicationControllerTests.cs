using DataAccess.Context;
using Logic;
using Logic.DTOs;
using Models;
using Models.Enums;
using Models.Exceptions;

namespace DataAccessTests
{
    [TestClass]
    public class ApplicationControllerTests
    {
        private ApplicationDbContext _context;
        private ApplicationController _controller;
        private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();
    
        [TestInitialize]
        public void SetUp()
        {
            _context = _contextFactory.CreateDbContext();
            _controller = new ApplicationController(_context);
        }
    
        [TestCleanup]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
        
        [TestMethod]
        public void WhenControllerReceivesAnEmptyUserDto_ShouldMapTheDtoIntoAnEmptyUserObject()
        {
            UserDto userDto = new UserDto();
            userDto.Email = "germanramos@gmail.com";
            userDto.Name = "German";
            userDto.Surname = "Ramos";
            userDto.Password = "German123456";
            
            User user = _controller.CreateUser(userDto);
            
            Assert.IsInstanceOfType(user, typeof(User));
        }
        
        [TestMethod]
        public void WhenControllerReceivesAUserDto_ShouldMapTheDtoIntoAUserObject()
        {
            UserDto userDto = new UserDto("german@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            
            User user = _controller.CreateUser(userDto);
            
            Assert.IsInstanceOfType(user, typeof(User));
        }
        
        [TestMethod]
        public void WhenControllerCreatesAUserWithAUserDto_ShouldCreateANewUserObject()
        {
            UserDto userDto = new UserDto("german@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            User expectedUser = new User("german@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            
            User user = _controller.CreateUser(userDto);
            
            Assert.AreEqual(user.Email,expectedUser.Email);
            Assert.AreEqual(user.Name,expectedUser.Name);
            Assert.AreEqual(user.Surname,expectedUser.Surname);
            Assert.AreEqual(user.Password,expectedUser.Password);
            Assert.AreEqual(user.AddressDirection,expectedUser.AddressDirection);
        }
        
        [TestMethod]
        public void WhenControllerAddANewUser_ShouldAddTheUserToTheUsersTable()
        {
            UserDto userDto = new UserDto("german@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            
            User user = _controller.CreateUser(userDto);
            
            _controller.AddUser(userDto);
            
            Assert.IsTrue(_controller._userRepository.UserAlreadyExists(user));
        }

        [TestMethod]
        public void WhenControllerChecksTheLoginForAExistingUser_ShouldReturnTrue()
        {
            UserDto userDto = new UserDto("german@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            
            User user = _controller.CreateUser(userDto);
            
            _controller.AddUser(userDto);

            Assert.IsTrue(_controller.CheckUserLogin(user.Email, user.Password));
        }
        
        [TestMethod]
        [ExpectedException(typeof(UserExceptions))]
        public void WhenControllerChecksTheLoginForANonExistingUser_ShouldAUserException()
        {
            UserDto userDto = new UserDto("german@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            
            User user = _controller.CreateUser(userDto);

            Assert.IsFalse(_controller.CheckUserLogin(user.Email, user.Password));
        }
        
        [TestMethod]
        public void WhenControllerFindsAUserByEmail_ShouldReturnTheCorrectUserForTheAssociatedEmail()
        {
            UserDto userDto = new UserDto("german@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            
            User user = _controller.CreateUser(userDto);
            
            _controller.AddUser(userDto);

            User expectedUser = _controller.FindUserByEmail(user.Email);

            Assert.AreEqual(user,expectedUser);
        }
        
        [TestMethod]
        public void WhenControllerModifiesTheUserName_ShouldReturnTheNewNameAssociated()
        {
            UserDto userDto = new UserDto("german@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            
            User user = _controller.CreateUser(userDto);
            
            _controller.AddUser(userDto);

            userDto.Name = "Santiago";
            
            _controller.ChangeUserName(user,userDto);
            
            Assert.AreEqual("Santiago", user.Name);
        }
        
        [TestMethod]
        public void WhenControllerModifiesTheUserSurname_ShouldReturnTheNewSurnameAssociated()
        {
            UserDto userDto = new UserDto("german@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            
            User user = _controller.CreateUser(userDto);
            
            _controller.AddUser(userDto);

            userDto.Surname = "Lopez";
            
            _controller.ChangeUserSurname(user,userDto);
            
            Assert.AreEqual("Lopez", user.Surname);
        }
        
        [TestMethod]
        public void WhenControllerModifiesTheUserAddress_ShouldReturnTheNewAddressAssociated()
        {
            UserDto userDto = new UserDto("german@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            
            User user = _controller.CreateUser(userDto);
            
            _controller.AddUser(userDto);

            userDto.Address = "Avenida 1234";
            
            _controller.ChangeUserAddress(user,userDto);
            
            Assert.AreEqual("Avenida 1234", user.AddressDirection);
        }
        
        [TestMethod]
        public void WhenControllerModifiesTheUserPassword_ShouldReturnTheNewPasswordAssociated()
        {
            UserDto userDto = new UserDto("german@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            
            User user = _controller.CreateUser(userDto);
            
            _controller.AddUser(userDto);

            userDto.Password = "Santiago123456";
            
            _controller.ChangeUserPassword(user,userDto);
            
            Assert.AreEqual("Santiago123456", user.Password);
        }
        
        [TestMethod]
        public void WhenControllerReceivesAnEmptyCategoryAccountDto_ShouldMapTheDtoIntoAnEmptyCategoryObject()
        {
            CategoryDto categoryDto = new CategoryDto();
            categoryDto.Name = "Cine";
            
            Category category = _controller.CreateCategory(categoryDto,1);
            
            Assert.IsInstanceOfType(category, typeof(Category));
        }

        [TestMethod]
        public void WhenControllerReceivesACategoryDto_ShouldMapTheDtoIntoACategoryObject()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            CategoryDto categoryDto = new CategoryDto(1,"Cine",DateTime.Now, CategoryType.ExpensesCategory, StatusType.Active);

            Category category = _controller.CreateCategory(categoryDto,1);

            Assert.IsInstanceOfType(category, typeof(Category));
        }

        [TestMethod]
        public void WhenControllerCreatesACategoryWithACategoryDto_ShouldCreateANewCategoryObject()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            DateTime dateTime = DateTime.Now;
            CategoryDto categoryDto = new CategoryDto(1,"Cine",dateTime, CategoryType.ExpensesCategory, StatusType.Active);
            Category expectedCategory = new Category("Cine",dateTime, CategoryType.ExpensesCategory, StatusType.Active);

            Category category = _controller.CreateCategory(categoryDto,1);
            
            Assert.AreEqual(category.Name,expectedCategory.Name);
            Assert.AreEqual(category.CreationDate,expectedCategory.CreationDate);
            Assert.AreEqual(category.TypeOf,expectedCategory.TypeOf);
            Assert.AreEqual(category.Status,expectedCategory.Status);
            
        }

        [TestMethod]
        public void WhenControllerTriesToCreateACategoryThatAlreadyExists_ShouldReturnTheExistingCategory()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            DateTime dateTime = DateTime.Now;
            CategoryDto categoryDto = new CategoryDto(1,"Cine",dateTime, CategoryType.ExpensesCategory, StatusType.Active);
            
            Category category = _controller.CreateCategory(categoryDto,1);
            _controller.AddCategory(categoryDto, 1);
            
            Category expectedCategory = _controller.CreateCategory(categoryDto,1);
            
            Assert.AreEqual(category.Name, expectedCategory.Name);
            
        }

        [TestMethod]
        public void WhenControllerAddANewCategory_ShouldAddTheCategoryToTheCategoryTable()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            CategoryDto categoryDto = new CategoryDto(1,"Cine",DateTime.Now, CategoryType.ExpensesCategory, StatusType.Active);

            Category category = _controller.CreateCategory(categoryDto,1);
            _controller.AddCategory(categoryDto, 1);

            Assert.IsTrue(_controller._categoryRepository.CategoryAlreadyExists(category, 1));
        }

        [TestMethod]
        public void WhenControllerFindsACategoryByName_ShouldReturnTheCorrectCategoryAssociatedName()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            CategoryDto categoryDto = new CategoryDto(1,"Cine",DateTime.Now, CategoryType.ExpensesCategory, StatusType.Active);

            Category category = _controller.CreateCategory(categoryDto,1);

            _controller.AddCategory(categoryDto,1);

            Category expectedCategory = _controller.FindCategoryByName(category.Name, 1);

            Assert.AreEqual(category.Name, expectedCategory.Name);
        }

        [TestMethod]
        public void WhenControllerModifiesTheCategoryName_ShouldReturnTheNewNameAssociated()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            CategoryDto categoryDto = new CategoryDto(1,"Cine", DateTime.Now, CategoryType.ExpensesCategory, StatusType.Active);

            Category category = _controller.CreateCategory(categoryDto,1); 

            _controller.AddCategory(categoryDto,1);

            categoryDto.Name = "Cine";

            _controller.ChangeCategoryName(category, categoryDto);

            Assert.AreEqual("Cine", category.Name);
        }

        [TestMethod]
        public void WhenControllerModifiesTheCategoryType_ShouldReturnTheNewTypeAssociated()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            CategoryDto categoryDto = new CategoryDto(1,"Cine", DateTime.Now, CategoryType.ExpensesCategory, StatusType.Active);
        
            Category category = _controller.CreateCategory(categoryDto,1);

            _controller.AddCategory(categoryDto,1);

            categoryDto.TypeOf = CategoryType.IncomeCategory;

            _controller.ChangeCategoryType(category, categoryDto);

            Assert.AreEqual(CategoryType.IncomeCategory, category.TypeOf); 
        
        }

        [TestMethod]
        public void WhenControllerModifiesTheCategoryStatus_ShouldReturnTheNewStatusAssociated()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            CategoryDto categoryDto = new CategoryDto(1,"Cine", DateTime.Now, CategoryType.ExpensesCategory, StatusType.Active);
        
            Category category = _controller.CreateCategory(categoryDto,1);

            _controller.AddCategory(categoryDto,1);

            categoryDto.Status = StatusType.Inactive;

            _controller.ChangeCategoryStatus(category, categoryDto);

            Assert.AreEqual(StatusType.Inactive, category.Status); 
        
        }

        [TestMethod]
        public void WhenControllerModifiesTheCategoryDate_ShouldReturnTheNewDateAssociated()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            DateTime dateTime = DateTime.Now.AddDays(-1);
            DateTime dateTime2 = DateTime.Now;
            CategoryDto categoryDto = new CategoryDto(1,"Cine", dateTime, CategoryType.ExpensesCategory, StatusType.Active);
        
            Category category = _controller.CreateCategory(categoryDto,1);

            _controller.AddCategory(categoryDto,1);

            categoryDto.CreationDate = dateTime2;

            _controller.ChangeCategoryCreationDate(category, categoryDto);

            Assert.AreEqual(dateTime2, category.CreationDate); 
        
        }
        
        [TestMethod]
        public void WhenControllerListsAllTheCategories_ShouldReturnACategoryDtoList()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            CategoryDto categoryDto = new CategoryDto(1,"Cine", DateTime.Now, CategoryType.ExpensesCategory, StatusType.Active);
        
            Category category = _controller.CreateCategory(categoryDto,1);

            _controller.AddCategory(categoryDto,1);
            
            List<CategoryDto> expectedCategories = _controller.GetAllCategories(1);
            
            int expectedCategoriesCount = expectedCategories.Count;

            Assert.AreEqual(1, expectedCategoriesCount);

        }
         [TestMethod]
        public void WhenControllerListsAllTheCategories_ShouldReturnACategoryDtoListWithOnlyActivesAndExpenses()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;

            CategoryDto categoryDto = new CategoryDto(1, "Cine", DateTime.Now, CategoryType.ExpensesCategory, StatusType.Active);

            Category category = _controller.CreateCategory(categoryDto, 1);

            _controller.AddCategory(categoryDto, 1);

            List<CategoryDto> expectedCategories = _controller.GetAllCategoriesForSpending(1);

            int expectedCategoriesCount = expectedCategories.Count;

            Assert.AreEqual(1, expectedCategoriesCount);

        }

        [TestMethod]
        public void WhenControllerDeletesACategory_ShouldDeleteTheCategoryFromTheCategoriesTable()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            CategoryDto categoryDto = new CategoryDto(1,"Cine", DateTime.Now, CategoryType.ExpensesCategory, StatusType.Active);
        
            Category category = _controller.CreateCategory(categoryDto,1);

            _controller.AddCategory(categoryDto,1);
            
            _controller.DeleteCategory(categoryDto, 1);
            
            Assert.IsFalse(_controller._categoryRepository.CategoryAlreadyExists(category,1));
        }
        
        [TestMethod]
        public void WhenControllerGetsACategoryById_ShouldReturnACategoryDtoWithTheCategoryData()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");

            CategoryDto categoryDto = new CategoryDto(1,"Cine", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
        
            Category category = _controller.CreateCategory(categoryDto,1);

            _controller.AddCategory(categoryDto,1);
            
            CategoryDto expectedCategoryDto = _controller.GetCategoryById(categoryDto.Id);
            
            Assert.AreEqual(categoryDto.Name, expectedCategoryDto.Name);
        }
        
        [TestMethod]
        public void WhenControllerUpdatesACategory_ShouldUpdateTheCategoryInTheDatabase()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;

            CategoryDto categoryDto = new CategoryDto(1,"Cine", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Inactive);

            _controller.AddCategory(categoryDto,1);
            
            categoryDto.Name = "Salidas";
            categoryDto.Status = StatusType.Active;
            
            _controller.UpdateCategory(categoryDto,user.Id);
            
            CategoryDto expectedCategoryDto = _controller.GetCategoryById(categoryDto.Id);
            
            Assert.AreEqual("Salidas", expectedCategoryDto.Name);
            Assert.AreEqual(StatusType.Active, expectedCategoryDto.Status);
        }
        
        [TestMethod]
        public void WhenControllerReceivesAnEmptyAccountDto_ShouldMapTheDtoIntoAnEmptyAccountObject()
        {
            AccountDto accountDto = new AccountDto();

            Account account = new Account();
            
            Assert.IsInstanceOfType(account, typeof(Account));
        }
        
        [TestMethod]
        public void WhenControllerReceivesAnAccountDto_ShouldMapTheDtoIntoAnAccountObject()
        {
            AccountDto accountDto = new AccountDto(1);
            
            Account account = new Account();
            
            Assert.IsInstanceOfType(account, typeof(Account));
        }
        
        [TestMethod]
        public void WhenControllerReceivesAnEmptyGeneralAccountDto_ShouldMapTheDtoIntoAnEmptyGeneralAccountObject()
        {
            GeneralAccountDto categoryDto = new GeneralAccountDto();
            categoryDto.Name = "Banco Santander";
            
            GeneralAccount generalAccount = _controller.CreateGeneralAccount(categoryDto,1);
            
            Assert.IsInstanceOfType(generalAccount, typeof(GeneralAccount));
        }
        
        [TestMethod]
        public void WhenControllerReceivesAGeneralAccountDto_ShouldMapTheDtoIntoAGeneralAccountObject()
        {
            Currency generalAccountCurrency = Currency.UruguayanPeso;
            DateTime creationDate = DateTime.Today;
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;

            GeneralAccountDto generalAccountDto = new GeneralAccountDto(1,"Banco Santander",25000,generalAccountCurrency,creationDate);
            
            GeneralAccount generalAccount = _controller.CreateGeneralAccount(generalAccountDto,user.Id);
            
            Assert.IsInstanceOfType(generalAccount, typeof(GeneralAccount));
        }
        
        [TestMethod]
        public void WhenControllerCreatesAGeneralAccountWithAGeneralAccountDto_ShouldCreateANewGeneralAccountObject()
        {
            Currency generalAccountCurrency = Currency.UruguayanPeso;
            DateTime creationDate = DateTime.Today;
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;

            GeneralAccountDto generalAccountDto = new GeneralAccountDto(1,"Banco Santander",25000,generalAccountCurrency,creationDate);
            GeneralAccount expectedGeneralAccount = new GeneralAccount("Banco Santander",generalAccountCurrency,creationDate,25000);
            
            GeneralAccount generalAccount = _controller.CreateGeneralAccount(generalAccountDto, user.Id);
            
            Assert.AreEqual(generalAccount.Name,expectedGeneralAccount.Name);
            Assert.AreEqual(generalAccount.AccountCurrency,expectedGeneralAccount.AccountCurrency);
            Assert.AreEqual(generalAccount.CreationDate,expectedGeneralAccount.CreationDate);
            Assert.AreEqual(generalAccount.InitialAmmount,expectedGeneralAccount.InitialAmmount);
        }
        
        [TestMethod]
        public void WhenControllerTriesToCreateAGeneralAccountThatAlreadyExists_ShouldReturnTheExistingGeneralAccount()
        {
            Currency generalAccountCurrency = Currency.UruguayanPeso;
            DateTime creationDate = DateTime.Today;
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;

            GeneralAccountDto generalAccountDto = new GeneralAccountDto(1,"Banco Santander",25000,generalAccountCurrency,creationDate);
            
            GeneralAccount generalAccount = _controller.CreateGeneralAccount(generalAccountDto, user.Id);
            _controller.AddGeneralAccount(generalAccountDto, 1);
            
            GeneralAccount expectedGeneralAccount = _controller.CreateGeneralAccount(generalAccountDto, user.Id);
            
            Assert.AreEqual(generalAccount.Name, expectedGeneralAccount.Name);
        }
        
        [TestMethod]
        public void WhenControllerAddsANewGeneralAccount_ShouldAddTheGeneralAccountToTheAccountsTable()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;

            Currency generalAccountCurrency = Currency.UruguayanPeso;
            DateTime creationDate = DateTime.Today;
            
            GeneralAccountDto generalAccountDto = new GeneralAccountDto(1,"Banco Santander",25000,generalAccountCurrency,creationDate);

            GeneralAccount generalAccount = _controller.CreateGeneralAccount(generalAccountDto, user.Id);
            
            _controller.AddGeneralAccount(generalAccountDto, 1);
            
            Assert.IsTrue(_controller._accountsRepository.GeneralAccountAlreadyExists(generalAccount, 1));
        }
        
        [TestMethod]
        public void WhenControllerListsAllTheGeneralAccounts_ShouldReturnAGeneralAccountDtoList()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            Currency generalAccountCurrency = Currency.UruguayanPeso;
            DateTime creationDate = DateTime.Today;
            
            GeneralAccountDto generalAccountDto = new GeneralAccountDto(1,"Banco Santander",25000,generalAccountCurrency,creationDate);

            GeneralAccount generalAccount = _controller.CreateGeneralAccount(generalAccountDto, user.Id);
            
            _controller.AddGeneralAccount(generalAccountDto, 1);

            List<GeneralAccountDto> expectedGeneralAccounts = _controller.GetGeneralAccounts(1);
            
            int expectedGeneralAccountsCount = expectedGeneralAccounts.Count;

            Assert.AreEqual(1, expectedGeneralAccountsCount);
        }
        
        [TestMethod]
        public void WhenControllerGetsAGeneralAccountById_ShouldReturnAGeneralAccountDtoWithTheGeneralAccountData()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;

            GeneralAccountDto generalAccountDto = new GeneralAccountDto(1,"Banco Santander",25000,Currency.UruguayanPeso,DateTime.Today);

            _controller.AddGeneralAccount(generalAccountDto, 1);

            GeneralAccountDto expectedGeneralAccountDto = _controller.GetGeneralAccountById(1);
            
            Assert.AreEqual(generalAccountDto.Name, expectedGeneralAccountDto.Name);
        }
        
        [TestMethod]
        public void WhenControllerModifiesTheGeneralAccountName_ShouldReturnTheNewNameAssociated()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            GeneralAccountDto generalAccountDto = new GeneralAccountDto(1,"Banco Santander",25000,Currency.UruguayanPeso,DateTime.Today);

            GeneralAccount generalAccount = _controller.CreateGeneralAccount(generalAccountDto, user.Id);

            _controller.AddGeneralAccount(generalAccountDto, 1);

            generalAccountDto.Name = "Itau";

            _controller.ChangeGeneralAccountName(generalAccount, generalAccountDto);

            Assert.AreEqual("Itau", generalAccount.Name);
        }
        
        [TestMethod]
        public void WhenControllerUpdatesAGeneralAccount_ShouldReturnTheUpdatedGeneralAccount()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            GeneralAccountDto generalAccountDto = new GeneralAccountDto(1,"Banco Santander",25000,Currency.UruguayanPeso,DateTime.Today);

            _controller.AddGeneralAccount(generalAccountDto, 1);

            generalAccountDto.Name = "Itau";

            _controller.UpdateGeneralAccount(generalAccountDto, user.Id);
            
            GeneralAccountDto generalAccount = _controller.GetGeneralAccountById(1);

            Assert.AreEqual("Itau", generalAccount.Name);
        }
        
        [TestMethod]
        public void WhenControllerDeletesAGeneralAccount_ShouldDeleteTheGeneralAccountOfTheAccountsTable()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;

            Currency generalAccountCurrency = Currency.UruguayanPeso;
            DateTime creationDate = DateTime.Today;
            
            GeneralAccountDto generalAccountDto = new GeneralAccountDto(1,"Banco Santander",25000,generalAccountCurrency,creationDate);

            GeneralAccount generalAccount = _controller.CreateGeneralAccount(generalAccountDto, user.Id);
            
            _controller.AddGeneralAccount(generalAccountDto, 1);
            
            _controller.DeleteGeneralAccount(1, 1);
            
            Assert.IsFalse(_controller._accountsRepository.GeneralAccountAlreadyExists(generalAccount, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(AccountExceptions))]
        public void WhenControllerReceivesAnEmptyCreditCardAccountDtoWithoutFourLastDigits_ShouldThrowAnException()
        {
            CreditCardAccountDto creditCardAccountDto = new CreditCardAccountDto();
            creditCardAccountDto.IssuingBank = "Santander";
            
            CreditCardAccount creditCardAccount = _controller.CreateCreditCardAccount(creditCardAccountDto);
        }
        
        [TestMethod]
        public void WhenControllerReceivesACreditCardAccountDto_ShouldMapTheDtoIntoACreditCardAccountObject()
        {
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            DateTime creationDate = DateTime.Today;
            DateTime closingDate = new DateTime(2024, 10, 10);
            
            CreditCardAccountDto creditCardAccountDto = new CreditCardAccountDto(1,"Banco Santander","1234",creditCardAccountCurrency,25000,creationDate,closingDate);
            
            Assert.IsInstanceOfType(_controller.CreateCreditCardAccount(creditCardAccountDto), typeof(CreditCardAccount));
        }
        
        [TestMethod]
        public void WhenControllerCreatesACreditCardAccountWithACreditCardAccountDto_ShouldCreateANewCreditCardAccountObject()
        {
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            DateTime creationDate = DateTime.Today;
            DateTime closingDate = new DateTime(2024, 10, 10);
            
            CreditCardAccountDto creditCardAccountDto = new CreditCardAccountDto(1,"Santander","1234",creditCardAccountCurrency,25000,creationDate,closingDate);
            CreditCardAccount expectedCreditCardAccount = new CreditCardAccount("Santander","1234",creditCardAccountCurrency,25000,creationDate,closingDate);

            CreditCardAccount creditCardAccount = _controller.CreateCreditCardAccount(creditCardAccountDto);
            
            Assert.AreEqual(creditCardAccount.IssuingBank,expectedCreditCardAccount.IssuingBank);
            Assert.AreEqual(creditCardAccount.LastFourDigits,expectedCreditCardAccount.LastFourDigits);
            Assert.AreEqual(creditCardAccount.AccountCurrency,expectedCreditCardAccount.AccountCurrency);
            Assert.AreEqual(creditCardAccount.AvailableBalance,expectedCreditCardAccount.AvailableBalance);
            Assert.AreEqual(creditCardAccount.CreationDate,expectedCreditCardAccount.CreationDate);
            Assert.AreEqual(creditCardAccount.ClosingDate,expectedCreditCardAccount.ClosingDate);
        }
        
        [TestMethod]
        public void WhenControllerTriesToCreateACreditCardAccountThatAlreadyExists_ShouldReturnTheExistingCreditCardAccount()
        {
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            DateTime creationDate = DateTime.Today;
            DateTime closingDate = new DateTime(2024, 10, 10);
            
            CreditCardAccountDto creditCardAccountDto = new CreditCardAccountDto(1,"Santander","1234",creditCardAccountCurrency,25000,creationDate,closingDate);
            CreditCardAccount creditCardAccount = _controller.CreateCreditCardAccount(creditCardAccountDto);
            creditCardAccount.Id = 1;
            
            _controller.AddCreditCardAccount(creditCardAccountDto,1);
            
            CreditCardAccount expectedCreditCardAccount = _controller.CreateCreditCardAccount(creditCardAccountDto);
            
            Assert.AreEqual(creditCardAccount.Id, expectedCreditCardAccount.Id);
        }
        
        [TestMethod]
        public void WhenControllerAddsANewCreditCardAccount_ShouldAddTheCreditCardAccountToTheAccountsTable()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            DateTime creationDate = DateTime.Today;
            DateTime closingDate = new DateTime(2024, 10, 10);
            
            CreditCardAccountDto creditCardAccountDto = new CreditCardAccountDto(1,"Santander","1234",creditCardAccountCurrency,25000,creationDate,closingDate);
            CreditCardAccount creditCardAccount = _controller.CreateCreditCardAccount(creditCardAccountDto);
            creditCardAccount.Id = 1;
            
            _controller.AddCreditCardAccount(creditCardAccountDto,1);
            
            Assert.IsTrue(_controller._accountsRepository.CreditCardAccountAlreadyExists(creditCardAccount,1));
        }
        
        [TestMethod]
        public void WhenControllerListsAllTheCreditCardAccounts_ShouldReturnACreditCardAccountDtoList()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            DateTime creationDate = DateTime.Today;
            DateTime closingDate = new DateTime(2024, 10, 10);
            
            CreditCardAccountDto creditCardAccountDto = new CreditCardAccountDto(1,"Santander","1234",creditCardAccountCurrency,25000,creationDate,closingDate);

            CreditCardAccount creditCardAccount = _controller.CreateCreditCardAccount(creditCardAccountDto);
            
            _controller.AddCreditCardAccount(creditCardAccountDto,1);

            List<CreditCardAccountDto> expectedCreditCardAccounts = _controller.GetCreditCardAccounts(1);
            
            int expectedCreditCardAccountsCount = expectedCreditCardAccounts.Count;

            Assert.AreEqual(1, expectedCreditCardAccountsCount);
        }
        
        [TestMethod]
        public void WhenControllerGetsACreditCardAccountById_ShouldReturnACreditCardAccountDtoWithTheCreditCardAccountData()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            DateTime creationDate = DateTime.Today;
            DateTime closingDate = new DateTime(2024, 10, 10);
            
            CreditCardAccountDto creditCardAccountDto = new CreditCardAccountDto(1,"Santander","1234",creditCardAccountCurrency,25000,creationDate,closingDate);
            creditCardAccountDto.Id = 1;
            
            _controller.AddCreditCardAccount(creditCardAccountDto,1);

            CreditCardAccountDto expectedCreditCardAccount = _controller.GetCreditCardAccountById(1);
            
            Assert.AreEqual(creditCardAccountDto.Id, expectedCreditCardAccount.Id);
        }
        
        [TestMethod]
        public void WhenControllerModifiesTheCreditCardAccountIssuingBank_ShouldReturnTheNewIssuingBankAssociated()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            DateTime creationDate = DateTime.Today;
            DateTime closingDate = new DateTime(2024, 10, 10);
            
            CreditCardAccountDto creditCardAccountDto = new CreditCardAccountDto(1,"Santander","1234",creditCardAccountCurrency,25000,creationDate,closingDate);
            creditCardAccountDto.Id = 1;
            
            CreditCardAccount creditCardAccount = _controller.CreateCreditCardAccount(creditCardAccountDto);
            
            _controller.AddCreditCardAccount(creditCardAccountDto,1);

            creditCardAccountDto.IssuingBank = "Itau";

            _controller.ChangeCreditCardAccountIssuingBank(creditCardAccount, creditCardAccountDto,1);

            Assert.AreEqual("Itau", creditCardAccount.IssuingBank);
        }
        
        [TestMethod]
        public void WhenControllerModifiesTheCreditCardAccountLastFourDigits_ShouldReturnTheNewLastFourDigitsAssociated()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            DateTime creationDate = DateTime.Today;
            DateTime closingDate = new DateTime(2024, 10, 10);
            
            CreditCardAccountDto creditCardAccountDto = new CreditCardAccountDto(1,"Santander","1234",creditCardAccountCurrency,25000,creationDate,closingDate);
            creditCardAccountDto.Id = 1;
            
            CreditCardAccount creditCardAccount = _controller.CreateCreditCardAccount(creditCardAccountDto);
            
            _controller.AddCreditCardAccount(creditCardAccountDto,1);

            creditCardAccountDto.LastFourDigits = "9876";

            _controller.ChangeCreditCardAccountLastFourDigits(creditCardAccount, creditCardAccountDto,1);

            Assert.AreEqual("9876", creditCardAccount.LastFourDigits);
        }
        
        [TestMethod]
        public void WhenControllerModifiesTheCreditCardAccountCurrency_ShouldReturnTheNewCurrenctAssociated()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            DateTime creationDate = DateTime.Today;
            DateTime closingDate = new DateTime(2024, 10, 10);
            
            CreditCardAccountDto creditCardAccountDto = new CreditCardAccountDto(1,"Santander","1234",creditCardAccountCurrency,25000,creationDate,closingDate);
            creditCardAccountDto.Id = 1;
            
            CreditCardAccount creditCardAccount = _controller.CreateCreditCardAccount(creditCardAccountDto);
            
            _controller.AddCreditCardAccount(creditCardAccountDto,1);

            creditCardAccountDto.AccountCurrency = Currency.Dollar;

            _controller.ChangeCreditCardAccountCurrency(creditCardAccount, creditCardAccountDto,1);

            Assert.AreEqual(Currency.Dollar, creditCardAccount.AccountCurrency);
        }
        
        [TestMethod]
        public void WhenControllerModifiesTheCreditCardAccountClosingDate_ShouldReturnTheNewClosingDateAssociated()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            DateTime creationDate = DateTime.Today;
            DateTime closingDate = new DateTime(2024, 10, 10);
            
            CreditCardAccountDto creditCardAccountDto = new CreditCardAccountDto(1,"Santander","1234",creditCardAccountCurrency,25000,creationDate,closingDate);
            creditCardAccountDto.Id = 1;
            
            CreditCardAccount creditCardAccount = _controller.CreateCreditCardAccount(creditCardAccountDto);
            
            _controller.AddCreditCardAccount(creditCardAccountDto,1);

            creditCardAccountDto.ClosingDate = new DateTime(2025, 10, 10);

            _controller.ChangeCreditCardAccountClosingDate(creditCardAccount, creditCardAccountDto,1);

            Assert.AreEqual(creditCardAccountDto.ClosingDate, creditCardAccount.ClosingDate);
        }
        
        [TestMethod]
        public void WhenControllerUpdatesACreditCardAccount_ShouldReturnTheUpdatedCreditCardAccount()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            DateTime creationDate = DateTime.Today;
            DateTime closingDate = new DateTime(2024, 10, 10);
            
            CreditCardAccountDto creditCardAccountDto = new CreditCardAccountDto(1,"Santander","1234",creditCardAccountCurrency,25000,creationDate,closingDate);
            creditCardAccountDto.Id = 1;
            
            CreditCardAccount creditCardAccount = _controller.CreateCreditCardAccount(creditCardAccountDto);
            
            _controller.AddCreditCardAccount(creditCardAccountDto,1);

            creditCardAccountDto.IssuingBank = "Itau";
            creditCardAccountDto.LastFourDigits = "9876";
            creditCardAccountDto.AccountCurrency = Currency.Dollar;
            creditCardAccountDto.ClosingDate = new DateTime(2025, 10, 10);
            
            _controller.UpdateCreditCardAccount(creditCardAccountDto,1);
            
            CreditCardAccountDto expectedCreditCardAccount = _controller.GetCreditCardAccountById(1);
            
            Assert.AreEqual("Itau", expectedCreditCardAccount.IssuingBank);
            Assert.AreEqual("9876", expectedCreditCardAccount.LastFourDigits);
            Assert.AreEqual(Currency.Dollar, expectedCreditCardAccount.AccountCurrency);
            Assert.AreEqual(new DateTime(2025, 10, 10), expectedCreditCardAccount.ClosingDate);
        }
        
        [TestMethod]
        public void WhenControllerDeletesACreditCardAccount_ShouldDeleteTheCreditCardAccountOfTheAccountsTable()
        {
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            DateTime creationDate = DateTime.Today;
            DateTime closingDate = new DateTime(2024, 10, 10);
            
            CreditCardAccountDto creditCardAccountDto = new CreditCardAccountDto(1,"Santander","1234",creditCardAccountCurrency,25000,creationDate,closingDate);
            creditCardAccountDto.Id = 1;
            
            CreditCardAccount creditCardAccount = _controller.CreateCreditCardAccount(creditCardAccountDto);
            
            _controller.AddCreditCardAccount(creditCardAccountDto,1);
            
            _controller.DeleteCreditCardAccount(1, 1);
            
            Assert.IsFalse(_controller._accountsRepository.CreditCardAccountAlreadyExists(creditCardAccount, 1));
        }
        
        [TestMethod]
        public void WhenControllerReceivesAnEmptyTransactionDto_ShouldMapTheDtoIntoAnEmptyTransactionObject()
        {
            TransactionDto transactionDto = new TransactionDto();
            transactionDto.Title = "Cine";
            
            Transaction transaction = _controller.CreateTransaction(transactionDto);
            
            Assert.IsInstanceOfType(transaction, typeof(Transaction));
        }
        
        [TestMethod]
        public void WhenControllerReceivesATransactionDto_ShouldMapTheDtoIntoATransactionObject()
        {
            ExchangeRate transactionExchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.UruguayanPeso);
            DateTime transactionDate = DateTime.Today;
            CategoryType transactionType = CategoryType.ExpensesCategory;
            Account transactionAccount = new GeneralAccount("Banco Santander",Currency.UruguayanPeso,transactionDate,25000);
            Category transactionCategory = new Category("Cine",transactionDate,transactionType,StatusType.Active);
            
            TransactionDto transactionDto = new TransactionDto(1,"Cine",transactionDate,250,transactionExchangeRate,transactionCategory,transactionAccount);
            
            Assert.IsInstanceOfType(_controller.CreateTransaction(transactionDto), typeof(Transaction));
        }
        
        [TestMethod]
        public void WhenControllerCreatesATransactionWithATransactionDto_ShouldCreateANewTransactionObject()
        {
            ExchangeRate transactionExchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.UruguayanPeso);
            Currency transactionCurrency = Currency.UruguayanPeso;
            DateTime transactionDate = DateTime.Today;
            CategoryType transactionType = CategoryType.ExpensesCategory;
            Account transactionAccount = new GeneralAccount("Banco Santander",transactionCurrency,transactionDate,25000);
            Category transactionCategory = new Category("Cine",transactionDate,transactionType,StatusType.Active);
            
            TransactionDto transactionDto = new TransactionDto(1,"Cine",transactionDate,250,transactionExchangeRate,transactionCategory,transactionAccount);
            
            Transaction expectedTransaction = new Transaction("Cine",transactionDate,250,transactionExchangeRate,transactionCategory,transactionAccount);
            
            Transaction transaction = _controller.CreateTransaction(transactionDto);
            
            Assert.AreEqual(transaction.Title,expectedTransaction.Title);
            Assert.AreEqual(transaction.Date,expectedTransaction.Date);
            Assert.AreEqual(transaction.Amount,expectedTransaction.Amount);
            Assert.AreEqual(transaction.ExchangeRateAssociated,expectedTransaction.ExchangeRateAssociated);
            Assert.AreEqual(transaction.CategoryAssociated,expectedTransaction.CategoryAssociated);
            Assert.AreEqual(transaction.AccountAssociated,expectedTransaction.AccountAssociated);
            
        }
        
        [TestMethod]
        public void WhenControllerAddANewTransaction_ShouldAddTheTransactionToTheTransactionsTable()
        {
            ExchangeRate transactionExchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.UruguayanPeso);
            transactionExchangeRate.Id = 1;
            Currency transactionCurrency = Currency.UruguayanPeso;
            DateTime transactionDate = DateTime.Today;
            CategoryType transactionType = CategoryType.ExpensesCategory;
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Account transactionAccount = new GeneralAccount("Banco Santander",transactionCurrency,transactionDate,25000);
            transactionAccount.Id = 1;
            Category transactionCategory = new Category("Cine",transactionDate,transactionType,StatusType.Active);
            transactionCategory.Id = 1;
            
            TransactionDto transactionDto = new TransactionDto(1,"Cine",transactionDate,250,transactionExchangeRate,transactionCategory,transactionAccount);
            
            Transaction transaction = _controller.CreateTransaction(transactionDto);
            transaction.Id = 1;
            
            _controller.AddTransaction(transactionDto,user.Id,transactionAccount.Id,transactionCategory.Id,transactionExchangeRate.Id);

            Assert.IsTrue(_controller._transactionsRepository.TransactionAlreadyExists(transaction));
        }
        
        [TestMethod]
        public void WhenControllerListsAllTheTransactions_ShouldReturnATransactionDtoList()
        {
            ExchangeRate transactionExchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.UruguayanPeso);
            transactionExchangeRate.Id = 1;
            Currency transactionCurrency = Currency.UruguayanPeso;
            DateTime transactionDate = DateTime.Today;
            CategoryType transactionType = CategoryType.ExpensesCategory;
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Account transactionAccount = new GeneralAccount("Banco Santander",transactionCurrency,transactionDate,25000);
            transactionAccount.Id = 1;
            Category transactionCategory = new Category("Cine",transactionDate,transactionType,StatusType.Active);
            transactionCategory.Id = 1;
            
            TransactionDto transactionDto = new TransactionDto(1,"Cine",transactionDate,250,transactionExchangeRate,transactionCategory,transactionAccount);

            Transaction transaction = _controller.CreateTransaction(transactionDto);
            
            _controller.AddTransaction(transactionDto,user.Id,transactionAccount.Id,transactionCategory.Id,transactionExchangeRate.Id);

            List<TransactionDto> expectedTransactions = _controller.GetTransactions(user.Id);
            
            int expectedTransactionsCount = expectedTransactions.Count;

            Assert.AreEqual(1, expectedTransactionsCount);
        }
        
        [TestMethod]
        public void WhenControllerGetsATransactionById_ShouldReturnATransactionDtoWithTheTransactionData()
        {
            ExchangeRate transactionExchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.UruguayanPeso);
            transactionExchangeRate.Id = 1;
            Currency transactionCurrency = Currency.UruguayanPeso;
            DateTime transactionDate = DateTime.Today;
            CategoryType transactionType = CategoryType.ExpensesCategory;
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Account transactionAccount = new GeneralAccount("Banco Santander",transactionCurrency,transactionDate,25000);
            transactionAccount.Id = 1;
            Category transactionCategory = new Category("Cine",transactionDate,transactionType,StatusType.Active);
            transactionCategory.Id = 1;
            
            TransactionDto transactionDto = new TransactionDto(1,"Cine",transactionDate,250,transactionExchangeRate,transactionCategory,transactionAccount);

            Transaction transaction = _controller.CreateTransaction(transactionDto);
            transaction.Id = 1;
            
            _controller.AddTransaction(transactionDto,user.Id,transactionAccount.Id,transactionCategory.Id,transactionExchangeRate.Id);

            TransactionDto expectedTransaction = _controller.GetTransactionById(1);
            
            Assert.AreEqual(transactionDto.Id, expectedTransaction.Id);
        }
        
        [TestMethod]
        public void WhenControllerModifiesTheTransactionCurrency_ShouldReturnTheNewCurrencyAssociated()
        {
            ExchangeRate transactionExchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.UruguayanPeso);
            transactionExchangeRate.Id = 1;
            Currency transactionCurrency = Currency.UruguayanPeso;
            DateTime transactionDate = DateTime.Today;
            CategoryType transactionType = CategoryType.ExpensesCategory;
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Account transactionAccount = new GeneralAccount("Banco Santander",transactionCurrency,transactionDate,25000);
            transactionAccount.Id = 1;
            Category transactionCategory = new Category("Cine",transactionDate,transactionType,StatusType.Active);
            transactionCategory.Id = 1;
            
            TransactionDto transactionDto = new TransactionDto(1,"Cine",transactionDate,250,transactionExchangeRate,transactionCategory,transactionAccount);

            Transaction transaction = _controller.CreateTransaction(transactionDto);
            transaction.Id = 1;
            
            _controller.AddTransaction(transactionDto,user.Id,transactionAccount.Id,transactionCategory.Id,transactionExchangeRate.Id);

            ExchangeRate newExchangeRate = new ExchangeRate(DateTime.Today, 50, Currency.Dollar);
            transactionDto.ExchangeRateAssociated = newExchangeRate;

            _controller.ChangeTransactionCurrency(transaction, transactionDto);

            Assert.AreEqual(newExchangeRate, transaction.ExchangeRateAssociated);
        }
        
        [TestMethod]
        public void WhenControllerModifiesTheTransactionAmount_ShouldReturnTheNewAmountAssociated()
        {
            ExchangeRate transactionExchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.UruguayanPeso);
            transactionExchangeRate.Id = 1;
            Currency transactionCurrency = Currency.UruguayanPeso;
            DateTime transactionDate = DateTime.Today;
            CategoryType transactionType = CategoryType.ExpensesCategory;
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Account transactionAccount = new GeneralAccount("Banco Santander",transactionCurrency,transactionDate,25000);
            transactionAccount.Id = 1;
            Category transactionCategory = new Category("Cine",transactionDate,transactionType,StatusType.Active);
            transactionCategory.Id = 1;
            
            TransactionDto transactionDto = new TransactionDto(1,"Cine",transactionDate,250,transactionExchangeRate,transactionCategory,transactionAccount);

            Transaction transaction = _controller.CreateTransaction(transactionDto);
            transaction.Id = 1;
            
            _controller.AddTransaction(transactionDto,user.Id,transactionAccount.Id,transactionCategory.Id,transactionExchangeRate.Id);

            transactionDto.Amount = 10;

            _controller.ChangeTransactionAmount(transaction, transactionDto);

            Assert.AreEqual(10, transaction.Amount);
        }
        
        [TestMethod]
        public void WhenControllerModifiesTheTransactionCategory_ShouldReturnTheNewCategoryAssociated()
        {
            ExchangeRate transactionExchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.UruguayanPeso);
            transactionExchangeRate.Id = 1;
            Currency transactionCurrency = Currency.UruguayanPeso;
            DateTime transactionDate = DateTime.Today;
            CategoryType transactionType = CategoryType.ExpensesCategory;
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Account transactionAccount = new GeneralAccount("Banco Santander",transactionCurrency,transactionDate,25000);
            transactionAccount.Id = 1;
            Category transactionCategory = new Category("Cine",transactionDate,transactionType,StatusType.Active);
            transactionCategory.Id = 1;
            
            TransactionDto transactionDto = new TransactionDto(1,"Cine",transactionDate,250,transactionExchangeRate,transactionCategory,transactionAccount);

            Transaction transaction = _controller.CreateTransaction(transactionDto);
            transaction.Id = 1;
            
            _controller.AddTransaction(transactionDto,user.Id,transactionAccount.Id,transactionCategory.Id,transactionExchangeRate.Id);

            transactionDto.CategoryAssociated = new Category("Salidas",transactionDate,transactionType,StatusType.Active);

            _controller.ChangeTransactionCategory(transaction, transactionDto);

            Assert.AreEqual(transactionDto.CategoryAssociated, transaction.CategoryAssociated);
        }
        
        [TestMethod] //solo amount
        public void WhenControllerUpdatesATransactionWithoutCategory_ShouldUpdateTheTransactionInTheDatabase()
        {
            ExchangeRate transactionExchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.UruguayanPeso);
            transactionExchangeRate.Id = 1;
            Currency transactionCurrency = Currency.UruguayanPeso;
            DateTime transactionDate = DateTime.Today;
            CategoryType transactionType = CategoryType.ExpensesCategory;
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Account transactionAccount = new GeneralAccount("Banco Santander",transactionCurrency,transactionDate,25000);
            transactionAccount.Id = 1;
            Category transactionCategory = new Category("Cine",transactionDate,transactionType,StatusType.Active);
            transactionCategory.Id = 1;
            
            TransactionDto transactionDto = new TransactionDto(1,"Cine",transactionDate,250,transactionExchangeRate,transactionCategory,transactionAccount);

            Transaction transaction = _controller.CreateTransaction(transactionDto);
            transaction.Id = 1;
            
            _controller.AddTransaction(transactionDto,user.Id,transactionAccount.Id,transactionCategory.Id,transactionExchangeRate.Id);
            
            ExchangeRate newExchangeRate = new ExchangeRate(DateTime.Today, 50, Currency.Dollar);
            transactionDto.ExchangeRateAssociated = newExchangeRate;
            
            transactionDto.Amount = 10;
            transactionDto.CategoryAssociated = new Category("Salidas",transactionDate,transactionType,StatusType.Active);
            
            _controller.UpdateTransaction(transactionDto);


            TransactionDto expectedTransactionDto = _controller.GetTransactionById(transactionDto.Id);
            
            Assert.AreEqual(10, expectedTransactionDto.Amount);
        }
        
        [TestMethod]
        public void WhenControllerUpdatesATransactionCategoryAssociated_ShouldUpdateTheTransactionCategoryAssociatedInTheDatabase()
        {
            ExchangeRate transactionExchangeRate = new ExchangeRate(DateTime.Today, 40, Currency.UruguayanPeso);
            transactionExchangeRate.Id = 1;
            Currency transactionCurrency = Currency.UruguayanPeso;
            DateTime transactionDate = DateTime.Today;
            CategoryType transactionType = CategoryType.ExpensesCategory;
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Account transactionAccount = new GeneralAccount("Banco Santander",transactionCurrency,transactionDate,25000);
            transactionAccount.Id = 1;
            Category transactionCategory = new Category("Cine",transactionDate,transactionType,StatusType.Active);
            transactionCategory.Id = 1;
            
            TransactionDto transactionDto = new TransactionDto(1,"Cine",transactionDate,250,transactionExchangeRate,transactionCategory,transactionAccount);
            
            Transaction transaction = _controller.CreateTransaction(transactionDto);
            transaction.Id = 1;
            
            _controller.AddTransaction(transactionDto,user.Id,transactionAccount.Id,transactionCategory.Id,transactionExchangeRate.Id);
            
            CategoryDto categoryDto = new CategoryDto(1,"Salidas", transactionDate, CategoryType.ExpensesCategory, StatusType.Active);
            transactionDto.CategoryAssociated = _controller.CreateCategory(categoryDto, user.Id);
            
            _controller.UpdateTransactionCategory(transactionDto, categoryDto);

            Transaction expectedTransaction = _controller._transactionsRepository.FindTransactionById(transactionDto.Id);
          
            Assert.AreEqual(transaction.CategoryAssociated.Name, expectedTransaction.CategoryAssociated.Name); 

        }
        [TestMethod]
        [ExpectedException(typeof(TransactionExceptions))]

        public void WhenControllerUpdatesATransactionThatHasDistinctsCurrency_ShouldThrowAnException()
        {            
            Currency transactionCurrency = Currency.UruguayanPeso;
            DateTime transactionDate = DateTime.Today;
            CategoryType transactionType = CategoryType.ExpensesCategory;
            User user = new User("germanramos@gmail.com", "German", "Ramos", "German123456", "Calle 123");
            user.Id = 1;
            Account transactionAccount = new GeneralAccount("Banco Santander", transactionCurrency, transactionDate, 25000);
            transactionAccount.Id = 1;
            Category transactionCategory = new Category("Cine", transactionDate, transactionType, StatusType.Active);
            transactionCategory.Id = 1;

            ExchangeRate transactionRate = new ExchangeRate(transactionDate, 40, Currency.Dollar);
            transactionRate.Id = 1;

            TransactionDto transactionDto = new TransactionDto(1, "Cine", transactionDate, 250, transactionRate, transactionCategory, transactionAccount);

            Transaction transaction = _controller.CreateTransaction(transactionDto);
            transaction.Id = 1;


        }
        
        [TestMethod]
        public void WhenControllerReceivesAnEmptyExchangeRateDto_ShouldMapTheDtoIntoAnEmptyExchangeRateObject()
        {
            ExchangeRateDto exchangeRateDto = new ExchangeRateDto();
            
            ExchangeRate exchangeRate = _controller.CreateExchangeRate(exchangeRateDto,1);
            
            Assert.IsInstanceOfType(exchangeRate, typeof(ExchangeRate));
        }

        [TestMethod]
        public void WhenControllerReceivesAExchangeRateDto_ShouldMapTheDtoIntoAExchangeRateObject()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Currency exchangeRateCurrency = Currency.Dollar;
            DateTime creationDate = DateTime.Today;
            
            ExchangeRateDto exchangeRateDto = new ExchangeRateDto(1,creationDate,40,exchangeRateCurrency);
            
            Assert.IsInstanceOfType(_controller.CreateExchangeRate(exchangeRateDto,user.Id), typeof(ExchangeRate));
        }
        
        [TestMethod]
        public void WhenControllerCreatesAnExchangeRateWithAnExchangeRateDto_ShouldCreateANewExchangeRateObject()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Currency exchangeRateCurrency = Currency.Dollar;
            DateTime creationDate = DateTime.Today;
            
            ExchangeRateDto exchangeRateDto = new ExchangeRateDto(1,creationDate,40,exchangeRateCurrency);
            
            ExchangeRate expectedExchangeRate = new ExchangeRate(creationDate,40,exchangeRateCurrency);
            
            ExchangeRate exchangeRate = _controller.CreateExchangeRate(exchangeRateDto,user.Id);
            
            Assert.AreEqual(exchangeRate.Date,expectedExchangeRate.Date);
            Assert.AreEqual(exchangeRate.Value,expectedExchangeRate.Value);
            Assert.AreEqual(exchangeRate.RateCurrency,expectedExchangeRate.RateCurrency);
            
        }
        
        [TestMethod]
        public void WhenControllerAddANewExchangeRate_ShouldAddTheExchangeRateToTheExchangeRatesTable()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Currency exchangeRateCurrency = Currency.Dollar;
            DateTime creationDate = DateTime.Today;
            
            ExchangeRateDto exchangeRateDto = new ExchangeRateDto(1,creationDate,40,exchangeRateCurrency);
            
            ExchangeRate exchangeRate = _controller.CreateExchangeRate(exchangeRateDto, user.Id);
            exchangeRate.Id = 1;
            
            _controller.AddExchangeRate(exchangeRateDto, user.Id);
            
            ExchangeRate expectedExchangeRate = _controller._exchangeRatesRepository.FindExchangeRateById(1);

            Assert.AreEqual(exchangeRate.Id,expectedExchangeRate.Id);
        }
        
        [TestMethod]
        public void WhenControllerListsAllTheExchangeRates_ShouldReturnAnExchangeRateDtoList()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Currency exchangeRateCurrency = Currency.Dollar;
            DateTime creationDate = DateTime.Today;
            
            ExchangeRateDto exchangeRateDto = new ExchangeRateDto(1,creationDate,40,exchangeRateCurrency);
            
            ExchangeRate exchangeRate = _controller.CreateExchangeRate(exchangeRateDto, user.Id);
            exchangeRate.Id = 1;
            
            _controller.AddExchangeRate(exchangeRateDto, user.Id);

            List<ExchangeRateDto> expectedExchangeRates = _controller.GetExchangeRates(user.Id);
            
            int expectedExchangeRatesCount = expectedExchangeRates.Count;

            Assert.AreEqual(1, expectedExchangeRatesCount);
        }
        
        [TestMethod]
        public void WhenControllerGetsAnExchangeRateById_ShouldReturnAnExchangeRateDtoWithTheExchangeRateData()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Currency exchangeRateCurrency = Currency.Dollar;
            DateTime creationDate = DateTime.Today;
            
            ExchangeRateDto exchangeRateDto = new ExchangeRateDto(1,creationDate,40,exchangeRateCurrency);
            
            ExchangeRate exchangeRate = _controller.CreateExchangeRate(exchangeRateDto, user.Id);
            exchangeRate.Id = 1;
            
            _controller.AddExchangeRate(exchangeRateDto, user.Id);

            ExchangeRateDto expectedExchangeRate = _controller.GetExchangeRateById(1);
            
            Assert.AreEqual(exchangeRateDto.Id, expectedExchangeRate.Id);
        }
        
        [TestMethod]
        public void WhenControllerModifiesTheExchangeRateValue_ShouldReturnTheNewValueAssociated()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Currency exchangeRateCurrency = Currency.Dollar;
            DateTime creationDate = DateTime.Today;
            
            ExchangeRateDto exchangeRateDto = new ExchangeRateDto(1,creationDate,40,exchangeRateCurrency);
            
            ExchangeRate exchangeRate = _controller.CreateExchangeRate(exchangeRateDto, user.Id);
            
            _controller.AddExchangeRate(exchangeRateDto, user.Id);

            exchangeRateDto.Value = 50;

            _controller.ChangeExchangeRateValue(exchangeRate, exchangeRateDto);

            Assert.AreEqual(50, exchangeRate.Value);
        }
        
        [TestMethod]
        public void WhenControllerUpdatesAnExchangeRate_ShouldUpdateTheExchangeRateInTheDatabase()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Currency exchangeRateCurrency = Currency.Dollar;
            DateTime creationDate = DateTime.Today;
            
            ExchangeRateDto exchangeRateDto = new ExchangeRateDto(1,creationDate,40,exchangeRateCurrency);
            
            ExchangeRate exchangeRate = _controller.CreateExchangeRate(exchangeRateDto,user.Id);
            
            _controller.AddExchangeRate(exchangeRateDto,user.Id);
            
            exchangeRateDto.Value = 50;
            
            _controller.UpdateExchangeRate(exchangeRateDto);

            ExchangeRateDto expectedExchangeRateDto = _controller.GetExchangeRateById(exchangeRateDto.Id);
            
            Assert.AreEqual(50, expectedExchangeRateDto.Value);
        }
        
        [TestMethod]
        public void WhenControllerDeletesAnExchangeRate_ShouldDeleteTheExchangeRateFromTheExchangeRatesTable()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Currency exchangeRateCurrency = Currency.Dollar;
            DateTime creationDate = DateTime.Today;
            
            ExchangeRateDto exchangeRateDto = new ExchangeRateDto(1,creationDate,40,exchangeRateCurrency);
            
            ExchangeRate exchangeRate = _controller.CreateExchangeRate(exchangeRateDto,user.Id);

            _controller.AddExchangeRate(exchangeRateDto,user.Id);
            
            _controller.DeleteExchangeRate(exchangeRateDto, exchangeRateDto.Id);

            List<ExchangeRate> exchangeRates = _controller._exchangeRatesRepository.GetExchangeRates(user.Id);
            
            Assert.AreEqual(0, exchangeRates.Count);
        }
        
        [TestMethod]
        public void WhenControllerReceivesAnEmptySpendingGoalDto_ShouldMapTheDtoIntoAnEmptySpendingGoalObject()
        {
            SpendingGoalDto spendingGoalDto = new SpendingGoalDto();
            spendingGoalDto.Title = "Menos Noche";
            
            SpendingGoal spendingGoal = _controller.CreateSpendingGoal(spendingGoalDto);
            
            Assert.IsInstanceOfType(spendingGoal, typeof(SpendingGoal));
        }
        
        [TestMethod]
        public void WhenControllerReceivesAnSpendingGoalDto_ShouldMapTheDtoIntoAnSpendingGoalObject()
        {
            Category category = new Category("Cine", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            List<Category> categories = new List<Category>();
            categories.Add(category);

            Currency goalCurrency = Currency.UruguayanPeso;

            SpendingGoalDto spendingGoalDto = new SpendingGoalDto(1, "Menos Noche",6000, goalCurrency,categories);
            
            Assert.IsInstanceOfType(_controller.CreateSpendingGoal(spendingGoalDto), typeof(SpendingGoal));
        }
        
        [TestMethod]
        public void WhenControllerCreatesAnSpendingGoalWithAnSpendingGoalDto_ShouldCreateANewSpendingGoalObject()
        {
            Category category = new Category("Cine", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            List<Category> categories = new List<Category>();
            categories.Add(category);
            
            Currency goalCurrency = Currency.UruguayanPeso;

            SpendingGoalDto spendingGoalDto = new SpendingGoalDto(1, "Menos Noche", 6000, goalCurrency,categories);

            SpendingGoal expectedSpendingGoal = new SpendingGoal("Menos Noche", 6000, goalCurrency,categories);
            
            SpendingGoal spendingGoal = _controller.CreateSpendingGoal(spendingGoalDto);
            
            Assert.AreEqual(spendingGoal.Title,expectedSpendingGoal.Title);
            Assert.AreEqual(spendingGoal.MaximumAmount,expectedSpendingGoal.MaximumAmount);
            Assert.AreEqual(spendingGoal.Categories,expectedSpendingGoal.Categories);
            
        }
        
        [TestMethod]
        public void WhenControllerAddANewSpendingGoal_ShouldAddTheSpendingGoalToTheSpendingGoalsTable()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Category category = new Category("Cine", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            List<Category> categories = new List<Category>();
            categories.Add(category);
            
            Currency goalCurrency = Currency.UruguayanPeso;

            SpendingGoalDto spendingGoalDto = new SpendingGoalDto(1, "Menos Noche", 6000, goalCurrency,categories);
            
            SpendingGoal spendingGoal = _controller.CreateSpendingGoal(spendingGoalDto);
            spendingGoal.Id = 1;
            
            _controller.AddSpendingGoal(spendingGoalDto,user.Id);
            
            SpendingGoal expectedSpendingGoal = _controller._spendingGoalsRepository.FindSpendingGoalById(1);

            Assert.AreEqual(spendingGoal.Id,expectedSpendingGoal.Id);
        }
        
        [TestMethod]
        public void WhenControllerTriesToCreateASpendingGoalThatAlreadyExists_ShouldReturnTheExistingSpendingGoal()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Category category = new Category("Cine", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            List<Category> categories = new List<Category>();
            categories.Add(category);
            
            Currency goalCurrency = Currency.UruguayanPeso;

            SpendingGoalDto spendingGoalDto = new SpendingGoalDto(1, "Menos Noche", 6000, goalCurrency,categories);
            
            SpendingGoal spendingGoal = _controller.CreateSpendingGoal(spendingGoalDto);
            spendingGoal.Id = 1;
            
            _controller.AddSpendingGoal(spendingGoalDto,user.Id);

            SpendingGoal expectedSpendingGoal = _controller.CreateSpendingGoal(spendingGoalDto);
            
            Assert.AreEqual(spendingGoal.Id,expectedSpendingGoal.Id);
        }
        
        [TestMethod]
        public void WhenControllerGetsAnSpendingGoalById_ShouldReturnAnSpendingGoalDtoWithTheSpendingGoalData()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Category category = new Category("Cine", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            List<Category> categories = new List<Category>();
            categories.Add(category);
            
            Currency goalCurrency = Currency.UruguayanPeso;

            SpendingGoalDto spendingGoalDto = new SpendingGoalDto(1, "Menos Noche", 6000, goalCurrency,categories);
            
            SpendingGoal spendingGoal = _controller.CreateSpendingGoal(spendingGoalDto);
            spendingGoal.Id = 1;
            
            _controller.AddSpendingGoal(spendingGoalDto,user.Id);

            SpendingGoalDto expectedSpendingGoal = _controller.GetSpendingGoalById(1);
            
            Assert.AreEqual(spendingGoalDto.Id, expectedSpendingGoal.Id);
        }
        
        [TestMethod]
        public void WhenControllerListsAllTheSpendingGoals_ShouldReturnAnSpendingGoalDtoList()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Category category = new Category("Cine", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            List<Category> categories = new List<Category>();
            categories.Add(category);
            
            Currency goalCurrency = Currency.UruguayanPeso;

            SpendingGoalDto spendingGoalDto = new SpendingGoalDto(1, "Menos Noche", 6000, goalCurrency,categories);
            
            SpendingGoal spendingGoal = _controller.CreateSpendingGoal(spendingGoalDto);
            spendingGoal.Id = 1;
            
            _controller.AddSpendingGoal(spendingGoalDto, user.Id);

            List<SpendingGoalDto> expectedSpendingGoals = _controller.GetSpendingGoals(user.Id);
            
            int expectedSpendingGoalsCount = expectedSpendingGoals.Count;

            Assert.AreEqual(1, expectedSpendingGoalsCount);
        }
        
        [TestMethod]
        public void WhenControllerModifiesTheSpendingGoalMaxAmount_ShouldReturnTheNewMaxAmountAssociated()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Category category = new Category("Cine", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            List<Category> categories = new List<Category>();
            categories.Add(category);
            
            Currency goalCurrency = Currency.UruguayanPeso;

            SpendingGoalDto spendingGoalDto = new SpendingGoalDto(1, "Menos Noche", 6000, goalCurrency,categories);
            
            SpendingGoal spendingGoal = _controller.CreateSpendingGoal(spendingGoalDto);
            
            _controller.AddSpendingGoal(spendingGoalDto, user.Id);

            spendingGoalDto.MaxAmount = 10000;

            _controller.ChangeSpendingGoalMaxAmount(spendingGoal, spendingGoalDto, user.Id);

            Assert.AreEqual(10000, spendingGoal.MaximumAmount);
        }
        
        [TestMethod]
        public void WhenControllerModifiesTheSpendingGoalCategories_ShouldReturnTheNewCategoriesAssociated()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Category category = new Category("Cine", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            List<Category> categories = new List<Category>();
            categories.Add(category);
            
            Currency goalCurrency = Currency.UruguayanPeso;

            SpendingGoalDto spendingGoalDto = new SpendingGoalDto(1, "Menos Noche", 6000, goalCurrency,categories);
            
            SpendingGoal spendingGoal = _controller.CreateSpendingGoal(spendingGoalDto);
            
            _controller.AddSpendingGoal(spendingGoalDto, user.Id);

            Category category2 = new Category("Cine", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            List<Category> categories2 = new List<Category>();
            categories2.Add(category2);
            
            spendingGoalDto.Categories = categories2;

            _controller.ChangeSpendingGoalCategories(spendingGoal, spendingGoalDto, user.Id);
            
            Assert.AreEqual(categories2, spendingGoal.Categories);
        }
        
        [TestMethod]
        public void WhenControllerUpdatesAnSpendingGoal_ShouldUpdateTheSpendingGoalInTheDatabase()
        {
            User user = new User("germanramos@gmail.com","German","Ramos","German123456","Calle 123");
            user.Id = 1;
            Category category = new Category("Cine", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            List<Category> categories = new List<Category>();
            categories.Add(category);
            
            Currency goalCurrency = Currency.UruguayanPeso;

            SpendingGoalDto spendingGoalDto = new SpendingGoalDto(1, "Menos Noche", 6000, goalCurrency,categories);
            
            SpendingGoal spendingGoal = _controller.CreateSpendingGoal(spendingGoalDto);
            
            _controller.AddSpendingGoal(spendingGoalDto, user.Id);
            
            spendingGoalDto.MaxAmount = 10000;
            Category category2 = new Category("Cine", DateTime.Today, CategoryType.ExpensesCategory, StatusType.Active);
            List<Category> categories2 = new List<Category>();
            categories2.Add(category2);
            spendingGoalDto.Categories = categories2;
            
            _controller.UpdateSpendingGoal(spendingGoalDto, user.Id);

            SpendingGoalDto expectedSpendingGoalDto = _controller.GetSpendingGoalById(spendingGoalDto.Id);
            
            Assert.AreEqual(10000, expectedSpendingGoalDto.MaxAmount);
            Assert.AreEqual(categories2, expectedSpendingGoalDto.Categories);
        }

    }
}