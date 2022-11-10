using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Post
{
    public int Id { get; set; }
   
    public User Owner { get; private set; }
    
    public int OwnerId { get; set; }
    
    public string Title { get; private set; }
    
    public string Body { get; private set; }
   // public bool IsCompleted { get; set; }

    public Post(User owner, string title, string body)
    {
        Owner = owner;
        Title = title;
        Body = body;
        OwnerId = owner.Id;
    }

    private Post()
    {
    }
}