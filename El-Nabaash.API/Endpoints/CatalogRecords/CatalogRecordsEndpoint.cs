using System.Security.Claims;
using El_Nabaash.API.DTOs.Catalog.Request;
using El_Nabaash.API.DTOs.CatalogRecord.Response;
using Microsoft.AspNetCore.Http.HttpResults;

namespace El_Nabaash.API.Endpoints.CatalogRecords;

public static class CatalogRecordsEndpoint
{
    public static IEndpointRouteBuilder MapCatalogRecordEndpoints(this IEndpointRouteBuilder route)
    {
        var privateGroup = route.MapGroup("/api/private/catalogrecords")
            .RequireAuthorization()
            .WithTags("CatalogRecords")
            .AddEndpointFilter<ExceptionHandlingFilter>();

        privateGroup.MapGet("/artifact/{artifactId:int}", GetCatalogRecordsByArtifact)
            .WithName(nameof(GetCatalogRecordsByArtifact))
            .WithSummary("Get Catalog Records by Artifact")
            .WithDescription("Returns all catalog records associated with the specified artifact.")
            .Produces<List<CatalogRecordResponseDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        privateGroup.MapGet("/{id:int}", GetCatalogRecordById)
            .WithName(nameof(GetCatalogRecordById))
            .WithSummary("Get Catalog Record by ID")
            .WithDescription("Returns a single catalog record including submitter, verifier, and notes.")
            .Produces<CatalogRecordResponseDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        
        return route;
    }


    // Handlers
    private static async Task<Results<Ok<List<CatalogRecordResponseDto>>, NotFound>>
        GetCatalogRecordsByArtifact(
            int artifactId,
            ICatalogRecordService service,
            CancellationToken ct)
    {
        var records = await service.GetCatalogRecordsByArtifactAsync(artifactId, ct);

        if (records is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(records);
    }
    
    private static async Task<Results<Ok<CatalogRecordResponseDto>, NotFound>>
        GetCatalogRecordById(
            int id,
            ICatalogRecordService service,
            CancellationToken ct)
    {
        var record = await service.GetCatalogRecordByIdAsync(id, ct);

        if (record is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(record);
    }
    
    private static async Task<Results<Created<CatalogRecordResponseDto>, BadRequest>>
        CreateCatalogRecord(
            CreateCatalogRecordRequestDto request,
            ClaimsPrincipal user,
            ICatalogRecordService service,
            CancellationToken ct)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return TypedResults.BadRequest();

        var result = await service.CreateCatalogRecordAsync(request, userId, ct);

        if (result is null)
            return TypedResults.BadRequest(); // artifact not found or bad input

        return TypedResults.Created(
            $"/api/private/catalogrecords/{result.Id}",
            result);
    }
}