namespace Domain.DTOs;

public class UserCreationDto
{
    public string UserName { get;}
    public string Password { get;}

    public UserCreationDto(string userName,string passWord)
    {
        UserName = userName;
        Password = passWord;
    }
}