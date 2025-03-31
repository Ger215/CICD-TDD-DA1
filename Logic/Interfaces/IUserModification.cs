using Logic.DTOs;
using Models;

namespace Logic.Interfaces;

public interface IUserModification
{
    public void ChangeUserName(User user,UserDto userDto);
    public void ChangeUserSurname(User user, UserDto userDto);
    public void ChangeUserAddress(User user, UserDto userDto);
    public void ChangeUserPassword(User user, UserDto userDto);
}