using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineMapPopupCMS;

// QUERIES
public record GetShrineMapPopupCMSQuery(int ShrineId) : IRequest<GetShrineMapPopupCMSResult>;

// RESULTS
public record GetShrineMapPopupCMSResult(ShrineListCMSDto ShrineMapPopup);