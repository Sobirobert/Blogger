using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers.V1;

[Route("api/{v:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [SwaggerOperation(Summary = "Retrieves all posts")]
    [HttpGet]
    public IActionResult Get()
    {
        var posts = _postService.GetAllPosts();
        return Ok(posts);
    }

    [SwaggerOperation(Summary = "Retrieves a specific post by unique id")]
    [HttpGet("(id)")]
    public IActionResult Get(int id)
    {
        var posts = _postService.GetPostByID(id);
        if (posts == null)
        {
            return NotFound();
        }
        return Ok(posts);
    }

    [SwaggerOperation(Summary = "Created a new post")]
    [HttpPost]
    public IActionResult Create(CreatePostDto newPost)
    {
        var post = _postService.AddNewPost(newPost);
        return Created($"api/post/{post.Id}", post);
    }

    [SwaggerOperation(Summary = "Update a existing post")]
    [HttpPut]
    public IActionResult Update(UpdatePostDto updatePost)
    {
        _postService.UpdatePost(updatePost);
        return NoContent();
    }

    [SwaggerOperation(Summary = "Delete a specific post")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _postService.DeletePost(id);
        return NoContent();
    }

    [SwaggerOperation(Summary = "Searching specific title")]
    [HttpGet("Search/{title}")]
    public IActionResult SearachingPost(string title)
    {
        var searchingPost = _postService.SearchTitle(title);
        return Ok(searchingPost);
    }
}


