namespace SiloAI.Application.Api.Features;

public class GetAllRagDocumentsQueryHandler(AiApiContext context) : IRequestHandler<GetAllRagDocumentsQuery, List<RagDocumentDto>>
{
    public async Task<List<RagDocumentDto>> Handle(GetAllRagDocumentsQuery request, CancellationToken cancellationToken)
    {
        return await context.RagDocuments
            .AsNoTracking()
            .OrderByDescending(d => d.CreateDateTime)
            .Select(d => MapDocument(d))
            .ToListAsync(cancellationToken);
    }

    private static RagDocumentDto MapDocument(RagDocument d) => new()
    {
        Id = d.Id,
        FileName = d.FileName,
        OriginalFileName = d.OriginalFileName,
        ContentType = d.ContentType,
        DocType = Enum.TryParse<RagDocType>(d.DocType, out var dt) ? dt : RagDocType.GeneralChat,
        Key = d.Key,
        Category = d.Category,
        Tags = d.Tags,
        FileHash = d.FileHash,
        FileSize = d.FileSize,
        ProcessingStatus = d.ProcessingStatus,
        ProcessingError = d.ProcessingError,
        ChunkCount = d.ChunkCount,
        CreateDateTime = d.CreateDateTime,
        CreatorUserId = d.CreatorUserId,
        LastUpdateDateTime = d.LastUpdateDateTime
    };
}
