using El_Nabaash.API.DTOs.Site.Request;
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

    public async Task<List<PrivateSiteResponse>> GetAllPrivateSitesAsync(CancellationToken ct)
    {
        return await _dbContext.Sites
            .Select(s => new PrivateSiteResponse
            {
                Id = s.Id,
                Name = s.Name,
                Location = s.Location,
                Coordinates = s.Coordinates,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                Description = s.Description,
                PublicNarrative = s.PublicNarrative,
                PrivateNarrative = s.ElNabaashNarrative
            })
            .ToListAsync(ct);
    }

    public async Task<PrivateSiteResponse?> GetPrivateSiteByIdAsync(int Id, CancellationToken ct)
    {
        return await _dbContext.Sites.AsNoTracking()
            .Where(s => s.Id == Id)
            .Select(s => new PrivateSiteResponse
            {
                Id = s.Id,
                Name = s.Name,
                Location = s.Location,
                Coordinates = s.Coordinates,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                Description = s.Description,
                PublicNarrative = s.PublicNarrative,
                PrivateNarrative = s.ElNabaashNarrative
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<PrivateSiteResponse> CreateSiteAsync(CreateSiteRequest request, CancellationToken ct)
    {
        var site = new Site
        {
            Name = request.Name,
            Location = request.Location,
            Coordinates = request.Coordinates,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Description = request.Description,
            PublicNarrative = request.PublicNarrative,
            ElNabaashNarrative = request.ElNabaashNarrative
        };

        _dbContext.Sites.Add(site);
        await _dbContext.SaveChangesAsync(ct);

        return new PrivateSiteResponse
        {
            Id = site.Id,
            Name = site.Name,
            Location = site.Location,
            Coordinates = site.Coordinates,
            Latitude = site.Latitude,
            Longitude = site.Longitude,
            Description = site.Description,
            PublicNarrative = site.PublicNarrative,
            PrivateNarrative = site.ElNabaashNarrative
        };
    }
}