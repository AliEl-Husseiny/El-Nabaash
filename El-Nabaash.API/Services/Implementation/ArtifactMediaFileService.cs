namespace El_Nabaash.API.Services.Implementation;

public class ArtifactMediaFileService(AppDbContext dbContext) : IArtifactMediaFileService
{
    public async Task<ArtifactMediaFile?> CreateArtifactMediaFileAsync(
        int artifactId, IFormFile file, bool isPrimary, CancellationToken ct)
    {
        // check the file 
        var artifact = await dbContext.Artifacts.FindAsync(artifactId, ct);
        if (artifact is null)
        {
            return null;
        }

        // validate the input
        if (file.Length == 0)
        {
            throw new ArgumentException("File is empty", nameof(file));
        }

        if (isPrimary)
        {
            var existingPrimary = await dbContext.ArtifactMediaFiles
                .Where(m => m.ArtifactId == artifactId && m.IsPrimary)
                .ToListAsync(ct);

            foreach (var mediaFile in existingPrimary)
            {
                mediaFile.IsPrimary = false;
            }
        }

        //convert IFormFile to byte[]
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms, ct);

        // create the media file 
        var newMedia = new ArtifactMediaFile()
        {
            ArtifactId = artifactId,
            FileName = file.FileName,
            ContentType = file.ContentType,
            Data = ms.ToArray(),
            IsPrimary = isPrimary
        };

        dbContext.ArtifactMediaFiles.Add(newMedia);
        await dbContext.SaveChangesAsync(ct);

        return newMedia;
    }
}