using Domain.DTOs;
using Domain.Models;

namespace EfcDataAccess.DAOs;

public interface IPostDao
{
    Task<Post> CreateAsync(Post post);
    Task<IEnumerable<Post>> GetAsync(SearchPostParametersDto searchParameters);
    Task<Post?> GetByIdAsync(int postId);
    Task UpdateAsync(Post dto);
    Task DeleteAsync(int id);
}