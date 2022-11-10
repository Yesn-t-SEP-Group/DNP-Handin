﻿using Application.DaoInterfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfcDataAccess.DAOs;

public class PostEfcDao : IPostDao
{
    
    private  TodoContext context;

    public PostEfcDao(TodoContext context)
    {
        this.context = context;
    }

    public async Task<Post> CreateAsync(Post todo)
    {
        EntityEntry<Post> added = await context.Posts.AddAsync(todo);
        await context.SaveChangesAsync();
        return added.Entity;
    }

    public async Task<IEnumerable<Post>> GetAsync(SearchPostParametersDto searchParams)
    {
        IQueryable<Post> query = context.Posts.Include(todo => todo.Owner).AsQueryable();
        
        if (!string.IsNullOrEmpty(searchParams.Username))
        {
            // we know username is unique, so just fetch the first
            query = query.Where(todo =>
                todo.Owner.UserName.ToLower().Equals(searchParams.Username.ToLower()));
        }
        
        if (searchParams.UserId != null)
        {
            query = query.Where(t => t.Owner.Id == searchParams.UserId);
        }

        if (!string.IsNullOrEmpty(searchParams.TitleContains))
        {
            query = query.Where(t =>
                t.Title.ToLower().Contains(searchParams.TitleContains.ToLower()));
        }

        List<Post> result = await query.ToListAsync();
        return result;
    }

    public async Task UpdateAsync(Post todo)
    {
        /*
        context.ChangeTracker.Clear();
        context.Posts.Update(todo);
        await context.SaveChangesAsync();
        */
        using (var db = new TodoContext())
        {
            var result = context.Posts.SingleOrDefault(p => p.Id == todo.Id);
            if (result != null)
            {
                result = todo;
                context.SaveChanges();
            }
        }
        
    }

    public async Task<Post?> GetByIdAsync(int todoId)
    {
        Post? found = await context.Posts.FindAsync(todoId);
        return found;
    }

    public async Task DeleteAsync(int id)
    {
        Post? existing = await GetByIdAsync(id);
        if (existing == null)
        {
            throw new Exception($"Todo with id {id} not found");
        }

        context.Posts.Remove(existing);
        await context.SaveChangesAsync();
    }
}