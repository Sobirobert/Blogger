﻿using Application.Dto;

namespace Application.Interfaces;

public interface IPostService
{
    Task<IEnumerable<PostDto>> GetAllPostsAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy);
    Task<int> GetAllPostsCountAsync(string filterBy);
    Task<PostDto> GetPostByIdAsync(int id);
    Task<PostDto> AddNewPostAsync(CreatePostDto newPost);
    Task UpdatePostAsync(UpdatePostDto updatePost);
    Task DeletePostAsync(int id);
}
