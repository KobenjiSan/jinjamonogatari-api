using Application.Common.Exceptions;
using Application.Features.Etiquette.Services;
using MediatR;

namespace Application.Features.Etiquette.Queries.GetEtiquetteTopicDetail;

public class GetEtiquetteTopicDetailByIdHandler
    : IRequestHandler<GetEtiquetteTopicDetailByIdQuery, GetEtiquetteTopicDetailResult>
{
    private readonly IEtiquetteReadService _read;

    public GetEtiquetteTopicDetailByIdHandler(IEtiquetteReadService read)
    {
        _read = read;
    }

    public async Task<GetEtiquetteTopicDetailResult> Handle(GetEtiquetteTopicDetailByIdQuery request, CancellationToken ct)
    {
        var topic = await _read.GetTopicDetailByIdAsync(request.TopicId, ct);
        if (topic is null) throw new NotFoundException("Etiquette topic not found.");
        return topic;
    }
}

public class GetEtiquetteTopicDetailBySlugHandler
    : IRequestHandler<GetEtiquetteTopicDetailBySlugQuery, GetEtiquetteTopicDetailResult>
{
    private readonly IEtiquetteReadService _read;

    public GetEtiquetteTopicDetailBySlugHandler(IEtiquetteReadService read)
    {
        _read = read;
    }

    public async Task<GetEtiquetteTopicDetailResult> Handle(GetEtiquetteTopicDetailBySlugQuery request, CancellationToken ct)
    {
        var topic = await _read.GetTopicDetailBySlugAsync(request.Slug, ct);
        if (topic is null) throw new NotFoundException("Etiquette topic not found.");
        return topic;
    }
}
