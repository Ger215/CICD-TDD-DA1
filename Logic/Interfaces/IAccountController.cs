using Logic.DTOs;

namespace Logic.Interfaces;

public interface IAccountController
{
    public List<AccountDto> GetAccounts(int userId);
    public AccountDto GetAccountById(int accountId);
}