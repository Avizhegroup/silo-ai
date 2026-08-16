namespace SiloAI.Application.Api.Features;

public class SearchRagDocumentsQueryHandler(IRagSearchService search) : IRequestHandler<SearchRagDocumentsQuery, List<RagSearchHitDto>>
{
    public async Task<List<RagSearchHitDto>> Handle(SearchRagDocumentsQuery request, CancellationToken cancellationToken)
    {
        var hits = await search.SearchAsync(
            request.Query, request.TopK, request.DocType?.ToString(), request.Key, cancellationToken);

        return hits.Select(h => new RagSearchHitDto
        {
            ChunkId = h.ChunkId,
            DocumentId = h.DocumentId,
            FileName = h.FileName,
            Category = h.Category,
            ChunkIndex = h.ChunkIndex,
            Content = h.Content,
            Distance = h.Distance,
            Similarity = h.Similarity
        }).ToList();
    }
}
