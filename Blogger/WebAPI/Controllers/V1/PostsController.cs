using Application.Dto;
using Application.Interfaces;
using Azure;
using Cosmonaut.Extensions;
using Cosmonaut.Response;
using Domain.Entities;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Filters;
using WebAPI.Helpers;
using WebAPI.Wrappers;

namespace WebAPI.Controllers.V1;


[Route("api/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [SwaggerOperation(Summary = "Retrieves sort fields")]
    [HttpGet("[action]")]
    public IActionResult GetSortFields()
    {
        return Ok(SortingHelper.GetSortFields().Select(x => x.Key));
    }

    [SwaggerOperation(Summary = "Retrieves all posts")]
    [EnableQuery]
    [HttpGet("[action]")]
    public async Task<IActionResult> Get([FromQuery] PaginationFilter paginationFilter, [FromQuery] SortingFilter sortingFilter, [FromQuery] string filterBy = "")
    {
        var validPaginationFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
        var validSortingFilter = new SortingFilter(sortingFilter.SortField, sortingFilter.Ascending);

        var posts = await _postService.GetAllPostsAsync(validPaginationFilter.PageNumber, validPaginationFilter.PageSize,
                                                       validSortingFilter.SortField, validSortingFilter.Ascending, filterBy);
        var totalRecords = await _postService.GetAllPostsCountAsync(filterBy);

        return Ok(PaginationHelper.CreatePageResponse(posts, validPaginationFilter, totalRecords));
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

        return Ok(new Wrappers.Response<PostDto>(post));
    }

    [SwaggerOperation(Summary = "Create a new post")]
    [HttpPost]
    public async Task<IActionResult> Create(CreatePostDto newPost)
    {
        var post = await _postService.AddNewPostAsync(newPost);
        return Created($"api/posts/{post.Id}", new Wrappers.Response<PostDto>(post));
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
}


