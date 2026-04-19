using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using API.Extensions;
using Application.Common.Models.Images;
using Application.Features.Shrines.Commands.CreateFolklore;
using Application.Features.Shrines.Commands.CreateGalleryImage;
using Application.Features.Shrines.Commands.CreateHeroImage;
using Application.Features.Shrines.Commands.CreateHistory;
using Application.Features.Shrines.Commands.CreateKamiInShrine;
using Application.Features.Shrines.Commands.CreateShrine;
using Application.Features.Shrines.Commands.DeleteFolklore;
using Application.Features.Shrines.Commands.DeleteGalleryImage;
using Application.Features.Shrines.Commands.DeleteHeroImage;
using Application.Features.Shrines.Commands.DeleteHistory;
using Application.Features.Shrines.Commands.DeleteShrine;
using Application.Features.Shrines.Commands.ImportShrines;
using Application.Features.Shrines.Commands.LinkKamiToShrine;
using Application.Features.Shrines.Commands.PublishReviewShrine;
using Application.Features.Shrines.Commands.RejectReviewShrine;
using Application.Features.Shrines.Commands.SubmitReviewShrine;
using Application.Features.Shrines.Commands.UnlinkKamiToShrine;
using Application.Features.Shrines.Commands.UpdateFolklore;
using Application.Features.Shrines.Commands.UpdateGalleryImage;
using Application.Features.Shrines.Commands.UpdateHeroImage;
using Application.Features.Shrines.Commands.UpdateHistory;
using Application.Features.Shrines.Commands.UpdateShrineMeta;
using Application.Features.Shrines.Commands.UpdateShrineNotes;
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

    // PUT /api/shrines/cms/{shrineId}/meta
    [HttpPut("cms/{shrineId}/meta")]
    public async Task<IActionResult> UpdateShrineMeta(
        [FromRoute] int shrineId,
        [FromBody] UpdateShrineMetaRequest request
    )
    {
        var role = User.GetUserRole();
        var command = new UpdateShrineMetaCommand(role, shrineId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // POST /api/shrines/cms/{shrineId}/hero-image
    [HttpPost("cms/{shrineId}/hero-image")]
    public async Task<ActionResult<ImageFullDto>> CreateHeroImage(
        [FromRoute] int shrineId,
        [FromForm] CreateImageFormRequest request
    )
    {
        var role = User.GetUserRole();
        var command = new CreateHeroImageCommand(role, shrineId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // PUT /api/shrines/cms/{shrineId}/hero-image
    [HttpPut("cms/{shrineId}/hero-image")]
    public async Task<ActionResult<ImageFullDto>> UpdateHeroImage(
        [FromRoute] int shrineId,
        [FromForm] UpdateImageFormRequest request
    )
    {
        var role = User.GetUserRole();
        var command = new UpdateHeroImageCommand(role, shrineId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // DELETE /api/shrines/cms/{shrineId}/hero-image/{imageId}
    [HttpDelete("cms/{shrineId}/hero-image/{imageId}")]
    public async Task<IActionResult> DeleteHeroImage(
        [FromRoute] int shrineId,
        [FromRoute] int imageId
    )
    {
        var role = User.GetUserRole();
        var command = new DeleteHeroImageCommand(role, shrineId, imageId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    #endregion

    #region NOTES

    // PUT /api/shrines/cms/{id}/notes
    [HttpPut("cms/{id}/notes")]
    public async Task<IActionResult> UpdateShrineNotes(
        [FromRoute] int id,
        [FromBody] UpdateShrineNotesRequest request
    )
    {
        var role = User.GetUserRole();
        var command = new UpdateShrineNotesCommand(role, id, request.Notes);
        await _mediator.Send(command);
        return NoContent();
    }

    #endregion

    #region KAMI

    // POST /api/shrines/cms/{shrineId}/kami/ (kami in body)
    // Create a new global Kami then link it automatically to shrine
    [HttpPost("cms/{shrineId}/kami")]
    public async Task<IActionResult> CreateKamiInShrine(
        [FromRoute] int shrineId,
        [FromForm] string data,
        [FromForm] IFormFile? file
    )
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var request = JsonSerializer.Deserialize<CreateKamiInShrineRequest>(data, options);

        if (request == null)
            return BadRequest("Invalid payload");

        var role = User.GetUserRole();
        var command = new CreateKamiInShrineCommand(role, shrineId, request, file);
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

    #endregion
    
    #region HISTORY

    // POST /api/shrines/cms/{shrineId}/history/ (history in body)
    // Create a new history item linked automatically to shrine
    [HttpPost("cms/{shrineId}/history")]
    public async Task<IActionResult> CreateHistory(
        [FromRoute] int shrineId, 
        [FromForm] string data,
        [FromForm] IFormFile? file
    )
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var request = JsonSerializer.Deserialize<CreateHistoryRequest>(data, options);

        if (request == null)
            return BadRequest("Invalid payload");
        
        var role = User.GetUserRole();
        var command = new CreateHistoryCommand(role, shrineId, request, file);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // PUT /api/shrines/cms/history/{historyId} (history in body)
    // Update history
    [HttpPut("cms/history/{historyId}")]
    public async Task<IActionResult> UpdateHistory(
        [FromRoute] int historyId, 
        [FromForm] string data,
        [FromForm] IFormFile? file
    )
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var request = JsonSerializer.Deserialize<UpdateHistoryRequest>(data, options);

        if (request == null)
            return BadRequest("Invalid payload");
        
        var role = User.GetUserRole();
        var command = new UpdateHistoryCommand(role, historyId, request, file);
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
    public async Task<IActionResult> CreateFolklore(
        [FromRoute] int shrineId, 
        [FromForm] string data,
        [FromForm] IFormFile? file
    )
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var request = JsonSerializer.Deserialize<CreateFolkloreRequest>(data, options);

        if (request == null)
            return BadRequest("Invalid payload");
        
        var role = User.GetUserRole();
        var command = new CreateFolkloreCommand(role, shrineId, request, file);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // PUT /api/shrines/cms/folklore/{folkloreId} (folklore in body)
    // Update folklore
    [HttpPut("cms/folklore/{folkloreId}")]
    public async Task<IActionResult> UpdateFolklore(
        [FromRoute] int folkloreId, 
        [FromForm] string data,
        [FromForm] IFormFile? file
    )
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var request = JsonSerializer.Deserialize<UpdateFolkloreRequest>(data, options);

        if (request == null)
            return BadRequest("Invalid payload");
        
        var role = User.GetUserRole();
        var command = new UpdateFolkloreCommand(role, folkloreId, request, file);
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
    public async Task<IActionResult> CreateGalleryImage([FromRoute] int shrineId, [FromForm] CreateImageFormRequest request)
    {
        var role = User.GetUserRole();
        var command = new CreateGalleryImageCommand(role, shrineId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // PUT /api/shrines/cms/gallery/{imageId} (image in body)
    // Update image
    [HttpPut("cms/gallery/{imageId}")]
    public async Task<IActionResult> UpdateGalleryImage([FromRoute] int imageId, [FromForm] UpdateImageFormRequest request)
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

    // DELETE /api/shrines/cms/delete/{shrineId}
    [HttpDelete("cms/delete/{shrineId}")]
    [Authorize(Roles = "Admin")]    // Admins only
    public async Task<IActionResult> DeleteShrineAsync([FromRoute] int shrineId)
    {
        var command = new DeleteShrineCommand(shrineId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    #endregion

    #region SUBMIT REVIEW SHRINE

    // POST /api/shrines/cms/{shrineId}/review/submit
    [HttpPost("cms/{shrineId}/review/submit")]
    public async Task<IActionResult> SubmitReviewShrineAsync([FromRoute] int shrineId)
    {
        var userId = User.GetUserId();
        var command = new SubmitReviewShrineCommand(shrineId, userId);
        await _mediator.Send(command);
        return NoContent();
    }

    #endregion

    #region REJECT REVIEW SHRINE

    // POST /api/shrines/cms/{shrineId}/review/reject
    [HttpPost("cms/{shrineId}/review/reject")]
    [Authorize(Roles = "Admin")]    // Admins only
    public async Task<IActionResult> RejectReviewShrineAsync([FromRoute] int shrineId, [FromBody] RejectShrineRequest request)
    {
        var userId = User.GetUserId();
        var command = new RejectReviewShrineCommand(shrineId, userId, request.Message);
        await _mediator.Send(command);
        return NoContent();
    }

    #endregion

    #region PUBLISH REVIEW SHRINE

    // POST /api/shrines/cms/{shrineId}/review/publish
    [HttpPost("cms/{shrineId}/review/publish")]
    [Authorize(Roles = "Admin")]    // Admins only
    public async Task<IActionResult> PublishReviewShrineAsync([FromRoute] int shrineId)
    {
        var userId = User.GetUserId();
        var command = new PublishReviewShrineCommand(shrineId, userId);
        await _mediator.Send(command);
        return NoContent();
    }

    #endregion
}


#region Meta Request

public record UpdateShrineMetaRequest(
    BasicMetaUpdateRequest Basic,
    TagLinkChangesRequest Tags
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
public record TagLinkChangesRequest(
    IReadOnlyList<int> Link,
    IReadOnlyList<int> Unlink
);

#endregion

#region Images Request

public record UpdateImageRequest(
    int ImgId,
    string? ImageUrl,
    string? Title,
    string? Desc,
    CitationRequest? Citation
);
public record ImageChangeRequest(
    string Action,
    string? ImageUrl,
    string? Title,
    string? Desc,
    CitationRequest? Citation
);
public record CreateImageRequest(
    string? ImageUrl,
    string? Title,
    string? Desc,
    CreateCitationRequest? Citation
);

public record CreateImageFormRequest(
    string? ImageUrl,
    string? Title,
    string? Desc,
    CreateCitationRequest? Citation,
    IFormFile? File
);

public record UpdateImageFormRequest(
    int ImgId,
    string? ImageUrl,
    string? Title,
    string? Desc,
    CitationRequest? Citation,
    IFormFile? File
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

public record CreateCitationRequest(
    string? Title,
    string? Author,
    string? Url,
    int? Year
);

public record LinkExistingCitationRequest(
    int CiteId,
    string? Title,
    string? Author,
    string? Url,
    int? Year
);

public record CitationCreateChangesRequest(
    IReadOnlyList<CreateCitationRequest> Create,
    IReadOnlyList<LinkExistingCitationRequest>? LinkExisting
);

public record CitationListChangesRequest(
    IReadOnlyList<CreateCitationRequest> Create,
    IReadOnlyList<CitationRequest> Update,
    IReadOnlyList<LinkExistingCitationRequest> LinkExisting,
    IReadOnlyList<int> Delete
);

#endregion

#region Notes Request

public record UpdateShrineNotesRequest(string Notes);

#endregion

#region Kami Request

// CREATE KAMI
public record CreateKamiInShrineRequest(
    string? NameEn,
    string? NameJp,
    string? Desc,
    CreateImageRequest? Image,
    CitationCreateChangesRequest Citations
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
    CitationCreateChangesRequest Citations
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
    CitationCreateChangesRequest Citations
);

// UPDATE FOLKLORE
public record UpdateFolkloreRequest(
    BasicFolkloreUpdateRequest Basic,
    ImageChangeRequest Image,
    CitationListChangesRequest Citations
);
public record BasicFolkloreUpdateRequest(
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

#region Reject Shrine Request

public record RejectShrineRequest(string Message);

#endregion