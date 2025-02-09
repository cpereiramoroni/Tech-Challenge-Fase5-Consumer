using App.Domain.Interfaces;
using App.Domain.Models;
using App.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class VideosRepositoryTests
{
    private readonly Mock<MySQLContextFactory> _mockContextFactory;
    private readonly Mock<MySQLContext> _mockDbContext;
    private readonly VideosRepository _videosRepository;
    public VideosRepositoryTests()
    {
        var myConfiguration = new Dictionary<string, string>
            {
                {"ConnectionVideos", "Server=testeLocal;Database=testeMr;User=Moroni;Password=1234567;"},

            };

        var configuration = new ConfigurationBuilder()
         .AddInMemoryCollection(myConfiguration)
         .Build();

        var _dbContextOptions = new DbContextOptionsBuilder<MySQLContext>()
             .UseInMemoryDatabase(databaseName: "TestDatabase")
             .Options;

        _mockDbContext = new Mock<MySQLContext>(_dbContextOptions);
        
        _mockContextFactory = new Mock<MySQLContextFactory>(configuration);


        _mockContextFactory.Setup(f => f.CreateDbContext()).Returns(_mockDbContext.Object);
        _videosRepository = new VideosRepository(_mockContextFactory.Object);

        _mockDbContext.Setup(s => s.Videos).Returns(MockDbSet(new List<VideoBD>
        {
            new VideoBD("Video 1") { Id = 1, Status = 1, DataCadastro = DateTime.Now.AddDays(-10) },
            new VideoBD("Video 2") { Id = 2, Status = 2, DataCadastro = DateTime.Now.AddDays(-5) },
            new VideoBD("Video 3") { Id = 3, Status = 1, DataCadastro = DateTime.Now.AddDays(-1) }
        }));
        _mockDbContext.Setup(s => s.Videos).Returns(MockDbSet(new List<VideoBD>()));
    }
    private DbSet<T> MockDbSet<T>(List<T> list) where T : class
    {
        var queryable = list.AsQueryable();
        var dbSet = new Mock<DbSet<T>>();
        dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
        return dbSet.Object;
    }


    [Fact]
    public async Task Update_ShouldUpdateVideo()
    {
        // Arrange

        var video = new VideoBD("Test Video") { Id = 1 };

        _mockDbContext.Setup(s => s.Update(It.IsAny<VideoBD>()));
        _mockDbContext.Setup(s => s.SaveChanges());
        // Act
        await _videosRepository.Update(video);

        // Assert
        _mockDbContext.Verify(db => db.Videos.Update(video), Times.Once);
        _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
    }

   
    [Fact]
    public async Task GetVideoById_ShouldReturnVideo()
    {
        // Arrange
        var video = new VideoBD("Video 1") { Id = 1 };
        var videos = new List<VideoBD> { video }; // In-memory data

        _mockDbContext.Setup(db => db.Videos)
                      .ReturnsDbSet(videos); // Moq.EntityFrameworkCore handles async LINQ operations

        // Act
        var result = await _videosRepository.GetVideoById(1);

        // Assert
        Assert.Equal(video, result);
        _mockDbContext.Verify(db => db.Videos, Times.Once); // Ensure DbSet was accessed
    }


}
