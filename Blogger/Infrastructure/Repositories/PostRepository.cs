﻿using Application.Dto;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.ExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly BloggerContext _context;

    public PostRepository(BloggerContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        return await _context.Posts.ToListAsync();
    }

    public async Task<Post> GetByIdAsync(int id)
    {
        return await _context.Posts.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Post> AddAsync(Post post)
    {
        //post.Created = DateTime.UtcNow;
        var createdPost = await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return createdPost.Entity;
    }

    public async Task UpdateAsync(Post post)
    {
        //post.LastModified = DateTime.UtcNow;
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Post post)
    {
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        await Task.CompletedTask;
    }
}
