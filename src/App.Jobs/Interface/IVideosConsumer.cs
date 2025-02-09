using System;
using System.Threading.Tasks;

namespace App.Jobs
{
    public interface IVideosConsumer : IDisposable
    {
        public Task<bool> ConsumirVideo();

    }
}
