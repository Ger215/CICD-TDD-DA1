using DataAccess.Context;
using DataAccess.Repository;
using Models;
using Models.Exceptions;

namespace DataAccessTests
{
    [TestClass]
    public class UsersRepositoryTests
    {
        private UsersRepository _repository;
        private ApplicationDbContext _context;
        private readonly IApplicationDbContextFactory _contextFactory = new InMemoryAppContextFactory();
    
        [TestInitialize]
        public void SetUp()
        {
            _context = _contextFactory.CreateDbContext();
            _repository = new UsersRepository(_context);
        }
    
        [TestCleanup]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
        
        [TestMethod]
        public void WhenAddingANewUser_ShouldAddTheNewUserInUsersTable()
        {
            User user = new User("johndoe@gmail.com", "John", "Doe", "Password38941", "");

            _repository.AddUser(user);

            var userInDb = _context.Users.First();
            
            Assert.AreEqual(user, userInDb);
        }

        [TestMethod]
        [ExpectedException(typeof(RepositoryExceptions))]

        public void WhenAddingAUserThatAlreadyExists_ShouldThrowAnException()
        {

            User user1 = new User("johndoe@gmail.com", "John", "Doe", "Password38941", "");
            User user2 = new User("johndoe@gmail.com", "German", "Ramos", "Password38941", "");

            _repository.AddUser(user1);
            _repository.AddUser(user2);
            
        }
        
        [TestMethod]
        [ExpectedException(typeof(RepositoryExceptions))]
        public void WhenSearchingForANonExistingUser_ShouldReturnARepositoryException()
        {
            User user = new User("johndoe@gmail.com", "John", "Doe", "Password38941", "");

            _repository.FindUserByEmail(user.Email);
        }
        
        [TestMethod]
        [ExpectedException(typeof(UserExceptions))]
        public void WhenSearchingForANonExistingUserEmail_ShouldReturnAUserException()
        {
            User user = new User("johndoe@gmail.com", "John", "Doe", "Password38941", "");
            
            _repository.AddUser(user);
            
            _repository.CheckLogin("germanramos@gmail.com", user.Password);
        }
        
        [TestMethod]
        [ExpectedException(typeof(UserExceptions))]
        public void WhenSearchingForAnExistingUserWithAWrongPassword_ShouldReturnAUserException()
        {
            User user = new User("johndoe@gmail.com", "John", "Doe", "Password38941", "");
            
            _repository.AddUser(user);
            
            _repository.CheckLogin(user.Email, "German123456");
        }

    }
}