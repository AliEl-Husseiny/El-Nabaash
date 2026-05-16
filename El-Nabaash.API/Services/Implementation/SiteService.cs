using El_Nabaash.API.DTOs.Site.Response;
using El_Nabaash.API.Services.Abstraction;

namespace El_Nabaash.API.Services;

public class SiteService(AppDbContext _dbContext) : ISiteService
{
    public async Task<List<PublicSiteResponse>> GetAllPublicSitesAsync(CancellationToken ct)
    {
        // throw new Exception(); // testing
        return await _dbContext.Sites
            .Select(s => new PublicSiteResponse
            {
                Id = s.Id,
                Name = s.Name,
                Location = s.Location,
                Coordinates = s.Coordinates,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                Description = s.Description,
                PublicNarrative = s.PublicNarrative
            })
            .ToListAsync(ct);
    }

    public async Task<PublicSiteResponse?> GetPublicSiteByIdAsync(int Id, CancellationToken ct)
    {
        return await _dbContext.Sites.AsNoTracking()
            .Where(s => s.Id == Id)
            .Select(s => new PublicSiteResponse
            {
                Id = s.Id,
                Name = s.Name,
                Location = s.Location,
                Coordinates = s.Coordinates,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                Description = s.Description,
                PublicNarrative = s.PublicNarrative
            })
            .FirstOrDefaultAsync(ct);
    }
}