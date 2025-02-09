using App.Domain.Interfaces;
using App.Domain.Models;
using App.Infra.Data.Context;
using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace App.Tests.Context
{
    public class MySQLContextTests
    {
        private readonly DbContextOptions<MySQLContext> _dbContextOptions;

        public MySQLContextTests()
        {
            // Configure In-Memory Database
            _dbContextOptions = new DbContextOptionsBuilder<MySQLContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public void MySQLContext_ShouldConfigureDbSet()
        {
            // Arrange & Act
            using var context = new MySQLContext(_dbContextOptions);

            // Assert
            Assert.NotNull(context.Videos);
        }

       
    }
}
