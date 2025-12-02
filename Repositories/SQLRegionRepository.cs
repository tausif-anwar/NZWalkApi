using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepository(NZWalksDbContext DbContext)
        {
            dbContext = DbContext;
        }

        public DbContext DbContext { get; }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();    
            return region;

        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var region = await dbContext.Regions.FirstOrDefaultAsync(s=> s.Id == id);

            if (region == null)
            {   
                return null;
            }

            dbContext.Regions.Remove(region);
            await dbContext.SaveChangesAsync();

            return region;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            var region = await dbContext.Regions.FirstOrDefaultAsync(s => s.Id == id);

            if (region == null)
            {
                return null;
            }

            return region;

        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(s => s.Id == id);

            if (region == null)
            {
                return null;
            }

            regionDomain.Code = region.Code;
            regionDomain.Name = region.Name;
            regionDomain.RegionImageUrl = region.RegionImageUrl;

            await dbContext.SaveChangesAsync();

            return region;

        }
    }
}
