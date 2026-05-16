using El_Nabaash.API.DTOs.Site.Response;
using El_Nabaash.API.Services.Abstraction;

namespace El_Nabaash.API.Endpoints.Sites;

public static class SiteEndpoints
{
    // Endpoint Groups for Sites

    //first group 
    public static IEndpointRouteBuilder MapSiteEndpoints(this IEndpointRouteBuilder route)
    {
        // first Create the group 
        var publicGroup = route.MapGroup("/api/public/sites")
            .AllowAnonymous()
            .WithSummary("Public Site Endpoints")
            .WithDescription("Endpoint that expose public site data")
            .WithTags("Sites - Public");
        
        
        
        // then endpoints 
        publicGroup.MapGet("",GetAllPublicSites)
            .WithName(nameof(GetAllPublicSites))
            .WithSummary("Get All Sites (Public)")
            .WithDescription("Return all sites with their public data")
            // .Produces(StatusCodes.Status200OK, typeof(List<PublicSiteResponse>));
        // or 
            .Produces<List<PublicSiteResponse>>(StatusCodes.Status200OK);
            


        return route;
    }

    // Handlers Methods for Sites
    private static async Task<IResult> GetAllPublicSites(ISiteService siteService, CancellationToken ct)
    {
        // Logic to get all sites
        return Results.Ok(await siteService.GetAllPublicSitesAsync(ct));
    }
}