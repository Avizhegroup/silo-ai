namespace SiloAI.Application.Api.Features;

public class UpdateRagInstructionCommandHandler(AiApiContext context) : IRequestHandler<UpdateRagInstructionCommand, RagInstructionDto?>
{
    public async Task<RagInstructionDto?> Handle(UpdateRagInstructionCommand request, CancellationToken cancellationToken)
    {
        var instruction = await context.RagInstructions
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (instruction is null) return null;

        instruction.DocType = (request.DocType ?? RagDocType.GeneralChat).ToString();
        instruction.Key = string.IsNullOrWhiteSpace(request.Key) ? null : request.Key.Trim();
        instruction.Category = request.Category;
        instruction.Tags = request.Tags;
        instruction.Content = request.Content;
        instruction.IsActive = request.IsActive;
        instruction.LastUpdateDateTime = DateTime.UtcNow;
        instruction.LastUpdateUserId = request.UpdaterUserId;

        await context.SaveChangesAsync(cancellationToken);

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
