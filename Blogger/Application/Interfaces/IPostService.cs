﻿using Application.Dto;

namespace Application.Interfaces;

public interface IPostService
{
    IEnumerable<PostDto> GetAllPosts();

    PostDto GetPostByID(int id);

    PostDto AddNewPost(CreatePostDto newPost);

    void UpdatePost(UpdatePostDto updatePostDto);

    void DeletePost(int id);

    PostDto SearchTitle(string searchingTitle);
}
