﻿using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;
    public PostService(IPostRepository postRepository, IMapper mapper) 
    { 
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public PostDto AddNewPost(CreatePostDto newPost)
    {
        if (string.IsNullOrEmpty(newPost.Title))
        {
            throw new Exception("Post can not have an empty title");
        }
        var post = _mapper.Map<Post>(newPost);
        _postRepository.Add(post);
        return _mapper.Map<PostDto>(post);
    }

    public IEnumerable<PostDto> GetAllPosts()
    {
        var posts = _postRepository.GetAll();
        return _mapper.Map<IEnumerable<PostDto>>(posts);
    }

    public PostDto GetPostByID(int id)
    {
        var post = _postRepository.GetById(id);
        return _mapper.Map<PostDto>(post);
    }
}
