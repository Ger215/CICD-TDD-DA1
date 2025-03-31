namespace Logic.DTOs;

public class UserDto
{
    public string Address { get; set; }

    public string Password { get; set; }

    public string Surname { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }
    public UserDto(){}

    public UserDto(string email, string name, string surname, string password, string address)
    {
        Email = email;
        Name = name;
        Surname = surname;
        Password = password;
        Address = address;
    }


}