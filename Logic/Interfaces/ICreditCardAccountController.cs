using Logic.DTOs;
using Models;

namespace Logic.Interfaces;

public interface ICreditCardAccountController
{
    public CreditCardAccount CreateCreditCardAccount(CreditCardAccountDto creditCardAccountDto);
    public void AddCreditCardAccount(CreditCardAccountDto creditCardAccountDto, int userId);
    public List<CreditCardAccountDto> GetCreditCardAccounts(int userId);
    public CreditCardAccountDto GetCreditCardAccountById(int id);
    public void UpdateCreditCardAccount(CreditCardAccountDto element, int userId);
    public void DeleteCreditCardAccount(int id , int userId);

}
