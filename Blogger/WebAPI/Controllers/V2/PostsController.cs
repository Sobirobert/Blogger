﻿using Application.Dto.Cosmos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPI.Controllers.V2;

[ApiExplorerSettings(IgnoreApi = true)]
[ApiVersion("2.0")]
//[Route("api/{v: apiVersion}/[controller]")]
[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly ICosmosPostService _postService;

    public PostsController(ICosmosPostService postService)
    {
        _postService = postService;
    }

    [SwaggerOperation(Summary = "Retrieves all posts")]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var posts = await _postService.GetAllPostAsync();
        return Ok(posts);
    }

    [SwaggerOperation(Summary = "Retrieves a specific post by unique Id")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
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
    public async Task<IActionResult> Create(CreateCosmosPostDto newPost)
    {
        var post = await _postService.AddNewPostAsync(newPost);
        return Created($"api/posts/{post.Id}", post);
    }

    [SwaggerOperation(Summary = "Update a exsisting post")]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateCosmosPostDto updatePost)
    {
        await _postService.UpdatePostAsync(updatePost);
        return NoContent();
    }

    [SwaggerOperation(Summary = "Delete a specific post")]
    [HttpDelete("Id")]
    public async Task<IActionResult> Delete(string id)
    {
        await _postService.DeletePostAsync(id);
        return NoContent();
    }
}