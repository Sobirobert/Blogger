using Application.Dto;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Reflection;
using WebAPI;
using WebAPI.Wrappers;
using Xunit;

namespace EndToEndTests.Controllers;

public class PostsControllerTests
{
    private readonly HttpClient _client;
    private readonly TestServer _server;
    public PostsControllerTests()
    {
        // Arrange
        //var webAppFactory = new WebApplicationFactory<Program>();
        //_httpClient = webAppFactory.CreateDefaultClient();

        var projectDir = Helper.GetProjectPath("", typeof(Startup).GetTypeInfo().Assembly); //Testy integracyjne 
        _server = new TestServer(new WebHostBuilder()
            .UseEnvironment("Development")
            .UseContentRoot(projectDir)
            .UseConfiguration(new ConfigurationBuilder()
                .SetBasePath(projectDir)
                .AddJsonFile("appsettings.json")
                .Build()
            )
            .UseStartup<Startup>());
        _client = _server.CreateClient();
    }

    [Fact]
    public async Task FetchingPostsShouldReturnNotEmptyCollection()
    {
        // Act
        var response = await _client.GetAsync(@"api/Posts");
        var content = await response.Content.ReadAsStringAsync();
        var pagedResponse = JsonConvert.DeserializeObject<PagedResponse<IEnumerable<PostDto>>>(content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);// do tego porzeba nugeta fluent 
        pagedResponse?.Data.Should().NotBeEmpty();
    }

    [Fact]
    public async Task FetchingRequestPostIdShouldReturnOnlyOneResult()
    {
        // Acts
        var response = await _client.GetAsync(@"api/Posts/1");
        var content = await response.Content.ReadAsStringAsync();
        var post = JsonConvert.DeserializeObject<Response<PostDto>>(content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        post?.Data.Should().NotBeNull();
        // should return just one post with Id 1
        post?.Data?.Id.Should().Be(1);
    }
}
