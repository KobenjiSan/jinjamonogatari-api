using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Kami.Commands.UpdateKami;

// COMMAND
public record UpdateKamiCommand(int KamiId, UpdateKamiRequest Request, IFormFile? File) : IRequest<UpdateKamiResult>;

// RESULTS
public record UpdateKamiResult(KamiReadCMSDto? Kami);