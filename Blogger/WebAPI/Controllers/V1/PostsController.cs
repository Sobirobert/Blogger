using Application.Dto;
using Application.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WebAPI.Attributes;
using WebAPI.Cache;
using WebAPI.Filters;
using WebAPI.Helpers;
using WebAPI.Wrappers;

namespace WebAPI.Controllers.V1;

[Route("api/[controller]")]
[ApiVersion("1.0")]
[Authorize()]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger _logger;



    public PostsController(IPostService postService, IMemoryCache memoryCache, ILogger<PostsController> logger)
    {
        _postService = postService;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    [SwaggerOperation(Summary = "Retrieves sort fields")]
    [HttpGet("[action]")]
    public IActionResult GetSortFields()
    {
        return Ok(SortingHelper.GetSortFields().Select(x => x.Key));
    }

    [SwaggerOperation(Summary = "Retrieves all posts")]
    [EnableQuery]
    [Cached(600)]
    [Authorize(Roles = UserRoles.Admin)]
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


    //[SwaggerOperation(Summary = "Retrieves all posts witch cache")]
    //[EnableQuery]
    //[Authorize(Roles = UserRoles.Admin)]
    //[HttpGet("[action]")]
    //public async Task<IActionResult> GetWithCache([FromQuery] PaginationFilter paginationFilter, [FromQuery] SortingFilter sortingFilter, [FromQuery] string filterBy = "")
    //{
    //    var posts = _memoryCache.Get<IEnumerable<PostDto>>("posts");

    //    var validPaginationFilter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
    //    var validSortingFilter = new SortingFilter(sortingFilter.SortField, sortingFilter.Ascending);
    //    if (posts == null)
    //    {                                                                         // pobieranie postów z cache służy nie do pobierania wszystkich, tylko postów z określonego czasu
    //        _logger.LogInformation("Fetching from service.");
    //        posts = await _postService.GetAllPostsAsync(validPaginationFilter.PageNumber, validPaginationFilter.PageSize,
    //                                                   validSortingFilter.SortField, validSortingFilter.Ascending, filterBy);
    //        _memoryCache.Set("posts", posts, TimeSpan.FromMinutes(1));
    //    }
    //    else
    //    {
    //        _logger.LogInformation("Fetching from cache.");
    //    }

    //    var totalRecords = await _postService.GetAllPostsCountAsync(filterBy);
    //    return Ok(PaginationHelper.CreatePageResponse(posts, validPaginationFilter, totalRecords));
    //}

    //[SwaggerOperation(Summary = "Retrieves all posts")]
    //[Authorize(Roles = UserRoles.Admin)]
    //[HttpGet("[action]")]
    //public IQueryable<PostDto> GetAll()
    //{
    //    var posts = _memoryCache.Get<IQueryable<PostDto>>("posts");
    //    if (posts == null)
    //    {                                                                         // pobieranie postów z cache służy nie do pobierania wszystkich, tylko postów z określonego czasu
    //        _logger.LogInformation("Fetching from service.");
    //        posts = _postService.GetAllPostsAsync();
    //        _memoryCache.Set("posts", posts, TimeSpan.FromMinutes(1));
    //    }
    //    else
    //    {
    //        _logger.LogInformation("Fetching from cache.");
    //    }

    //    return posts;
    //}

    [SwaggerOperation(Summary = "Retrieves a specific post by unique Id")]
    [AllowAnonymous]
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

    [ValidateFilter]
    [SwaggerOperation(Summary = "Create a new post")]
    [Authorize(Roles = UserRoles.User)]
    [HttpPost]
    public async Task<IActionResult> Create(CreatePostDto newPost)
    {
        //var validator = new CreatePostDtoValidator();
        //var result = validator.Validate(newPost);
        //if (!result.IsValid)
        //{
        //    return BadRequest(new Response<bool>
        //    {
        //        Succeeded = false,
        //        Message = "Something went wrong.",
        //        Errors = result.Errors.Select(x => x.ErrorMessage)
        //    });
        //}

        var post = await _postService.AddNewPostAsync(newPost, User.FindFirstValue(ClaimTypes.NameIdentifier));
        return Created($"api/posts/{post.Id}", new Wrappers.Response<PostDto>(post));
    }

    [SwaggerOperation(Summary = "Update a existing post")]
    [Authorize(Roles = UserRoles.User)]
    [HttpPut]
    public async Task<IActionResult> Update(UpdatePostDto updatePost)
    {
        var userOwnsPost = await _postService.UserOwnsPostAsync(updatePost.Id, User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (!userOwnsPost)
        {
            return BadRequest(new Response(false, "You do not own this post."));
        }

        await _postService.UpdatePostAsync(updatePost);
        return NoContent();
    }

    [SwaggerOperation(Summary = "Delete a specific post")]
    [Authorize(Roles = UserRoles.AdminOrUser)]
    [HttpDelete("Id")]
    public async Task<IActionResult> Delete(int id)
    {
        var userOwnsPost = await _postService.UserOwnsPostAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
        var isAdmin = User.FindFirstValue(ClaimTypes.Role).Contains(UserRoles.Admin);
        if (!isAdmin && !userOwnsPost)
        {
            return BadRequest(new Response(false, "You do not own this post."));
        }
        await _postService.DeletePostAsync(id);
        return NoContent();
    }
}