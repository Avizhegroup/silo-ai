namespace SiloAI.Application.Api.Features;

public class GetRagInstructionByIdQueryHandler(AiApiContext context) : IRequestHandler<GetRagInstructionByIdQuery, RagInstructionDto?>
{
    public async Task<RagInstructionDto?> Handle(GetRagInstructionByIdQuery request, CancellationToken cancellationToken)
    {
        var instruction = await context.RagInstructions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (instruction is null) return null;

        return new RagInstructionDto
        {
            Id = instruction.Id,
            DocType = Enum.TryParse<RagDocType>(instruction.DocType, out var dt) ? dt : RagDocType.GeneralChat,
            Key = instruction.Key,
            Category = instruction.Category,
            Tags = instruction.Tags,
            Content = instruction.Content,
            IsActive = instruction.IsActive,
            CreateDateTime = instruction.CreateDateTime,
            LastUpdateDateTime = instruction.LastUpdateDateTime
        };
    }
}
