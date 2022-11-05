using Application.DaoInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace FileData.DAOs;

public class PostFileDao : IPostDao
{
    private readonly FileContext context;

    public PostFileDao(FileContext context)
    {
        this.context = context;
    }

    public Task<Post> CreateAsync(Post post)
    {
        int id = 1;
        if (context.Todos.Any())
        {
            id = context.Todos.Max(t => t.Id);
            id++;
        }

        post.Id = id;

        context.Todos.Add(post);
        context.SaveChanges();

        return Task.FromResult(post);
    }

    public Task<IEnumerable<Post>> GetAsync(SearchPostParametersDto searchParams)
    {
        IEnumerable<Post> result = context.Todos.AsEnumerable();

        if (!string.IsNullOrEmpty(searchParams.Username))
        {
            // we know username is unique, so just fetch the first
            result = context.Todos.Where(todo =>
                todo.Owner.UserName.Equals(searchParams.Username, StringComparison.OrdinalIgnoreCase));
        }

        if (searchParams.UserId != null)
        {
            result = result.Where(t => t.Owner.Id == searchParams.UserId);
        }

        /*if (searchParams.CompletedStatus != null)
        {
            result = result.Where(t => t.IsCompleted == searchParams.CompletedStatus);
        }*/

        if (!string.IsNullOrEmpty(searchParams.TitleContains))
        {
            result = result.Where(t =>
                t.Title.Contains(searchParams.TitleContains, StringComparison.OrdinalIgnoreCase));
        }
        
        /*if (!string.IsNullOrEmpty(searchParams.BodyContains))
        {
            result = result.Where(t =>
                t.Title.Contains(searchParams.BodyContains, StringComparison.OrdinalIgnoreCase));
        }*/

        return Task.FromResult(result);
    }

    public Task<Post?> GetByIdAsync(int postId)
    {
        Post? existing = context.Todos.FirstOrDefault(t => t.Id == postId);
        return Task.FromResult(existing);
    }

    public Task UpdateAsync(Post dto)
    {
        Post? existing = context.Todos.FirstOrDefault(post => post.Id == dto.Id);
        if (existing == null)
        {
            throw new Exception($"Todo with id {dto.Id} does not exist!");
        }

        context.Todos.Remove(existing);
        context.Todos.Add(dto);
        
        context.SaveChanges();
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post? existing = context.Todos.FirstOrDefault(todo => todo.Id == id);
        if (existing == null)
        {
            throw new Exception($"Post with id {id} does not exist!");
        }

        context.Todos.Remove(existing); 
        context.SaveChanges();

        return Task.CompletedTask;
    }
}