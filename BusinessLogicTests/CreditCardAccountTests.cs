using Models;
using Models.Enums;

namespace DataAccessTests
{
    [TestClass]
    public class CreditCardAccountTests
    {
        [TestMethod]

        public void WhenCreatingANewEmptyCreditCardAccount_ShouldReturnAnEmptyCreditCardAccount()
        {
            CreditCardAccount creditCardAccount = new CreditCardAccount();
            CreditCardAccount expectedCreditCardAccount = new CreditCardAccount();
            
            Assert.AreEqual(expectedCreditCardAccount.Name, creditCardAccount.Name);
        }
        
        [TestMethod]
        public void WhenCreatingANewCreditCardAccount_ShouldReturnTheNewCreditCardAccount()
        {
            string creditCardAccountName = "Caja de Ahorros Santander";
            string issuingBank = "Itau";
            string lastFourDigits = "4321";
            double availableBalance = 5000.65;
            DateTime closingDate = new DateTime(2023,9,23);
            
            Currency creditCardAccountCurrency = Currency.UruguayanPeso;
            DateTime todayDate = DateTime.Now;
            
            CreditCardAccount creditCardAccount = 
                new CreditCardAccount(creditCardAccountName, creditCardAccountCurrency, todayDate,issuingBank,lastFourDigits,
                                      availableBalance,closingDate);

            Assert.AreEqual(creditCardAccountName, creditCardAccount.Name);
            Assert.AreEqual(creditCardAccountCurrency, creditCardAccount.AccountCurrency);
            Assert.AreEqual(todayDate, creditCardAccount.CreationDate);

        }

        
    }
}