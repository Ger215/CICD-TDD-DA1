using Logic.DTOs;
using Models;

namespace Logic.Interfaces;

public interface IUserAuthentication 
{
    public User CreateUser(UserDto userDto);
    public void AddUser(UserDto userDto);
    public bool CheckUserLogin(string email, string password);
    public User FindUserByEmail(string email);
}