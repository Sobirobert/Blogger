using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Interfaces;

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
