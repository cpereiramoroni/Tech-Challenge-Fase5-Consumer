using System;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface IVideosService: IDisposable
    {
        Task ConsumerVideo(int id);
        
    }
}
