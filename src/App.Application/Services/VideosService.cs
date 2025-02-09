using App.Application.Interfaces;
using App.Application.Util;
using App.Domain.Interfaces;
using System;
using System.Threading.Tasks;


namespace Application.Services
{
    public class VideosService : IVideosService
    {
        private readonly IVideosRepository _repository;
        private readonly IAwsService _awsService;

        public VideosService(IVideosRepository repository, IAwsService awsService)
        {
            _repository = repository;
            _awsService = awsService;
        }

        public async Task ConsumerVideo(int id)
        {
            //try
            //{

            var itemBD = await _repository.GetVideoById(id);

            try
            {
                var video = await _awsService.GetVideoFromS3(itemBD.Id);
                var screenshots = VideoUtil.CaptureScreenshots(video, itemBD.Id);
                var zipFile = VideoUtil.CreateZipFile(screenshots);
                await _awsService.SaveZipToS3(itemBD, zipFile);
                itemBD.Status = 2;
                //processado

            }
            catch (Exception ex)
            {
                itemBD.Status = 3;
                //TODO send Email

            }
            await _repository.Update(itemBD);

        }

    



      


        public void Dispose()
        {
            _awsService.Dispose();
            
        }
    }
}