using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using App.Application.Interfaces;
using App.Application.Util;
using App.Domain.Interfaces;
using App.Domain.Models;
using App.Domain.Models.External;
using App.Test.MockObjects;
using Application.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace App.Application.Tests.Services
{
    public class VideosServiceTests
    {
        private readonly Mock<IVideosRepository> _repositoryMock;
        private readonly Mock<IAwsService> _awsMock;
        private readonly AwsConfig _awsConfig;
        private readonly IVideosService _videosService;
        

        public VideosServiceTests()
        {
            _repositoryMock = new Mock<IVideosRepository>();
            _awsConfig = new AwsConfig { S3Access = "access", S3Secret = "secret" };
            _awsMock = new Mock<IAwsService>();
            _videosService = new VideosService(_repositoryMock.Object, _awsMock.Object);
            

            //_awsMock = new Mock<IAmazonS3>(FallbackCredentialsFactory.GetCredentials(true)) ;


        }

        [Fact]
        public async Task Process_Video_OK()
        {
            // Arrange

            //mock bd
            _repositoryMock.Setup(r => r.GetVideoById(It.IsAny<int>())).ReturnsAsync(new VideoBD("teste"));
            _repositoryMock.Setup(r => r.Update(It.IsAny<VideoBD>())).Returns(Task.CompletedTask);
            _awsMock.Setup(s3 => s3.GetVideoFromS3(It.IsAny<int>())).ReturnsAsync(new MockBase64().Base64());
            _awsMock.Setup(s3 => s3.SaveZipToS3(It.IsAny<VideoBD>(), It.IsAny<byte[]>())).Returns(Task.CompletedTask);



            // Act
            await _videosService.ConsumerVideo(1);

            // Assert
            _repositoryMock.Verify(r => r.Update(It.Is<VideoBD>(v => v.Status == 2)), Times.Once);


        }

        [Fact]
        public async Task Process_Video_Exception()
        {
            // Arrange

            //mock bd
            _repositoryMock.Setup(r => r.GetVideoById(It.IsAny<int>())).ReturnsAsync(new VideoBD("teste"));
            _repositoryMock.Setup(r => r.Update(It.IsAny<VideoBD>())).Returns(Task.CompletedTask);
            
            
            _awsMock.Setup(s3 => s3.GetVideoFromS3(It.IsAny<int>())).ReturnsAsync(new MockBase64().Base64());
            
            _awsMock.Setup(s3 => s3.SaveZipToS3(It.IsAny<VideoBD>(), It.IsAny<byte[]>())).ThrowsAsync(new Exception("Error saving zip to S3"));


            // Act
            await _videosService.ConsumerVideo(1);

            // Assert
            _repositoryMock.Verify(r => r.Update(It.Is<VideoBD>(v => v.Status == 3)), Times.Once);

        }


        [Fact]
        public async Task CaptureScreenshots_ShouldReturnListOfScreenshots()
        {

            // Act
            var result = VideoUtil.CaptureScreenshots(new MockBase64().Base64(), 1);

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task CreateZipFile_ShouldReturnZipFile()
        {
            // Arrange
            var screenshots = new List<byte[]> { new byte[] { 1, 2, 3 } };

            // Act
            var result = VideoUtil.CreateZipFile(screenshots);

            // Assert
            Assert.NotEmpty(result);
        }

    }
}
