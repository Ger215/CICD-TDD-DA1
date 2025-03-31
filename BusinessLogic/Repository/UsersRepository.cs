using DataAccess.Context;
using Models.Exceptions;
using Models;

namespace DataAccess.Repository
{
    public class UsersRepository 
    {
        private ApplicationDbContext _database;

        public UsersRepository(ApplicationDbContext database)
        {
            _database = database;
        }

        public void AddUser(User user)
        {
            if (UserAlreadyExists(user))
            {
                UserAlreadyExistsException();
            }

            AddNewUserToUsersTable(user);

        }

        private static void UserAlreadyExistsException()
        {
            throw new RepositoryExceptions("The user already exists");
        }

        private void AddNewUserToUsersTable(User user)
        {
            _database.Users.Add(user);

            _database.SaveChanges();
        }

        public bool UserAlreadyExists(User newUser)
        {
            return _database.Users.Any(user => user.Email == newUser.Email);
        }
        
        public bool CheckLogin(string email, string password)
        {
            bool login = false;
            
            if(_database.Users.Any(currentUser => currentUser.Email == email && currentUser.Password != password))
            {
                throw new UserExceptions("Wrong password");
            }
            else if(!_database.Users.Any(currentUser => currentUser.Email == email))
            {
                throw new UserExceptions("User not found");
            }
            else if (_database.Users.Any(currentUser => currentUser.Email == email && currentUser.Password == password))
            {
                login = true;   
            }
            return login;
        }

        public void UpdateUser(User user)
        {
            var dbUser = _database.Users.FirstOrDefault(u => u.Email == user.Email);
            if(dbUser != null)
            {
                dbUser.Name = user.Name;
                dbUser.Surname = user.Surname;
                dbUser.Password = user.Password;
                dbUser.Categories = user.Categories;
                dbUser.UserCreditCards = user.UserCreditCards;
                dbUser.Goals = user.Goals;
                dbUser.Transactions = user.Transactions;
                dbUser.ExchangeRates = user.ExchangeRates;
                dbUser.UserGeneralAccounts = user.UserGeneralAccounts;
                _database.SaveChanges();
            }
        }

        public User FindUserByEmail(string email)
        {
            User user = _database.Users.FirstOrDefault(user => user.Email == email);
            if(user == null)
            {
                throw new RepositoryExceptions("User not found"); 
            }
            return user;
        }
    }
}
