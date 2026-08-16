using System.Security.Cryptography;

namespace SiloAI.Application.Api.Features;

public class RebuildRagDocumentCommandHandler(
    AiApiContext context,
    IRagIndexingService indexing) : IRequestHandler<RebuildRagDocumentCommand, RagUploadResponseDto>
{
    public async Task<RagUploadResponseDto> Handle(RebuildRagDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = await context.RagDocuments
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (document is null)
            throw new InvalidOperationException($"RagDocument {request.Id} not found.");

        string hash;
        using (var ms = new MemoryStream(request.FileContent))
        {
            hash = ComputeSha256(ms);
        }

        document.FileHash = hash;
        document.FileSize = request.FileContent.Length;
        document.ContentType = string.IsNullOrWhiteSpace(request.ContentType) ? document.ContentType : request.ContentType;
        document.LastUpdateDateTime = DateTime.UtcNow;
        document.LastUpdateUserId = request.UpdaterUserId;
        await context.SaveChangesAsync(cancellationToken);

        using var indexStream = new MemoryStream(request.FileContent);
        var result = await indexing.RebuildAsync(
            document.Id, indexStream, request.FileName, document.ContentType, cancellationToken);

        return new RagUploadResponseDto
        {
            DocumentId = result.DocumentId,
            ChunkCount = result.ChunkCount,
            ProcessingStatus = result.Status,
            ProcessingError = result.Error
        };
    }

    private static string ComputeSha256(Stream stream)
    {
        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(stream);
        return Convert.ToHexString(hash);
    }
}
