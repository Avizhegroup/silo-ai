namespace SiloAI.Application.Api.Features;

public class CreateRagInstructionCommandHandler(AiApiContext context) : IRequestHandler<CreateRagInstructionCommand, RagInstructionDto>
{
    public async Task<RagInstructionDto> Handle(CreateRagInstructionCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var instruction = new RagInstruction
        {
            Id = Guid.NewGuid(),
            DocType = (request.DocType ?? RagDocType.GeneralChat).ToString(),
            Key = string.IsNullOrWhiteSpace(request.Key) ? null : request.Key.Trim(),
            Category = request.Category,
            Tags = request.Tags,
            Content = request.Content,
            IsActive = true,
            CreateDateTime = now,
            CreatorUserId = request.CreatorUserId,
            LastUpdateDateTime = now,
            LastUpdateUserId = request.CreatorUserId
        };

        context.RagInstructions.Add(instruction);
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
