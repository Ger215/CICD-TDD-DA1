using DataAccess.Context;
using DataAccess.Repository;
using Models;
using Models.Exceptions;

namespace DataAccessTests
{
    [TestClass]
    public class UserTests
    {
        private UsersRepository _userRepository;
        private CategoryRepository _categoryRepository;
        private ApplicationDbContext _context;
        private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();
    
        [TestInitialize]
        public void SetUp()
        {
            _context = _contextFactory.CreateDbContext();
            _categoryRepository = new CategoryRepository(_context);
            _userRepository = new UsersRepository(_context);
        }
    
        [TestCleanup]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
        
        [TestMethod]

        public void WhenCreatingANewEmptyUser_ShouldReturnAnEmptyUser()
        {
            User user = new User();
            User expectedUser = new User();
            
            Assert.AreEqual(expectedUser, user);
        }
        
        [TestMethod]

        public void WhenCreatingANewUserWithPasswordValidations_ShouldReturnTrueIfItIsAValidPassword() 
        {
            User user = new User("germamram@gmail.com", "German", "Ramos", "GermanRamos2023");
            
            Assert.IsTrue(user.ValidatePassword());
        }

        [TestMethod]
        [ExpectedException(typeof(UserExceptions))]
        public void WhenChoosingAPasswordWithLessThanTenDigits_ShouldThrowAUserExcepction() 
        {
            User user = new User("germamram@gmail.com", "German", "Ramos", "German");
        }

        [TestMethod]
        [ExpectedException(typeof(UserExceptions))]

        public void WhenChoosingAPasswordWithMoreThanThirtyDigits_ShouldThrowAUserException() 
        {
            User user = new User("germamram@gmail.com", "German", "Ramos", "German1234German1234German123456");

        }

        [TestMethod]

        public void WhenChoosingAPasswordWithAtLeastOneUppercaseLetter_ShouldReturnTrue() 
        {
            User user = new User("germamram@gmail.com", "German", "Ramos", "German123456");

            Assert.IsTrue(user.ValidatePassword());
        }

        [TestMethod]
        [ExpectedException(typeof(UserExceptions))]

        public void WhenChoosingAPasswordWithNoneUppercaseLetter_ShouldThrowAUserException() 
        {
            User user = new User("germamram@gmail.com", "German", "Ramos", "german123456");
        }

        [TestMethod]

        public void WhenUserChangesPasswordToAValidOne_ShouldReturnTrue() 
        {
            User user = new User("germamram@gmail.com", "German", "Ramos", "German123456");

            string newValidPassword = "Santiago123456";
            user.Password = newValidPassword;

            Assert.AreEqual(newValidPassword, user.Password);
        }

        [TestMethod]
        [ExpectedException(typeof(UserExceptions))]

        public void WhenUserChangesPasswordToAnInvalidOne_ShouldThrowAUserException() 
        {
            User user = new User("germamram@gmail.com", "German", "Ramos", "German123456");

            user.Password= "german";
        }
        
        [TestMethod]
        [ExpectedException(typeof(UserExceptions))]
        public void WhenUserChangesNameToAnEmptyOne_ShouldReturnAUserException()
        {
            User user = new User("germamram@gmail.com", "German", "Ramos", "German123456");

            user.Name= "";
        }

        [TestMethod]
        public void WhenUserChangesNameToAValidName_ShouldReturnTheNewValidName() 
        {
            User user = new User("germamram@gmail.com", "German", "Ramos", "German123456");

            string newName = "Santiago";

            user.Name =newName;

            Assert.AreEqual(newName, user.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(UserExceptions))] 
        public void WhenUserChangesSurnameToAnEmptyOne_ShouldReturnAUserException() 
        {
            User user = new User("germamram@gmail.com", "German", "Ramos", "German123456");

            user.Surname = "";
        }


        [TestMethod]
        [ExpectedException(typeof(UserExceptions))]
        public void WhenUserSetAnEmptyEmail_ShouldReturnAUserException()
        {
            User user = new User("", "German", "Ramos", "German123456");

        }

        [TestMethod]
        public void WhenUserChangesSurnameToAValidSurname_ShouldReturnTheNewValidSurname() 
        {
            User user = new User("germamram@gmail.com", "German", "Ramos", "German123456");
            
            string newSurname = "Lopez";
            user.Surname= newSurname;

            Assert.AreEqual(newSurname, user.Surname);

        }

        [TestMethod]
        public void WhenCreatingAUserWithAddressDirection_ShouldReturnTheNewUserWithAddressDirection()
        {
            User user = new User("germamram@gmail.com", "German", "Ramos", "German123456","Av. Brasil 2233");

            string expectedAddressDirection = "Av. Brasil 2233";

            Assert.AreEqual(expectedAddressDirection, user.AddressDirection);

        }

        [TestMethod]
        public void WhenUserChangesAddressDirection_ShouldReturnTheNewAddressDirection() 
        {
            User user = new User("germamram@gmail.com", "German", "Ramos", "German123456", "Av. Brasil 2233");

            string newAddressDirection = "Av. Italia 2896";

            user.AddressDirection = newAddressDirection;

            Assert.AreEqual(newAddressDirection, user.AddressDirection);

        }

        [TestMethod]
        public void WhenCreatingUserWithValidEmail_ShouldReturnTheNewUserWithEmailWithValidation() 
        {
            User user = new User("germamram@gmail.com", "German", "Ramos", "German123456", "Av. Brasil 2233");

            Assert.IsTrue(user.EmailValidationPattern());

        }

        [TestMethod]
        [ExpectedException(typeof(UserExceptions))] 
        public void WhenCreatingUserWithInvalidEmail_ShouldReturnAUserException()
        {
            User user = new User("germamramgmail.com", "German", "Ramos", "German123456", "Av. Brasil 2233");
        }

        [TestMethod]
        [ExpectedException(typeof(UserExceptions))] 

        public void WhenCreatingUserWithAlreadyExistingMail_ShouldReturnAUserException()
        {
            User user1 = new User("germamram@gmail.com", "German", "Ramos", "German123456", "Av. Brasil 2233");
            User user2 = new User("germamram@gmail.com", "Santiago", "Lopez", "Santiago123456", "Av. Italia 2896");

            user1.AreEqual(user2);

        }
    }
}