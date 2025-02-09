using App.Domain.Models;
using System.Threading.Tasks;

namespace App.Domain.Interfaces
{
    public interface IVideosRepository
    {
        Task Update(VideoBD Video);
        Task<VideoBD> GetVideoById(int id);
    }
}
