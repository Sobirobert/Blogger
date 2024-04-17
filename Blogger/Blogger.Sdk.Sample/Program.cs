using System;
using Blogger.Contracts.Request;
using System.Threading.Tasks;
using Blogger.Sdk;
using Refit;

namespace Blogger.Sdk.Sample;

public class Program
{
    static async Task Main(string[] args)
    {
        var cachedToken = string.Empty;

        var identityApi = RestService.For<IIdentityApi>("https://localhost:44310/");
        var bloggerApi = RestService.For<IBloggerApi>("https://localhost:44310/", new RefitSettings
        {
            AuthorizationHeaderValueGetter = (request, cancellationToken) => Task.FromResult(cachedToken)
            //AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
        });

        var register = await identityApi.RegisterAsync(new RegisterModel()
        {
            Email = "sdkacccount@gmail.com",
            Username = "sdkaacccount",
            Password = "Pa$$wOrd123!"

        });

        var login = await identityApi.LoginAsync(new LoginModel()
        {
            Username = "sdkaacccount",
            Password = "Pa$$wOrd123!"
        });

        cachedToken = login.Content.Token;

        var createdPost = await bloggerApi.CreatePostAsync(new CreatePostDto
        {
            Title = "Nowy post SDK",
            Content = "Taki testowy post"
        });

        var retrievedPost = await bloggerApi.GetPostAsync(createdPost.Content.Data.Id);

        await bloggerApi.UpdatePostAsync(new UpdatePostDto
        {
            Id = retrievedPost.Content.Data.Id,
            Content = "Nowa treść posta SDK"
        });

        await bloggerApi.DeletePostAsync(retrievedPost.Content.Data.Id);
    }
}

