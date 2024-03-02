using Application.Dto;
using Application.Interfaces;
using Azure;
using Domain.Entities;
using Microsoft.AspNet.OData;
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

    [SwaggerOperation(Summary = "Retrives all posts")]
    [EnableQuery]
    [HttpGet("[action]")]
    public async Task<IActionResult> Get()
    {
        var posts = await _postService.GetAllPostAsync();
        return Ok(posts);
    }

    [SwaggerOperation(Summary = "Retrieves a specific post by unique Id")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var post = await _postService.GetPostByIdAsync(id);
        if (post == null)
        {
            return NotFound(id);
        }

        return Ok(post);
    }

    [SwaggerOperation(Summary = "Create a new post")]
    [HttpPost]
    public async Task<IActionResult> Create(CreatePostDto newPost)
    {
        var post = await _postService.AddNewPostAsync(newPost);
        return Created($"api/posts/{post.Id}", post);
    }

    [SwaggerOperation(Summary = "Update a exsisting post")]
    [HttpPut]
    public async Task<IActionResult> Update(UpdatePostDto updatePost)
    {
        await _postService.UpdatePostAsync(updatePost);
        return NoContent();
    }

    [SwaggerOperation(Summary = "Delete a specific post")]
    [HttpDelete("Id")]
    public async Task<IActionResult> Delete(int id)
    {
        await _postService.DeletePostAsync(id);
        return NoContent();
    }

    [SwaggerOperation(Summary = "Searching specific title")]
    [HttpGet("Search/{title}")]
    public async Task<IActionResult> SearachingPostAsync(string title)
    {
        var searchingPost = await _postService.SearachingPostAsync(title);
        return Ok(searchingPost);
    }
}


