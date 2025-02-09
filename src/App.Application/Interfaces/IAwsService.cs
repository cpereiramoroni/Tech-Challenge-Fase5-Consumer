using App.Domain.Models;
using System;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface IAwsService : IDisposable
    {
        Task<string> GetVideoFromS3(int id);

        Task SaveZipToS3(VideoBD itemBD, byte[] zipFile);
    }
}
