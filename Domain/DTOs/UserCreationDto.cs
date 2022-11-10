namespace Domain.DTOs;

public class UserCreationDto
{
    public string UserName { get;}
    public string Password { get;}

    public string Role { get;}

    public UserCreationDto(string userName,string passWord)
    {
        UserName = userName;
        Password = passWord;
        Role = "User";
    }
}