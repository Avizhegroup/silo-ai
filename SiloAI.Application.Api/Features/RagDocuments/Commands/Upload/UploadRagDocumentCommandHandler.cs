using System.Security.Cryptography;

namespace SiloAI.Application.Api.Features;

public class UploadRagDocumentCommandHandler(
    AiApiContext context,
    IRagIndexingService indexing) : IRequestHandler<UploadRagDocumentCommand, RagUploadResponseDto>
{
    public async Task<RagUploadResponseDto> Handle(UploadRagDocumentCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var extension = (Path.GetExtension(request.FileName) ?? string.Empty).ToLowerInvariant();

        string hash;
        using (var ms = new MemoryStream(request.FileContent))
        {
            hash = ComputeSha256(ms);
        }

        var document = new RagDocument
        {
            Id = Guid.NewGuid(),
            FileName = $"{Guid.NewGuid():N}{extension}",
            OriginalFileName = request.FileName,
            ContentType = string.IsNullOrWhiteSpace(request.ContentType) ? "application/octet-stream" : request.ContentType,
            DocType = (request.DocType ?? RagDocType.GeneralChat).ToString(),
            Key = string.IsNullOrWhiteSpace(request.Key) ? null : request.Key.Trim(),
            Category = request.Category,
            Tags = request.Tags,
            FileHash = hash,
            FileSize = request.FileContent.Length,
            ProcessingStatus = RagProcessingStatus.Pending,
            ChunkCount = 0,
            CreateDateTime = now,
            CreatorUserId = request.CreatorUserId,
            LastUpdateDateTime = now,
            LastUpdateUserId = request.CreatorUserId
        };

        context.RagDocuments.Add(document);
        await context.SaveChangesAsync(cancellationToken);

        using var indexStream = new MemoryStream(request.FileContent);
        var result = await indexing.IndexAsync(
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
