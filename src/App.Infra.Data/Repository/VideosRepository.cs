using App.Domain.Models;
using App.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Domain.Interfaces
{
    public class VideosRepository : IVideosRepository
    {
        private readonly MySQLContextFactory _contextFactory;

        public VideosRepository(MySQLContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task Update(VideoBD Video)
        {
            using (var _dbContext = _contextFactory.CreateDbContext())
            {
                _dbContext.Videos.Update(Video);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<VideoBD> GetVideoById(int id)
        {
            using (var _dbContext = _contextFactory.CreateDbContext())
            {
                return await _dbContext.Videos.FirstOrDefaultAsync(c => c.Id == id);
            }
        }
    }





}
