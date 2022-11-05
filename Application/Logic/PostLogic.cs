using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace Application.Logic;

public class PostLogic : IPostLogic
{
    private  readonly IPostDao _postDao;
    private  readonly IUserDao userDao;

    public PostLogic(IPostDao postDao, IUserDao userDao)
    {
        this._postDao = postDao;
        this.userDao = userDao;
    }

    public async Task<Post> CreateAsync(PostCreationDto dto)
    {
        User? user = await userDao.GetByIdAsync(dto.OwnerId);
        if (user == null)
        {
            throw new Exception($"User with id {dto.OwnerId} was not found.");
        }

        ValidateTodo(dto);
        Post post = new Post(user, dto.Title, dto.Body);
        Post created = await _postDao.CreateAsync(post);
        return created;
    }

    public Task<IEnumerable<Post>> GetAsync(SearchPostParametersDto searchParameters)
    {
        return _postDao.GetAsync(searchParameters);
    }

    private void ValidateTodo(PostCreationDto dto)
    {
        if (string.IsNullOrEmpty(dto.Title)) throw new Exception("Title cannot be empty.");
        if (string.IsNullOrEmpty(dto.Body)) throw new Exception("Body cannot be empty.");
        // other validation stuff
    }

    public  async Task UpdateAsync(PostUpdateDto dto)
    {
        Post? existing = await _postDao.GetByIdAsync(dto.Id);

        if (existing == null)
        {
            throw new Exception($"Todo with ID {dto.Id} not found!");
        }

        User? user = null;
        if (dto.OwnerId != null)
        {
            user = await userDao.GetByIdAsync((int)dto.OwnerId);
            if (user == null)
            {
                throw new Exception($"User with id {dto.OwnerId} was not found.");
            }
        }
/*
        if (dto.IsCompleted != null && existing.IsCompleted && !(bool)dto.IsCompleted)
        {
            throw new Exception("Cannot un-complete a completed Todo");
        }
        */

        User userToUse = user ?? existing.Owner;
        string titleToUse = dto.Title ?? existing.Title;
        string bodyToUse = dto.Body ?? existing.Body;
     //   bool completedToUse = dto.IsCompleted ?? existing.IsCompleted;
        
        Post updated = new (userToUse, titleToUse,bodyToUse)
        {
        //    IsCompleted = completedToUse,
            Id = existing.Id,
        };

        ValidateTodo(updated);

        await _postDao.UpdateAsync(updated);
    }

    public  async Task DeleteAsync(int id)
    {
        Post? todo = await _postDao.GetByIdAsync(id);
        if (todo == null)
        {
            throw new Exception($"Todo with ID {id} was not found!");
        }
/*
        if (!todo.IsCompleted)
        {
            throw new Exception("Cannot delete un-completed Todo!");
        }
        */

        await _postDao.DeleteAsync(id);
    }

    public  async Task<PostBasicDto> GetByIdAsync(int id)
    {
        Post? todo = await _postDao.GetByIdAsync(id);
        if (todo == null)
        {
            throw new Exception($"Todo with id {id} not found");
        }

        return new PostBasicDto(todo.Id, todo.Owner.UserName, todo.Title, todo.Body); //,todo.IsCompleted);
    }
    
    private  void ValidateTodo(Post dto)
    {
        
        if (string.IsNullOrEmpty(dto.Title)) throw new Exception("Title cannot be empty.");
        if (string.IsNullOrEmpty(dto.Body)) throw new Exception("Body cannot be empty.");
        // other validation stuff
    }
}