using System.ComponentModel.DataAnnotations;
using API.Extensions;
using Application.Features.Shrines.Commands.CreateFolklore;
using Application.Features.Shrines.Commands.CreateGalleryImage;
using Application.Features.Shrines.Commands.CreateHistory;
using Application.Features.Shrines.Commands.CreateKamiInShrine;
using Application.Features.Shrines.Commands.CreateShrine;
using Application.Features.Shrines.Commands.DeleteFolklore;
using Application.Features.Shrines.Commands.DeleteGalleryImage;
using Application.Features.Shrines.Commands.DeleteHistory;
using Application.Features.Shrines.Commands.DeleteShrine;
using Application.Features.Shrines.Commands.ImportShrines;
using Application.Features.Shrines.Commands.LinkKamiToShrine;
using Application.Features.Shrines.Commands.UnlinkKamiToShrine;
using Application.Features.Shrines.Commands.UpdateFolklore;
using Application.Features.Shrines.Commands.UpdateGalleryImage;
using Application.Features.Shrines.Commands.UpdateHistory;
using Application.Features.Shrines.Commands.UpdateKami;
using Application.Features.Shrines.Commands.UpdateShrineMeta;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/shrines")]
[Authorize(Roles = "Admin,Editor")]
public class ShrineWriteController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShrineWriteController(IMediator mediator)
    {
        _mediator = mediator;
    }


    #region META

    // PUT /api/shrines/cms/{id}/meta
    [HttpPut("cms/{id}/meta")]
    public async Task<IActionResult> UpdateShrineMeta(
        [FromRoute] int id,
        [FromBody] UpdateShrineMetaRequest request
    )
    {
        var role = User.GetUserRole();
        var command = new UpdateShrineMetaCommand(role, id, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    #endregion

    #region KAMI

    // POST /api/shrines/cms/{shrineId}/kami/ (kami in body)
    // Create a new global Kami then link it automatically to shrine
    [HttpPost("cms/{shrineId}/kami")]
    public async Task<IActionResult> CreateKamiInShrine(
        [FromRoute] int shrineId,
        [FromBody] CreateKamiInShrineRequest request
    )
    {
        var role = User.GetUserRole();
        var command = new CreateKamiInShrineCommand(role, shrineId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // POST /api/shrines/cms/{shrineId}/kami/{kamiId}
    // link exisiting Kami to shrine
    [HttpPost("cms/{shrineId}/kami/{kamiId}")]
    public async Task<IActionResult> LinkKamiToShrine(
        [FromRoute] int shrineId,
        [FromRoute] int kamiId
    )
    {
        var role = User.GetUserRole();
        var command = new LinkKamiToShrineCommand(role, shrineId, kamiId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // DELETE /api/shrines/cms/{shrineId}/kami/{kamiId}
    // unlink kami from shrine
    [HttpDelete("cms/{shrineId}/kami/{kamiId}")]
    public async Task<IActionResult> UnlinkKamiToShrine(
        [FromRoute] int shrineId,
        [FromRoute] int kamiId
    )
    {
        var role = User.GetUserRole();
        var command = new UnlinkKamiToShrineCommand(role, shrineId, kamiId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // To be moved later into Kami Controller
    // PUT /api/shrines/cms/kami/{kamiId} (kami in body)
    // Update kami from shrine editor
    [HttpPut("cms/kami/{kamiId}")]
    public async Task<IActionResult> UpdateKami([FromRoute] int kamiId, [FromBody] UpdateKamiRequest request)
    {
        var command = new UpdateKamiCommand(kamiId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    #endregion
    
    #region HISTORY

    // POST /api/shrines/cms/{shrineId}/history/ (history in body)
    // Create a new history item linked automatically to shrine
    [HttpPost("cms/{shrineId}/history")]
    public async Task<IActionResult> CreateHistory([FromRoute] int shrineId, [FromBody] CreateHistoryRequest request)
    {
        var role = User.GetUserRole();
        var command = new CreateHistoryCommand(role, shrineId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // PUT /api/shrines/cms/history/{historyId} (history in body)
    // Update history
    [HttpPut("cms/history/{historyId}")]
    public async Task<IActionResult> UpdateHistory([FromRoute] int historyId, [FromBody] UpdateHistoryRequest request)
    {
        var role = User.GetUserRole();
        var command = new UpdateHistoryCommand(role, historyId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // DELETE /api/shrines/cms/history/{historyId}
    // fully delete history from shrine
    [HttpDelete("cms/history/{historyId}")]
    public async Task<IActionResult> DeleteHistory([FromRoute] int historyId)
    {
        var role = User.GetUserRole();
        var command = new DeleteHistoryCommand(role, historyId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    #endregion
    
    #region FOLKLORE

    // POST /api/shrines/cms/{shrineId}/folklore/ (folklore in body)
    // Create a new folklore item linked automatically to shrine
    [HttpPost("cms/{shrineId}/folklore")]
    public async Task<IActionResult> CreateFolklore([FromRoute] int shrineId, [FromBody] CreateFolkloreRequest request)
    {
        var role = User.GetUserRole();
        var command = new CreateFolkloreCommand(role, shrineId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // PUT /api/shrines/cms/folklore/{folkloreId} (folklore in body)
    // Update folklore
    [HttpPut("cms/folklore/{folkloreId}")]
    public async Task<IActionResult> UpdateFolklore([FromRoute] int folkloreId, [FromBody] UpdateFolkloreRequest request)
    {
        var role = User.GetUserRole();
        var command = new UpdateFolkloreCommand(role, folkloreId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // DELETE /api/shrines/cms/folklore/{folkloreId}
    // fully delete folklore from shrine
    [HttpDelete("cms/folklore/{folkloreId}")]
    public async Task<IActionResult> DeleteFolklore([FromRoute] int folkloreId)
    {
        var role = User.GetUserRole();
        var command = new DeleteFolkloreCommand(role, folkloreId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    #endregion

    #region GALLERY

    // POST /api/shrines/cms/{shrineId}/gallery/ (image in body)
    // Create a new image linked automatically to shrine gallery
    [HttpPost("cms/{shrineId}/gallery")]
    public async Task<IActionResult> CreateGalleryImage([FromRoute] int shrineId, [FromBody] CreateImageRequest request)
    {
        var role = User.GetUserRole();
        var command = new CreateGalleryImageCommand(role, shrineId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // PUT /api/shrines/cms/gallery/{imageId} (image in body)
    // Update image
    [HttpPut("cms/gallery/{imageId}")]
    public async Task<IActionResult> UpdateGalleryImage([FromRoute] int imageId, [FromBody] ImageRequest request)
    {
        if (request.ImgId != imageId)
            throw new ValidationException("Route ID and body ID do not match.");

        var role = User.GetUserRole();
        var command = new UpdateGalleryImageCommand(role, imageId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // DELETE /api/shrines/cms/gallery/{imageId}
    [HttpDelete("cms/gallery/{imageId}")]
    public async Task<IActionResult> DeleteGalleryImage([FromRoute] int imageId)
    {
        var role = User.GetUserRole();
        var command = new DeleteGalleryImageCommand(role, imageId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    #endregion

    #region IMPORT

    // POST /api/shrines/cms/import
    [HttpPost("cms/import")]
    public async Task<IActionResult> ImportShrinesAsync([FromBody] ImportShrinesRequest request)
    {
        var command = new ImportShrinesCommand(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    #endregion

    #region CREATE SHRINE

    // POST /api/shrines/cms/create
    [HttpPost("cms/create")]
    public async Task<IActionResult> CreateShrineAsync([FromBody] CreateShrineRequest request)
    {
        var command = new CreateShrineCommand(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    #endregion

    #region DELETE SHRINE

    // Delete /api/shrines/cms/delete/{shrineId}
    [HttpDelete("cms/delete/{shrineId}")]
    [Authorize(Roles = "Admin")]    // Admins only
    public async Task<IActionResult> DeleteShrineAsync([FromRoute] int shrineId)
    {
        var command = new DeleteShrineCommand(shrineId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    #endregion
}


#region Meta Request

public record UpdateShrineMetaRequest(
    BasicMetaUpdateRequest Basic,
    TagChangesRequest Tags,
    ImageChangeRequest HeroImage
);

public record BasicMetaUpdateRequest(
    string? Slug,
    string? NameEn,
    string? NameJp,
    string? ShrineDesc,
    decimal? Lat,
    decimal? Lon,
    string? Prefecture,
    string? City,
    string? Ward,
    string? Locality,
    string? PostalCode,
    string? Country,
    string? PhoneNumber,
    string? Email,
    string? Website
);

#endregion

#region Tags Request

// Tag Changes
public record TagChangesRequest(
    IReadOnlyList<CreateTagRequest> Create,
    IReadOnlyList<UpdateTagRequest> Update,
    IReadOnlyList<int> Delete
);
public record CreateTagRequest(
    string TitleEn,
    string? TitleJp
);
public record UpdateTagRequest(
    int TagId,
    string TitleEn,
    string? TitleJp
);

#endregion

#region Images Request

public record ImageRequest(
    int ImgId,
    string? ImageUrl,
    string? Title,
    string? Desc,
    CitationRequest? Citation
);
public record ImageChangeRequest(
    string Action,
    string? ImgSource,
    string? Title,
    string? Desc,
    CitationRequest? Citation
);
public record CreateImageRequest(
    string? ImgSource,
    string? Title,
    string? Desc,
    CreateCitationRequest? Citation
);

#endregion

#region Citations Request

public record CitationRequest(
    int CiteId,
    string? Title,
    string? Author,
    string? Url,
    int? Year
);
public record CitationListChangesRequest(
    IReadOnlyList<CreateCitationRequest> Create,
    IReadOnlyList<CitationRequest> Update,
    IReadOnlyList<int> Delete
);
public record CreateCitationRequest(
    string? Title,
    string? Author,
    string? Url,
    int? Year
);

#endregion

#region Kami Request

// CREATE KAMI
public record CreateKamiInShrineRequest(
    string? NameEn,
    string? NameJp,
    string? Desc,
    CreateImageRequest? Image,
    IReadOnlyList<CreateCitationRequest> Citations
);


// UPDATE KAMI
public record UpdateKamiRequest(
    BasicKamiUpdateRequest Basic,
    ImageChangeRequest Image,
    CitationListChangesRequest Citations
);

public record BasicKamiUpdateRequest(
    string? NameEn,
    string? NameJp,
    string? Desc
);

#endregion

#region History Request

// CREATE HISTORY
public record CreateHistoryRequest(
    DateOnly? EventDate,
    int? SortOrder,
    string? Title,
    string? Information,
    CreateImageRequest? Image,
    IReadOnlyList<CreateCitationRequest> Citations
);

// UPDATE HISTORY
public record UpdateHistoryRequest(
    BasicHistoryUpdateRequest Basic,
    ImageChangeRequest Image,
    CitationListChangesRequest Citations
);
public record BasicHistoryUpdateRequest(
    DateOnly? EventDate,
    int? SortOrder,
    string? Title,
    string? Information
);

#endregion

#region Folklore Request

// CREATE FOLKLORE
public record CreateFolkloreRequest(
    int? SortOrder,
    string? Title,
    string? Information,
    CreateImageRequest? Image,
    IReadOnlyList<CreateCitationRequest> Citations
);

// UPDATE FOLKLORE
public record UpdateFolkloreRequest(
    BasicFolkloreUpdateRequest Basic,
    ImageChangeRequest Image,
    CitationListChangesRequest Citations
);
public record BasicFolkloreUpdateRequest(
    DateOnly? EventDate,
    int? SortOrder,
    string? Title,
    string? Information
);

#endregion

#region Import Request

public record ImportShrinesRequest(
    IReadOnlyList<ImportPreviewItemRequest> Previews
);

public record ImportPreviewItemRequest(
    string ImportId,
    string? Name,
    double Lat,
    double Lon
);

#endregion

#region Create Shrine Request

public record CreateShrineRequest(
    string? NameEn,
    string? NameJp,
    string? Address,
    double? Lat,
    double? Lon
);

#endregion