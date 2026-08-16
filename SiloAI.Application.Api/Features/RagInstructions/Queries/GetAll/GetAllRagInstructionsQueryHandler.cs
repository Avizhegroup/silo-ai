namespace SiloAI.Application.Api.Features;

public class GetAllRagInstructionsQueryHandler(AiApiContext context) : IRequestHandler<GetAllRagInstructionsQuery, List<RagInstructionDto>>
{
    public async Task<List<RagInstructionDto>> Handle(GetAllRagInstructionsQuery request, CancellationToken cancellationToken)
    {
        var query = context.RagInstructions.AsNoTracking().AsQueryable();

        if (request.DocType.HasValue)
        {
            var docType = request.DocType.Value.ToString();
            query = query.Where(x => x.DocType == docType);
        }

        if (request.IsActive.HasValue)
            query = query.Where(x => x.IsActive == request.IsActive.Value);

        return await query
            .OrderBy(x => x.CreateDateTime)
            .Select(x => MapInstruction(x))
            .ToListAsync(cancellationToken);
    }

    private static RagInstructionDto MapInstruction(RagInstruction x) => new()
    {
        Id = x.Id,
        DocType = Enum.TryParse<RagDocType>(x.DocType, out var dt) ? dt : RagDocType.GeneralChat,
        Key = x.Key,
        Category = x.Category,
        Tags = x.Tags,
        Content = x.Content,
        IsActive = x.IsActive,
        CreateDateTime = x.CreateDateTime,
        LastUpdateDateTime = x.LastUpdateDateTime
    };
}
