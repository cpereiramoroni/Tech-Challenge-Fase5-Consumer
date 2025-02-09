using App.Domain.Models;
using App.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xunit;

namespace App.Tests.Mappings
{
    public class VideosMapTests
    {
        [Fact]
        public void Configure_Should_SetPrimaryKey()
        {
            // Arrange
            var modelBuilder = new ModelBuilder();
            var builder = modelBuilder.Entity<VideoBD>();

            var videosMap = new VideosMap();

            // Act
            videosMap.Configure(builder);

            // Assert
            var key = builder.Metadata.FindPrimaryKey();
            Assert.NotNull(key);
            Assert.Contains(key.Properties, p => p.Name == "Id");
        }

        [Fact]
        public void Configure_Should_SetNomeProperty()
        {
            // Arrange
            var modelBuilder = new ModelBuilder();
            var builder = modelBuilder.Entity<VideoBD>();

            var videosMap = new VideosMap();

            // Act
            videosMap.Configure(builder);

            // Assert
            var property = builder.Metadata.FindProperty("Nome");
            Assert.NotNull(property);
            
            Assert.Equal(100, property.GetMaxLength());
        }

        [Fact]
        public void Configure_Should_SetDataCadastroProperty()
        {
            // Arrange
            var modelBuilder = new ModelBuilder();
            var builder = modelBuilder.Entity<VideoBD>();

            var videosMap = new VideosMap();

            // Act
            videosMap.Configure(builder);

            // Assert
            var property = builder.Metadata.FindProperty("DataCadastro");
            Assert.NotNull(property);
            
        }

        [Fact]
        public void Configure_Should_SetStatusProperty()
        {
            // Arrange
            var modelBuilder = new ModelBuilder();
            var builder = modelBuilder.Entity<VideoBD>();

            var videosMap = new VideosMap();

            // Act
            videosMap.Configure(builder);

            // Assert
            var property = builder.Metadata.FindProperty("Status");
            Assert.NotNull(property);
            
        }

        [Fact]
        public void Configure_Should_SetTableName()
        {
            // Arrange
            var modelBuilder = new ModelBuilder();
            var builder = modelBuilder.Entity<VideoBD>();

            var videosMap = new VideosMap();

            // Act
            videosMap.Configure(builder);

            // Assert
            Assert.Equal("Videos", builder.Metadata.GetTableName());
        }
    }
}
