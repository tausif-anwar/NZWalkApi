using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext DBContext;

        public SQLWalkRepository(NZWalksDbContext nzdbContext)
        {
            this.DBContext = nzdbContext;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await DBContext.Walks.AddAsync(walk);
            await DBContext.SaveChangesAsync();
            return walk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true)
        {
            var walks = DBContext.Walks.Include("Region").Include("Difficulty").AsQueryable();

            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {

                if (filterOn.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(s=>s.Name.Contains(filterQuery));    
                }
            }

            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    walks = isAscending == false ? walks.OrderByDescending(x => x.Name) : walks.OrderBy(x => x.Name);
                else if (sortBy.Equals("LenghtInKm", StringComparison.OrdinalIgnoreCase))
                    walks = isAscending == false ? walks.OrderByDescending(x => x.LenghtInKm) : walks.OrderBy(x => x.Name);
            }

            return await walks.ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await DBContext.Walks.Include("Region").Include("Difficulty").FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var walkDomain = await DBContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (walkDomain == null)
            {
                return null; 
            }

            walkDomain.Name = walk.Name;
            walkDomain.Description = walk.Description;
            walkDomain.LenghtInKm = walk.LenghtInKm;
            walkDomain.WalkImageUrl = walk.WalkImageUrl;

            walkDomain.RegionId = walk.RegionId;
            walkDomain.DifficultyId = walk.DifficultyId;

            await DBContext.SaveChangesAsync();

            return walkDomain;

        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var walkDomain = await DBContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (walkDomain == null)
            {
                return null;
            }

            DBContext.Walks.Remove(walkDomain);
            await DBContext.SaveChangesAsync();

            return walkDomain;
        }

        

       
    }
}
