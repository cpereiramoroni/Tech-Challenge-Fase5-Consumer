using Amazon.S3;
using Amazon.S3.Model;
using App.Application.Interfaces;
using App.Domain.Models;
using App.Domain.Models.External;
using System;
using System.IO;
using System.Threading.Tasks;


namespace Application.Services
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AwsService : IAwsService
    {
        
        private readonly AwsConfig _awsconfig;
        private IAmazonS3 _S3Client;

        public AwsService( AwsConfig awsconfig)
        {
            
            _awsconfig = awsconfig;
            _S3Client = new AmazonS3Client(
                _awsconfig.S3Access,
                _awsconfig.S3Secret,
                Amazon.RegionEndpoint.USEast1); 
        }
        public async Task<string> GetVideoFromS3(int id)
        {
          

            var getRequest = new GetObjectRequest
            {
                BucketName = "fiapvideo",
                Key = $"{id}.mp4"
            };

            using var response = await _S3Client.GetObjectAsync(getRequest);
            using var responseStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(responseStream);
            responseStream.Position = 0; // Reset stream position to the beginning

            var base64String = Convert.ToBase64String(responseStream.ToArray());
            return base64String;
        }

        public async Task SaveZipToS3(VideoBD itemBD, byte[] zipFile)
        {
            // 2º Upload video to AWS S3
         

            var putRequest = new PutObjectRequest
            {
                BucketName = "fiapvideo",
                Key = $"{itemBD.Id}.zip",
                InputStream = new MemoryStream(zipFile),
                ContentType = "application/zip"
            };
            await _S3Client.PutObjectAsync(putRequest);
        }


        public void Dispose()
        {
            _S3Client.Dispose();
            
        }
    }
}