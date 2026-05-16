using El_Nabaash.API.DTOs.Site.Response;

namespace El_Nabaash.API.Services.Abstraction;

public interface ISiteService
{
    // get all sites as a list of SiteResponseDto
    Task<List<PublicSiteResponse>> GetAllPublicSitesAsync(CancellationToken ct);
}