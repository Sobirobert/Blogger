
using Application.Dto;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace UnitTests.Services;



public class PostServiceTests
{
    [Fact]
    public async Task Add_Post_Async_Should_Invoke_Add_Async_On_Post_Repository()
    {
        // Arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<PostService>>();

        var postService = new PostService(postRepositoryMock.Object, mapperMock.Object, loggerMock.Object);

        var postDto = new CreatePostDto()
        {
            Title = "Title 1",
            Content = "Content 1"
        };

        mapperMock.Setup(x => x.Map<Post>(postDto)).Returns(new Post() { Title = postDto.Title, Content = postDto.Content });

        // Act
        await postService.AddNewPostAsync(postDto, "290c9f17-f71a-443a-99c4-256731cf870f");

        // Assert
        postRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Post>()), Times.Once);

    }

    [Fact]
    public async Task When_Invoking_Get_Post_Async_It_Should_Invoke_Get_Async_On_Post_Repository()
    {
        // Arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<PostService>>();

        var postService = new PostService(postRepositoryMock.Object, mapperMock.Object, loggerMock.Object);

        var post = new Post(1, "Title 1", "Content 1");
        var postDto = new PostDto()
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content
        };

        mapperMock.Setup(x => x.Map<Post>(postDto)).Returns(post);
        postRepositoryMock.Setup(x => x.GetByIdAsync(post.Id)).ReturnsAsync(post);

        // Act
        var existingPostDto = await postService.GetPostByIdAsync(post.Id);

        // Assert
        postRepositoryMock.Verify(x => x.GetByIdAsync(post.Id), Times.Once);
        postDto.Should().NotBeNull();
        postDto.Title.Should().NotBeNull();
        postDto.Title.Should().BeEquivalentTo(post.Title);
        postDto.Content.Should().NotBeNull();
        postDto.Content.Should().BeEquivalentTo(post.Content);
    }

    [Fact]
    public async Task UpdatePostAsyncShouldInvokeUpdateAsyncOnPostRepository()
    {
        // Arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<PostService>>();

        var postService = new PostService(postRepositoryMock.Object, mapperMock.Object, loggerMock.Object);

        var postDto = new UpdatePostDto()
        {
            Id = 1,
            Content = "Content 1"
        };

        var post = new Post()
        {
            Id = postDto.Id,
            Content = postDto.Content
        };

        mapperMock.Setup(x => x.Map<Post>(postDto)).Returns(post);
        postRepositoryMock.Setup(x => x.GetByIdAsync(post.Id)).ReturnsAsync(post);

        // Act
        await postService.UpdatePostAsync(postDto);

        // Assert
        postRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Post>()), Times.Once);
        postDto.Should().NotBeNull();
        postDto.Id.Should().Be(post.Id);
        postDto.Content.Should().NotBeNull();
        postDto.Content.Should().BeEquivalentTo(post.Content);
    }

    [Fact]
    public async Task DeletePostAsyncShouldInvokeDeleteAsyncOnPostRepository()
    {
        // Arrange
        var postRepositoryMock = new Mock<IPostRepository>();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<PostService>>();

        var postService = new PostService(postRepositoryMock.Object, mapperMock.Object, loggerMock.Object);

        var post = new Post()
        {
            Id = 1,
            Title = "Title 1",
            Content = "Content 1"
        };

        postRepositoryMock.Setup(x => x.GetByIdAsync(post.Id)).ReturnsAsync(post);

        // Act
        await postService.DeletePostAsync(post.Id);

        // Assert
        postRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Post>()), Times.Once);
        post.Should().NotBeNull();
        post.Id.Should().Be(1);
        post.Title.Should().NotBeNull();
        post.Title.Should().BeEquivalentTo("Title 1");
        post.Content.Should().NotBeNull();
        post.Content.Should().BeEquivalentTo("Content 1");
    }
}

