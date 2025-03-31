using Models;
using Models.Enums;

namespace DataAccessTests
{
    [TestClass]
    public class GeneralAccountTests
    {
        [TestMethod]

        public void WhenCreatingANewEmptyGeneralAccount_ShouldReturnAnEmptyGeneralAccount()
        {
            GeneralAccount generalAccount = new GeneralAccount();

            Assert.AreEqual("", generalAccount.Name);
            Assert.AreEqual(Currency.UruguayanPeso, generalAccount.AccountCurrency);
            Assert.AreEqual(new DateTime(), generalAccount.CreationDate);
            Assert.AreEqual(0, generalAccount.InitialAmmount);
        }
        
        [TestMethod]
        public void WhenCreatingANewGeneralAccount_ShouldReturnTheNewGeneralAccount()
        {
            string generalAccountName = "Caja de Ahorros Santander";
            Currency generalAccountCurrency = Currency.UruguayanPeso;
            DateTime todayDate = DateTime.Now;
            double generalAccountInitialAmmount = 2000.50;

            GeneralAccount generalAccount = new GeneralAccount(generalAccountName, generalAccountCurrency, todayDate, generalAccountInitialAmmount);

            Assert.AreEqual(generalAccountName, generalAccount.Name);
            Assert.AreEqual(generalAccountCurrency, generalAccount.AccountCurrency);
            Assert.AreEqual(todayDate, generalAccount.CreationDate);
            Assert.AreEqual(generalAccountInitialAmmount, generalAccount.InitialAmmount);

        }

        
    }
}