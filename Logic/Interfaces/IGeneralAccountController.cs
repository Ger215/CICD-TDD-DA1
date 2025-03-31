using Logic.DTOs;
using Models;

namespace Logic.Interfaces;

public interface IGeneralAccountController
{
    public GeneralAccount CreateGeneralAccount(GeneralAccountDto generalAccountDto, int userId);
    public void AddGeneralAccount(GeneralAccountDto generalAccountDto, int userId);
    public List<GeneralAccountDto> GetGeneralAccounts(int userId);
    public GeneralAccountDto GetGeneralAccountById(int id);
    public void UpdateGeneralAccount(GeneralAccountDto element, int userId);
    public void DeleteGeneralAccount(int id, int userId);
}