namespace Domain.DTOs;

public class UserCreationDto
{
    public string UserName { get;}
    public string Password { get;}

    public string Role { get;}
    
    public string Name { get; set; }

    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }

    public UserCreationDto(string userName,string passWord, string name, string email, string phoneNumber, string address)
    {
        UserName = userName;
        Password = passWord;
        Role = "User"; 
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;

    }
}