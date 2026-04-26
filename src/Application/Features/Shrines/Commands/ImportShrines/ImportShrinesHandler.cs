using System.ComponentModel.DataAnnotations;
using Application.Common.Policies;
using Application.Features.Audits.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.ImportShrines;

public class ImportShrinesHandler : IRequestHandler<ImportShrinesCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IShrineReadService _shrineReadService;
    private readonly IAuditService _audit;

    public ImportShrinesHandler(
        IShrineWriteService shrineWriteService,
        IShrineReadService shrineReadService,
        IAuditService audit
    )
    {
        _shrineWriteService = shrineWriteService;
        _shrineReadService = shrineReadService;
        _audit = audit;
    }

    public async Task<Unit> Handle(ImportShrinesCommand request, CancellationToken ct)
    {
        try
        {
            var previews = request.Request.Previews;

            if (previews is null || previews.Count == 0)
                throw new ValidationException("At least one shrine preview is required.");

            // basic item validation
            foreach (var preview in previews)
            {
                if (string.IsNullOrWhiteSpace(preview.ImportId))
                    throw new ValidationException("Each preview must include an import ID.");

                if (preview.Lat is < -90 or > 90)
                    throw new ValidationException($"Invalid latitude for import ID '{preview.ImportId}'.");

                if (preview.Lon is < -180 or > 180)
                    throw new ValidationException($"Invalid longitude for import ID '{preview.ImportId}'.");
            }

            // request-level duplicate check
            var duplicateRequestIds = previews
                .GroupBy(p => p.ImportId.Trim(), StringComparer.OrdinalIgnoreCase) // Ordinal compares unicode
                .Where(g => g.Count() > 1)
                .Select(g => g.Key) // Key = ImportId
                .ToList();

            if (duplicateRequestIds.Count > 0)
                throw new ValidationException(
                    $"Duplicate import IDs found in request: {string.Join(", ", duplicateRequestIds)}");

            var importIds = previews
                .Select(p => p.ImportId.Trim())
                .ToList();

            var existingImportIds = await _shrineReadService.GetExistingImportIdsAsync(importIds, ct);

            // FILTER OUT already imported
            var filteredPreviews = previews
                .Where(p => !existingImportIds.Contains(p.ImportId.Trim(), StringComparer.OrdinalIgnoreCase))
                .ToList();

            // nothing new to import
            if (filteredPreviews.Count == 0)
                return Unit.Value;

            var filteredRequest = new ImportShrinesRequest(filteredPreviews);

            await _shrineWriteService.ImportShrinesAsync(filteredRequest, ct);

            await _audit.LogAsync(request.UserId, request.Username, "ImportedShrines", "Shrine Management", true, null, ct);
        }
        catch (Exception e)
        {
            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "ImportedShrines", "Shrine Management", false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return Unit.Value;
    }
}