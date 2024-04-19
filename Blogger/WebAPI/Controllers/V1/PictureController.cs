using Application.Dto;
using Application.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WebAPI.Wrappers;

namespace WebAPI.Controllers.V1;

[Route("api/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = UserRoles.User)]
[ApiController]
public class PictureController : ControllerBase
{
    private readonly IPictureService _pictureSerwice;
    private readonly IPostService _postService;

    public PictureController(IPictureService pictureService, IPostService postService)
    {
        _pictureSerwice = pictureService;
        _postService = postService;
    }

    [SwaggerOperation(Summary = "Retrieves a picture by uniqe post id")]
    [HttpGet("[action]/{postId}")]
    public async Task<IActionResult> GetByPostId(int postId)
    {
        var pictures = await _pictureSerwice.GetPicturesByPostIdAsync(postId);
        return Ok(new Response<IEnumerable<PictureDto>>(pictures));
    }

    [SwaggerOperation(Summary = "Retrieves a specific picture by unique id")]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var picture = await _pictureSerwice.GetPictureByIdAsync(id);
        if (picture == null)
        {
            return NotFound();
        }

        return Ok(new Response<PictureDto>(picture));
    }

    [SwaggerOperation(Summary = "Add a new picture to post")]
    [HttpPost("{postId}")]
    public async Task<IActionResult> AddToPostAsync(int postId, IFormFile file)
    {
        var post = await _postService.GetPostByIdAsync(postId);
        if (post == null)
        {
            return BadRequest(new Response(false, $"Post with id {postId} does not exist.")); //sprawdzanie czy post istnieje
        }

        var userOwner = await _postService.UserOwnsPostAsync(postId, User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (!userOwner)
        {
            return BadRequest(new Response(false, "You do not own this post.")); // sprawdzanie czy użytkownik ma dostęp do posta
        }

        var picture = await _pictureSerwice.AddPictureToPostAsync(postId, file); // dodanie zdjęcia do posta
        return Created($"api/pictures/{picture.Id}", new Response<PictureDto>(picture));
    }

    [SwaggerOperation(Summary = "Sets the main picture of the post")]
    [HttpPut("[action]/{postId}/{id}")]
    public async Task<IActionResult> SetMainPicture(int postId, int id)
    {
        var userOwnsPost = await _postService.UserOwnsPostAsync(postId, User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (!userOwnsPost)
        {
            return BadRequest(new Response(false, "You do not own this post."));
        }

        await _pictureSerwice.SetMainPicture(postId, id);
        return NoContent();
    }

    [SwaggerOperation(Summary = "Delete a specific picture")]
    [HttpDelete("{postId}/{id}")]
    public async Task<IActionResult> Delate(int id, int postId)
    {
        var userOwnsPost = await _postService.UserOwnsPostAsync(postId, User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (!userOwnsPost)
        {
            return BadRequest(new Response(false, "You do not own this post."));
        }

        await _pictureSerwice.DeletePictureAsync(id);
        return NoContent();
    }
}