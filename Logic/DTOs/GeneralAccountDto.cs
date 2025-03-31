using Models.Enums;

namespace Logic.DTOs;

public class GeneralAccountDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public double InitialAmmount { get; set; }

    public Currency AccountCurrency { get; set; }

    public DateTime CreationDate { get; set; }
    public GeneralAccountDto() { }
    
    public GeneralAccountDto(int id, string name, double initialAmmount, Currency accountCurrency, DateTime creationDate)
    {
        Id = id;
        Name = name;
        InitialAmmount = initialAmmount;
        AccountCurrency = accountCurrency;
        CreationDate = creationDate;
    }
}