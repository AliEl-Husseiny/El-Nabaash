using El_Nabaash.API.DTOs.Site.Request;

namespace El_Nabaash.API.Services.Implementation;

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

    public async Task<bool> UpdateSiteAsync(int Id, UpdateSiteRequest request, CancellationToken ct)
    {
        var site = _dbContext.Sites.FindAsync(Id, ct).Result as Site;
        if (site is null)
            return false;
       
        site.Name = request.Name;
        site.Location = request.Location;
        site.Coordinates = request.Coordinates;
        site.Latitude = request.Latitude;
        site.Longitude = request.Longitude;
        site.Description = request.Description;
        site.PublicNarrative = request.PublicNarrative;
        site.ElNabaashNarrative = request.ElNabaashNarrative;
        return true;

    }

    public async Task<bool> DeleteSiteAsync(int id, CancellationToken ct)
    {
        var site = await _dbContext.Sites.FindAsync(id, ct);
        if (site is null)
            return false;

        _dbContext.Sites.Remove(site);
        await _dbContext.SaveChangesAsync(ct);
        return true;
    }
}