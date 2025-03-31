using Logic.DTOs;

namespace Logic.Interfaces
{
    public interface IReportExpenses
    {
        public List<TransactionDto> GetTransactionsFiltered(DateTime inital, DateTime final, CategoryDto category,AccountDto accountDto, int userId);
    }
}
