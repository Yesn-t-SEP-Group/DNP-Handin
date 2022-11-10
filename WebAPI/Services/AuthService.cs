using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Domain.Models;
using EfcDataAccess;

//using FileData;

namespace WebAPI.Services;

public class AuthService : IAuthService
{
    //private FileContext file = new FileContext();
    private TodoContext context = new TodoContext();
    private readonly IList<User> Users = new List<User>();

    public AuthService()
    {
    //file.LoadData();
    }

    public Task<User> ValidateUser(string username, string password)
    {
    //    file.LoadData();
    //    User? existingUser = file.Users.FirstOrDefault(u => 
     //       u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
     ;
    // file.LoadData();
     
     Console.WriteLine(username+password);
     User? existingUser = context.Users.FirstOrDefault(u => 
            u.UserName.ToLower().Equals(username.ToLower()));
     
        
        if (existingUser == null)
        {
            throw new Exception("User not found");
        }

        if (!existingUser.Password.Equals(password))
        {
            throw new Exception("Password mismatch");
        }

        return Task.FromResult(existingUser);
    }
    

    public Task RegisterUser(User user)
    {

        if (string.IsNullOrEmpty(user.UserName))
        {
            throw new ValidationException("Username cannot be null");
        }

        if (string.IsNullOrEmpty(user.Password))
        {
            throw new ValidationException("Password cannot be null");
        }
        // Do more user info validation here
        
        // save to persistence instead of list
        
   //     file.Users.Add(user);
   //     file.SaveChanges();
        
        return Task.CompletedTask;
    }
}