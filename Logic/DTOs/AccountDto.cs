namespace Logic.DTOs;

public class AccountDto
{
    public int Id { get; set; }
    public AccountDto() { }
    
    public AccountDto(int id)
    {
        Id = id;
    }

}