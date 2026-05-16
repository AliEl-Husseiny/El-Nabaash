using El_Nabaash.API.DTOs.Site.Request;
using El_Nabaash.API.DTOs.Site.Response;

namespace El_Nabaash.API.Services.Abstraction;

public interface ISiteService
{
    // get all sites as a list of SiteResponseDto
    Task<List<PublicSiteResponse>> GetAllPublicSitesAsync(CancellationToken ct);
    Task<PublicSiteResponse?> GetPublicSiteByIdAsync(int Id, CancellationToken ct);
    Task<List<PrivateSiteResponse>> GetAllPrivateSitesAsync(CancellationToken ct);
    Task<PrivateSiteResponse?> GetPrivateSiteByIdAsync(int Id, CancellationToken ct);
    Task<PrivateSiteResponse> CreateSiteAsync(CreateSiteRequest request, CancellationToken ct);
    Task<bool> UpdateSiteAsync(int Id, UpdateSiteRequest request, CancellationToken ct);
    Task<bool> DeleteSiteAsync(int id, CancellationToken ct);
}