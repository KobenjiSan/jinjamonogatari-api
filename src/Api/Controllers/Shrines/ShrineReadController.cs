using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/shrines")]
public class ShrineReadController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShrineReadController(IMediator mediator)
    {
        _mediator = mediator;
    }

    
}