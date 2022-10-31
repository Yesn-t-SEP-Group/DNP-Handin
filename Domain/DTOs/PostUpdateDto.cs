namespace Domain.DTOs;

public class PostUpdateDto
{
    public int Id { get; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public int? OwnerId { get; set; }

    public PostUpdateDto(int id)
    {
        Id = id;
    }
}