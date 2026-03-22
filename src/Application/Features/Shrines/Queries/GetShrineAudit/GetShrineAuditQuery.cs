using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineAudit;

public record GetShrineAuditQuery(int ShrineId) : IRequest<ShrineAuditDto>;