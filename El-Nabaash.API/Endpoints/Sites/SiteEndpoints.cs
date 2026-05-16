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
            .WithTags("Sites - Public")
            .AddEndpointFilter<ExceptionHandlingFilter>();
        
        
        
        // then endpoints 
        publicGroup.MapGet("",GetAllPublicSites)
            .WithName(nameof(GetAllPublicSites))
            .WithSummary("Get All Sites (Public)")
            .WithDescription("Return all sites with their public data")
            // .Produces(StatusCodes.Status200OK, typeof(List<PublicSiteResponse>));
        // or 
            .Produces<List<PublicSiteResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);
            

        
        publicGroup.MapGet("/{id:int}",GetPublicSiteById)
            .WithName(nameof(GetPublicSiteById))
            .WithSummary("Get Site By Id (Public)")
            .WithDescription("Return a site with its public data by given Id")
            .Produces<PublicSiteResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        
        // private group 
        var privateGroup = route.MapGroup("/api/private/sites")
            .RequireAuthorization()
            .WithSummary("Private Site Endpoints")
            .WithDescription("Endpoint that expose private site data")
            .WithTags("Sites - Private")
            .RequireAuthorization()
            .AddEndpointFilter<ExceptionHandlingFilter>();

        
        privateGroup.MapGet("", GetAllPrivateSites)
            .WithName(nameof(GetAllPrivateSites))
            .WithSummary("Get All Sites (Private)")
            .WithDescription("Return all sites with their private data")
            .Produces<List<PrivateSiteResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError);
        
        privateGroup.MapGet("/{id:int}", GetPrivateSiteById)
            .WithName(nameof(GetPrivateSiteById))
            .WithSummary("Get Site By Id (Private)")
            .WithDescription("Return a site with its private data by given Id")
            .Produces<PrivateSiteResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError);
        
        
        
        return route;
    }

    
    // Handlers Methods for private Sites 
    
    private static async Task<IResult> GetPrivateSiteById(int Id, ISiteService siteService, CancellationToken ct)
    {
        var site = await siteService.GetPrivateSiteByIdAsync(Id, ct);
        return site == null ? Results.NotFound($"Site with Id {Id} not found") : Results.Ok(site);
    }
    
    private static async Task<IResult> GetAllPrivateSites(ISiteService siteService, CancellationToken ct)
    {
        // Logic to get all sites
        return Results.Ok(await siteService.GetAllPrivateSitesAsync(ct));
    }
    
    
    // Handlers Methods for public Sites
    
    private static async Task<IResult> GetPublicSiteById(int Id, ISiteService siteService, CancellationToken ct)
    {
        var site = await siteService.GetPublicSiteByIdAsync(Id, ct);
        return site == null ? Results.NotFound($"Site with Id {Id} not found") : Results.Ok(site);
    }

    private static async Task<IResult> GetAllPublicSites(ISiteService siteService, CancellationToken ct)
    {
        // Logic to get all sites
        return Results.Ok(await siteService.GetAllPublicSitesAsync(ct));
    }
}