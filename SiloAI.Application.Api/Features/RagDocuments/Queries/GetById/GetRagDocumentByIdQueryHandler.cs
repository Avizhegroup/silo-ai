namespace SiloAI.Application.Api.Features;

public class GetRagDocumentByIdQueryHandler(AiApiContext context) : IRequestHandler<GetRagDocumentByIdQuery, RagDocumentDetailsDto?>
{
    public async Task<RagDocumentDetailsDto?> Handle(GetRagDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        var doc = await context.RagDocuments
            .AsNoTracking()
            .Where(d => d.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (doc is null) return null;

        var chunks = await context.RagDocumentChunks
            .AsNoTracking()
            .Where(c => c.DocumentId == request.Id)
            .OrderBy(c => c.ChunkIndex)
            .Select(c => new RagDocumentChunkDto
            {
                Id = c.Id,
                ChunkIndex = c.ChunkIndex,
                Content = c.Content,
                TokenCount = c.TokenCount,
                CreateDateTime = c.CreateDateTime
            })
            .ToListAsync(cancellationToken);

        return new RagDocumentDetailsDto
        {
            Id = doc.Id,
            FileName = doc.FileName,
            OriginalFileName = doc.OriginalFileName,
            ContentType = doc.ContentType,
            DocType = Enum.TryParse<RagDocType>(doc.DocType, out var dt) ? dt : RagDocType.GeneralChat,
            Key = doc.Key,
            Category = doc.Category,
            Tags = doc.Tags,
            FileHash = doc.FileHash,
            FileSize = doc.FileSize,
            ProcessingStatus = doc.ProcessingStatus,
            ProcessingError = doc.ProcessingError,
            ChunkCount = doc.ChunkCount,
            CreateDateTime = doc.CreateDateTime,
            CreatorUserId = doc.CreatorUserId,
            LastUpdateDateTime = doc.LastUpdateDateTime,
            Chunks = chunks
        };
    }
}
