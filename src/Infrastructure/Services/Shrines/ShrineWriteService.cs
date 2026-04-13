using System.ComponentModel.DataAnnotations;
using Application.Common.Exceptions;
using Application.Features.Shrines.Services;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Shrines;

public class ShrineWriteService : IShrineWriteService
{
    private readonly AppDbContext _db;

    public ShrineWriteService(AppDbContext db)
    {
        _db = db;
    }

    #region CITATION HELPERS

    private static Citation BuildCitation(CreateCitationRequest request)
    {
        return new Citation
        {
            Title = request.Title,
            Author = request.Author,
            Url = request.Url,
            Year = request.Year
        };
    }

    private async Task<Citation> GetExistingCitationOrThrowAsync(int citeId, CancellationToken ct)
    {
        var citation = await _db.Citations
            .FirstOrDefaultAsync(c => c.CiteId == citeId, ct);

        if (citation is null)
            throw new NotFoundException($"Citation {citeId} was not found.");

        return citation;
    }

    #endregion

    #region UPDATE META

    public async Task UpdateShrineMetaAsync(int shrineId, UpdateShrineMetaRequest request, CancellationToken ct)
    {
        // Load shrine / related data
        var shrine = await _db.Shrines
        .Include(s => s.Image!)
            .ThenInclude(i => i.Citation)
        .Include(s => s.ShrineTags)
            .ThenInclude(st => st.Tag)
        .FirstOrDefaultAsync(s => s.ShrineId == shrineId, ct);

        // validate shrine exists
        if (shrine is null)
            throw new NotFoundException("Shrine not found.");

        // update basic fields
        shrine.Slug = request.Basic.Slug;
        shrine.NameEn = request.Basic.NameEn;
        shrine.NameJp = request.Basic.NameJp;
        shrine.ShrineDesc = request.Basic.ShrineDesc;

        shrine.Lat = request.Basic.Lat;
        shrine.Lon = request.Basic.Lon;

        shrine.Prefecture = request.Basic.Prefecture;
        shrine.City = request.Basic.City;
        shrine.Ward = request.Basic.Ward;
        shrine.Locality = request.Basic.Locality;
        shrine.PostalCode = request.Basic.PostalCode;
        shrine.Country = request.Basic.Country;

        shrine.PhoneNumber = request.Basic.PhoneNumber;
        shrine.Email = request.Basic.Email;
        shrine.Website = request.Basic.Website;

        // Unlink Tags
        if (request.Tags.Unlink.Count > 0)
        {
            var shrineTagsToRemove = shrine.ShrineTags
                .Where(st => request.Tags.Unlink.Contains(st.TagId))
                .ToList();

            foreach (var shrineTag in shrineTagsToRemove)
            {
                shrine.ShrineTags.Remove(shrineTag);
            }
        }

        // Link Tags
        if (request.Tags.Link.Count > 0)
        {
            var existingLinkedTagIds = shrine.ShrineTags
                .Select(st => st.TagId)
                .ToHashSet();

            var tagIdsToLink = request.Tags.Link
                .Where(tagId => !existingLinkedTagIds.Contains(tagId))
                .Distinct()
                .ToList();

            foreach (var tagId in tagIdsToLink)
            {
                shrine.ShrineTags.Add(new ShrineTag
                {
                    ShrineId = shrine.ShrineId,
                    Shrine = shrine,
                    TagId = tagId
                });
            }
        }

        // Delete Image
        if (request.HeroImage.Action == "delete")
        {
            shrine.Image = null;
            shrine.ImgId = null;
        }

        // Create Image
        if (request.HeroImage.Action == "create")
        {
            var image = new Image
            {
                ImageUrl = request.HeroImage.ImageUrl,
                Title = request.HeroImage.Title,
                Desc = request.HeroImage.Desc
            };

            if (request.HeroImage.Citation is not null)
            {
                image.Citation = new Citation
                {
                    Title = request.HeroImage.Citation.Title,
                    Author = request.HeroImage.Citation.Author,
                    Url = request.HeroImage.Citation.Url,
                    Year = request.HeroImage.Citation.Year
                };
            }

            shrine.Image = image;
        }

        // Update Image
        if (request.HeroImage.Action == "update")
        {
            if (shrine.Image is null)
                throw new NotFoundException("Hero image not found.");

            shrine.Image.ImageUrl = request.HeroImage.ImageUrl;
            shrine.Image.Title = request.HeroImage.Title;
            shrine.Image.Desc = request.HeroImage.Desc;

            if (request.HeroImage.Citation is null)
            {
                shrine.Image.Citation = null;
                shrine.Image.CiteId = null;
            }
            else if (shrine.Image.Citation is null)
            {
                shrine.Image.Citation = new Citation
                {
                    Title = request.HeroImage.Citation.Title,
                    Author = request.HeroImage.Citation.Author,
                    Url = request.HeroImage.Citation.Url,
                    Year = request.HeroImage.Citation.Year
                };
            }
            else
            {
                shrine.Image.Citation.Title = request.HeroImage.Citation.Title;
                shrine.Image.Citation.Author = request.HeroImage.Citation.Author;
                shrine.Image.Citation.Url = request.HeroImage.Citation.Url;
                shrine.Image.Citation.Year = request.HeroImage.Citation.Year;
            }
        }

        // force shrine timestamp to update                                                                                                                  
        shrine.UpdatedAt = DateTime.UtcNow;
        _db.Entry(shrine).Property(s => s.UpdatedAt).IsModified = true;

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region UPDATE NOTES

    public async Task UpdateShrineNotesAsync(int shrineId, string notes, CancellationToken ct)
    {
        // Load shrine
        var shrine = await _db.Shrines.FirstOrDefaultAsync(s => s.ShrineId == shrineId, ct);

        // validate shrine exists
        if (shrine is null)
            throw new NotFoundException("Shrine not found.");

        // update notes
        shrine.Notes = notes;

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region CREATE KAMI

    public async Task CreateKamiInShrineAsync(
        int shrineId,
        CreateKamiInShrineRequest request,
        CancellationToken ct
    )
    {
        // Load shrine
        var shrine = await _db.Shrines
            .Include(s => s.ShrineKamis)
            .FirstOrDefaultAsync(s => s.ShrineId == shrineId, ct);

        // Validate shrine exists
        if (shrine is null)
            throw new NotFoundException("Shrine not found.");

        // Create kami
        var kami = new Kami
        {
            NameEn = request.NameEn,
            NameJp = request.NameJp,
            Desc = request.Desc,
            Status = "draft"
        };

        // Create hero image if provided
        if (request.Image is not null)
        {
            var image = new Image
            {
                ImageUrl = request.Image.ImageUrl,
                Title = request.Image.Title,
                Desc = request.Image.Desc
            };

            if (request.Image.Citation is not null)
            {
                image.Citation = new Citation
                {
                    Title = request.Image.Citation.Title,
                    Author = request.Image.Citation.Author,
                    Url = request.Image.Citation.Url,
                    Year = request.Image.Citation.Year
                };
            }

            kami.Image = image;
        }

        // Create new citations
        if (request.Citations.Create.Count > 0)
        {
            foreach (var citationRequest in request.Citations.Create)
            {
                var citation = BuildCitation(citationRequest);

                kami.KamiCitations.Add(new KamiCitation
                {
                    Kami = kami,
                    Citation = citation
                });
            }
        }

        // Link existing citations
        if (request.Citations.LinkExisting.Count > 0)
        {
            foreach (var citationRequest in request.Citations.LinkExisting)
            {
                var citation = await GetExistingCitationOrThrowAsync(citationRequest.CiteId, ct);

                // apply any edits sent along with the reuse request
                citation.Title = citationRequest.Title;
                citation.Author = citationRequest.Author;
                citation.Url = citationRequest.Url;
                citation.Year = citationRequest.Year;

                var alreadyLinked = kami.KamiCitations.Any(kc => kc.CiteId == citation.CiteId);
                if (alreadyLinked) continue;

                kami.KamiCitations.Add(new KamiCitation
                {
                    Kami = kami,
                    Citation = citation,
                    CiteId = citation.CiteId
                });
            }
        }

        // Link kami to shrine
        shrine.ShrineKamis.Add(new ShrineKami
        {
            ShrineId = shrine.ShrineId,
            Shrine = shrine,
            Kami = kami
        });

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region LINK KAMI

    public async Task LinkKamiToShrineAsync(int shrineId, int kamiId, CancellationToken ct)
    {
        // Load shrine with existing kami links
        var shrine = await _db.Shrines
            .Include(s => s.ShrineKamis)
            .FirstOrDefaultAsync(s => s.ShrineId == shrineId, ct);

        if (shrine is null)
            throw new NotFoundException("Shrine not found.");

        // Validate kami exists
        var kamiExists = await _db.Kamis
            .AnyAsync(k => k.KamiId == kamiId, ct);

        if (!kamiExists)
            throw new NotFoundException("Kami not found.");

        // Prevent duplicate link
        var alreadyLinked = shrine.ShrineKamis.Any(sk => sk.KamiId == kamiId);

        if (alreadyLinked)
            return;

        // Create link
        shrine.ShrineKamis.Add(new ShrineKami
        {
            ShrineId = shrineId,
            KamiId = kamiId
        });

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region UNLINK KAMI

    public async Task UnlinkKamiToShrineAsync(int shrineId, int kamiId, CancellationToken ct)
    {
        var shrineExists = await _db.Shrines
            .AnyAsync(s => s.ShrineId == shrineId, ct);

        if (!shrineExists)
            throw new NotFoundException("Shrine not found.");

        var shrineKami = await _db.Set<ShrineKami>()
            .FirstOrDefaultAsync(
                sk => sk.ShrineId == shrineId && sk.KamiId == kamiId,
                ct
            );

        if (shrineKami is null)
            throw new NotFoundException("Kami link not found for this shrine.");

        _db.Set<ShrineKami>().Remove(shrineKami);

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region UPDATE KAMI

    public async Task UpdateKamiAsync(int kamiId, UpdateKamiRequest request, CancellationToken ct)
    {
        // Load kami / related data
        var kami = await _db.Kamis
            .Include(k => k.Image!)
                .ThenInclude(i => i.Citation)
            .Include(k => k.KamiCitations)
                .ThenInclude(kc => kc.Citation)
            .FirstOrDefaultAsync(k => k.KamiId == kamiId, ct);

        // Validate kami exists
        if (kami is null)
            throw new NotFoundException("Kami not found.");

        // Update basic fields
        kami.NameEn = request.Basic.NameEn;
        kami.NameJp = request.Basic.NameJp;
        kami.Desc = request.Basic.Desc;

        // Delete image
        if (request.Image.Action == "delete")
        {
            kami.Image = null;
            kami.ImgId = null;
        }

        // Create image
        if (request.Image.Action == "create")
        {
            var image = new Image
            {
                ImageUrl = request.Image.ImageUrl,
                Title = request.Image.Title,
                Desc = request.Image.Desc
            };

            if (request.Image.Citation is not null)
            {
                image.Citation = new Citation
                {
                    Title = request.Image.Citation.Title,
                    Author = request.Image.Citation.Author,
                    Url = request.Image.Citation.Url,
                    Year = request.Image.Citation.Year
                };
            }

            kami.Image = image;
        }

        // Update image
        if (request.Image.Action == "update")
        {
            if (kami.Image is null)
                throw new NotFoundException("Kami image not found.");

            kami.Image.ImageUrl = request.Image.ImageUrl;
            kami.Image.Title = request.Image.Title;
            kami.Image.Desc = request.Image.Desc;

            if (request.Image.Citation is null)
            {
                kami.Image.Citation = null;
                kami.Image.CiteId = null;
            }
            else if (kami.Image.Citation is null)
            {
                kami.Image.Citation = new Citation
                {
                    Title = request.Image.Citation.Title,
                    Author = request.Image.Citation.Author,
                    Url = request.Image.Citation.Url,
                    Year = request.Image.Citation.Year
                };
            }
            else
            {
                kami.Image.Citation.Title = request.Image.Citation.Title;
                kami.Image.Citation.Author = request.Image.Citation.Author;
                kami.Image.Citation.Url = request.Image.Citation.Url;
                kami.Image.Citation.Year = request.Image.Citation.Year;
            }
        }

        // Delete citations (unlink only)
        if (request.Citations.Delete.Count > 0)
        {
            var kamiCitationsToRemove = kami.KamiCitations
                .Where(kc => request.Citations.Delete.Contains(kc.CiteId))
                .ToList();

            foreach (var kamiCitation in kamiCitationsToRemove)
            {
                kami.KamiCitations.Remove(kamiCitation);
                _db.Set<KamiCitation>().Remove(kamiCitation);
            }
        }

        // Create citations
        if (request.Citations.Create.Count > 0)
        {
            foreach (var citationRequest in request.Citations.Create)
            {
                var citation = new Citation
                {
                    Title = citationRequest.Title,
                    Author = citationRequest.Author,
                    Url = citationRequest.Url,
                    Year = citationRequest.Year
                };

                kami.KamiCitations.Add(new KamiCitation
                {
                    KamiId = kami.KamiId,
                    Kami = kami,
                    Citation = citation
                });
            }
        }

        // Link existing citations
        if (request.Citations.LinkExisting.Count > 0)
        {
            foreach (var citationRequest in request.Citations.LinkExisting)
            {
                var citation = await GetExistingCitationOrThrowAsync(citationRequest.CiteId, ct);

                citation.Title = citationRequest.Title;
                citation.Author = citationRequest.Author;
                citation.Url = citationRequest.Url;
                citation.Year = citationRequest.Year;

                var alreadyLinked = kami.KamiCitations.Any(kc => kc.CiteId == citation.CiteId);
                if (alreadyLinked) continue;

                kami.KamiCitations.Add(new KamiCitation
                {
                    KamiId = kami.KamiId,
                    Kami = kami,
                    Citation = citation,
                    CiteId = citation.CiteId
                });
            }
        }

        // Update citations
        if (request.Citations.Update.Count > 0)
        {
            foreach (var citationRequest in request.Citations.Update)
            {
                var existingKamiCitation = kami.KamiCitations
                    .FirstOrDefault(kc => kc.CiteId == citationRequest.CiteId);

                if (existingKamiCitation is null)
                    continue;

                existingKamiCitation.Citation.Title = citationRequest.Title;
                existingKamiCitation.Citation.Author = citationRequest.Author;
                existingKamiCitation.Citation.Url = citationRequest.Url;
                existingKamiCitation.Citation.Year = citationRequest.Year;
            }
        }

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region CREATE HISTORY

    public async Task CreateHistoryAsync(int shrineId, CreateHistoryRequest request, CancellationToken ct)
    {
        // Load shrine
        var shrine = await _db.Shrines
            .Include(s => s.ShrineHistories)
            .FirstOrDefaultAsync(s => s.ShrineId == shrineId, ct);

        // Validate shrine exists
        if (shrine is null)
            throw new NotFoundException("Shrine not found.");

        // Create History
        var history = new History
        {
            ShrineId = shrine.ShrineId,
            EventDate = request.EventDate,
            SortOrder = request.SortOrder,
            Title = request.Title,
            Information = request.Information,
            Status = "draft"
        };

        // Create hero image if provided
        if (request.Image is not null)
        {
            var image = new Image
            {
                ImageUrl = request.Image.ImageUrl,
                Title = request.Image.Title,
                Desc = request.Image.Desc
            };

            if (request.Image.Citation is not null)
            {
                image.Citation = new Citation
                {
                    Title = request.Image.Citation.Title,
                    Author = request.Image.Citation.Author,
                    Url = request.Image.Citation.Url,
                    Year = request.Image.Citation.Year
                };
            }

            history.Image = image;
        }

        // Create new citations
        if (request.Citations.Create.Count > 0)
        {
            foreach (var citationRequest in request.Citations.Create)
            {
                var citation = BuildCitation(citationRequest);

                history.HistoryCitations.Add(new HistoryCitation
                {
                    History = history,
                    Citation = citation
                });
            }
        }

        // Link existing citations
        if (request.Citations.LinkExisting.Count > 0)
        {
            foreach (var citationRequest in request.Citations.LinkExisting)
            {
                var citation = await GetExistingCitationOrThrowAsync(citationRequest.CiteId, ct);

                citation.Title = citationRequest.Title;
                citation.Author = citationRequest.Author;
                citation.Url = citationRequest.Url;
                citation.Year = citationRequest.Year;

                var alreadyLinked = history.HistoryCitations.Any(hc => hc.CiteId == citation.CiteId);
                if (alreadyLinked) continue;

                history.HistoryCitations.Add(new HistoryCitation
                {
                    History = history,
                    Citation = citation,
                    CiteId = citation.CiteId
                });
            }
        }

        // Link history to shrine
        shrine.ShrineHistories.Add(history);

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region DELETE HISTORY

    public async Task DeleteHistoryAsync(int historyId, CancellationToken ct)
    {
        // Load history with related data
        var history = await _db.Histories
            .Include(h => h.Image!)
                .ThenInclude(i => i.Citation)
            .Include(h => h.HistoryCitations)
                .ThenInclude(hc => hc.Citation)
            .FirstOrDefaultAsync(h => h.HistoryId == historyId, ct);

        // Validate history exists
        if (history is null)
            throw new NotFoundException("History not found.");

        // Remove history citation links only
        if (history.HistoryCitations.Count > 0)
        {
            foreach (var historyCitation in history.HistoryCitations.ToList())
            {
                _db.Set<HistoryCitation>().Remove(historyCitation);
            }
        }

        // Remove image citation if present
        if (history.Image?.Citation is not null)
        {
            _db.Citations.Remove(history.Image.Citation);
        }

        // Remove image if present
        if (history.Image is not null)
        {
            _db.Images.Remove(history.Image);
        }

        // Remove history
        _db.Histories.Remove(history);

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region UPDATE HISTORY

    public async Task UpdateHistoryAsync(int historyId, UpdateHistoryRequest request, CancellationToken ct)
    {
        // Load history / related data
        var history = await _db.Histories
            .Include(h => h.Image!)
                .ThenInclude(i => i.Citation)
            .Include(h => h.HistoryCitations)
                .ThenInclude(hc => hc.Citation)
            .FirstOrDefaultAsync(h => h.HistoryId == historyId, ct);

        // Validate history exists
        if (history is null)
            throw new NotFoundException("History not found.");

        // Update basic fields
        history.EventDate = request.Basic.EventDate;
        history.SortOrder = request.Basic.SortOrder;
        history.Title = request.Basic.Title;
        history.Information = request.Basic.Information;

        // Delete image
        if (request.Image.Action == "delete" && history.Image is not null)
        {
            if (history.Image.Citation is not null)
                _db.Citations.Remove(history.Image.Citation);

            _db.Images.Remove(history.Image);
            history.Image = null;
            history.ImgId = null;
        }

        // Create image
        if (request.Image.Action == "create")
        {
            var image = new Image
            {
                ImageUrl = request.Image.ImageUrl,
                Title = request.Image.Title,
                Desc = request.Image.Desc
            };

            if (request.Image.Citation is not null)
            {
                image.Citation = new Citation
                {
                    Title = request.Image.Citation.Title,
                    Author = request.Image.Citation.Author,
                    Url = request.Image.Citation.Url,
                    Year = request.Image.Citation.Year
                };
            }

            history.Image = image;
        }

        // Update image
        if (request.Image.Action == "update")
        {
            if (history.Image is null)
                throw new NotFoundException("History image not found.");

            history.Image.ImageUrl = request.Image.ImageUrl;
            history.Image.Title = request.Image.Title;
            history.Image.Desc = request.Image.Desc;

            if (request.Image.Citation is null)
            {
                history.Image.Citation = null;
                history.Image.CiteId = null;
            }
            else if (history.Image.Citation is null)
            {
                history.Image.Citation = new Citation
                {
                    Title = request.Image.Citation.Title,
                    Author = request.Image.Citation.Author,
                    Url = request.Image.Citation.Url,
                    Year = request.Image.Citation.Year
                };
            }
            else
            {
                history.Image.Citation.Title = request.Image.Citation.Title;
                history.Image.Citation.Author = request.Image.Citation.Author;
                history.Image.Citation.Url = request.Image.Citation.Url;
                history.Image.Citation.Year = request.Image.Citation.Year;
            }
        }

        // Delete citations (unlink only)
        if (request.Citations.Delete.Count > 0)
        {
            var historyCitationsToRemove = history.HistoryCitations
                .Where(hc => request.Citations.Delete.Contains(hc.CiteId))
                .ToList();

            foreach (var historyCitation in historyCitationsToRemove)
            {
                history.HistoryCitations.Remove(historyCitation);
                _db.Set<HistoryCitation>().Remove(historyCitation);
            }
        }

        // Create citations
        if (request.Citations.Create.Count > 0)
        {
            foreach (var citationRequest in request.Citations.Create)
            {
                var citation = new Citation
                {
                    Title = citationRequest.Title,
                    Author = citationRequest.Author,
                    Url = citationRequest.Url,
                    Year = citationRequest.Year
                };

                history.HistoryCitations.Add(new HistoryCitation
                {
                    HistoryId = history.HistoryId,
                    History = history,
                    Citation = citation
                });
            }
        }

        // Link existing citations
        if (request.Citations.LinkExisting.Count > 0)
        {
            foreach (var citationRequest in request.Citations.LinkExisting)
            {
                var citation = await GetExistingCitationOrThrowAsync(citationRequest.CiteId, ct);

                citation.Title = citationRequest.Title;
                citation.Author = citationRequest.Author;
                citation.Url = citationRequest.Url;
                citation.Year = citationRequest.Year;

                var alreadyLinked = history.HistoryCitations.Any(hc => hc.CiteId == citation.CiteId);
                if (alreadyLinked) continue;

                history.HistoryCitations.Add(new HistoryCitation
                {
                    HistoryId = history.HistoryId,
                    History = history,
                    Citation = citation,
                    CiteId = citation.CiteId
                });
            }
        }

        // Update citations
        if (request.Citations.Update.Count > 0)
        {
            foreach (var citationRequest in request.Citations.Update)
            {
                var existingHistoryCitation = history.HistoryCitations
                    .FirstOrDefault(hc => hc.CiteId == citationRequest.CiteId);

                if (existingHistoryCitation is null)
                    continue;

                existingHistoryCitation.Citation.Title = citationRequest.Title;
                existingHistoryCitation.Citation.Author = citationRequest.Author;
                existingHistoryCitation.Citation.Url = citationRequest.Url;
                existingHistoryCitation.Citation.Year = citationRequest.Year;
            }
        }

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region CREATE FOLKLORE

    public async Task CreateFolkloreAsync(int shrineId, CreateFolkloreRequest request, CancellationToken ct)
    {
        // Load shrine
        var shrine = await _db.Shrines
            .Include(s => s.ShrineFolklores)
            .FirstOrDefaultAsync(s => s.ShrineId == shrineId, ct);

        // Validate shrine exists
        if (shrine is null)
            throw new NotFoundException("Shrine not found.");

        // Create Folklore
        var folklore = new Folklore
        {
            ShrineId = shrine.ShrineId,
            SortOrder = request.SortOrder,
            Title = request.Title,
            Information = request.Information,
            Status = "draft"
        };

        // Create hero image if provided
        if (request.Image is not null)
        {
            var image = new Image
            {
                ImageUrl = request.Image.ImageUrl,
                Title = request.Image.Title,
                Desc = request.Image.Desc
            };

            if (request.Image.Citation is not null)
            {
                image.Citation = new Citation
                {
                    Title = request.Image.Citation.Title,
                    Author = request.Image.Citation.Author,
                    Url = request.Image.Citation.Url,
                    Year = request.Image.Citation.Year
                };
            }

            folklore.Image = image;
        }

        // Create new citations
        if (request.Citations.Create.Count > 0)
        {
            foreach (var citationRequest in request.Citations.Create)
            {
                var citation = BuildCitation(citationRequest);

                folklore.FolkloreCitations.Add(new FolkloreCitation
                {
                    Folklore = folklore,
                    Citation = citation
                });
            }
        }

        // Link existing citations
        if (request.Citations.LinkExisting.Count > 0)
        {
            foreach (var citationRequest in request.Citations.LinkExisting)
            {
                var citation = await GetExistingCitationOrThrowAsync(citationRequest.CiteId, ct);

                citation.Title = citationRequest.Title;
                citation.Author = citationRequest.Author;
                citation.Url = citationRequest.Url;
                citation.Year = citationRequest.Year;

                var alreadyLinked = folklore.FolkloreCitations.Any(fc => fc.CiteId == citation.CiteId);
                if (alreadyLinked) continue;

                folklore.FolkloreCitations.Add(new FolkloreCitation
                {
                    Folklore = folklore,
                    Citation = citation,
                    CiteId = citation.CiteId
                });
            }
        }

        // Link folklore to shrine
        shrine.ShrineFolklores.Add(folklore);

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region DELETE FOLKLORE

    public async Task DeleteFolkloreAsync(int folkloreId, CancellationToken ct)
    {
        // Load folklore with related data
        var folklore = await _db.Folklores
            .Include(f => f.Image!)
                .ThenInclude(i => i.Citation)
            .Include(f => f.FolkloreCitations)
                .ThenInclude(fc => fc.Citation)
            .FirstOrDefaultAsync(f => f.FolkloreId == folkloreId, ct);

        // Validate folklore exists
        if (folklore is null)
            throw new NotFoundException("Folklore not found.");

        // Remove folklore citation links only
        if (folklore.FolkloreCitations.Count > 0)
        {
            foreach (var folkloreCitation in folklore.FolkloreCitations.ToList())
            {
                _db.Set<FolkloreCitation>().Remove(folkloreCitation);
            }
        }

        // Remove image citation if present
        if (folklore.Image?.Citation is not null)
        {
            _db.Citations.Remove(folklore.Image.Citation);
        }

        // Remove image if present
        if (folklore.Image is not null)
        {
            _db.Images.Remove(folklore.Image);
        }

        // Remove folklore
        _db.Folklores.Remove(folklore);

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region UPDATE FOLKLORE

    public async Task UpdateFolkloreAsync(int folkloreId, UpdateFolkloreRequest request, CancellationToken ct)
    {
        // Load folklore / related data
        var folklore = await _db.Folklores
            .Include(f => f.Image!)
                .ThenInclude(i => i.Citation)
            .Include(f => f.FolkloreCitations)
                .ThenInclude(fc => fc.Citation)
            .FirstOrDefaultAsync(f => f.FolkloreId == folkloreId, ct);

        // Validate folklore exists
        if (folklore is null)
            throw new NotFoundException("Folklore not found.");

        // Update basic fields
        folklore.SortOrder = request.Basic.SortOrder;
        folklore.Title = request.Basic.Title;
        folklore.Information = request.Basic.Information;

        // Delete image
        if (request.Image.Action == "delete" && folklore.Image is not null)
        {
            if (folklore.Image.Citation is not null)
                _db.Citations.Remove(folklore.Image.Citation);

            _db.Images.Remove(folklore.Image);
            folklore.Image = null;
            folklore.ImgId = null;
        }

        // Create image
        if (request.Image.Action == "create")
        {
            var image = new Image
            {
                ImageUrl = request.Image.ImageUrl,
                Title = request.Image.Title,
                Desc = request.Image.Desc
            };

            if (request.Image.Citation is not null)
            {
                image.Citation = new Citation
                {
                    Title = request.Image.Citation.Title,
                    Author = request.Image.Citation.Author,
                    Url = request.Image.Citation.Url,
                    Year = request.Image.Citation.Year
                };
            }

            folklore.Image = image;
        }

        // Update image
        if (request.Image.Action == "update")
        {
            if (folklore.Image is null)
                throw new NotFoundException("Folklore image not found.");

            folklore.Image.ImageUrl = request.Image.ImageUrl;
            folklore.Image.Title = request.Image.Title;
            folklore.Image.Desc = request.Image.Desc;

            if (request.Image.Citation is null)
            {
                folklore.Image.Citation = null;
                folklore.Image.CiteId = null;
            }
            else if (folklore.Image.Citation is null)
            {
                folklore.Image.Citation = new Citation
                {
                    Title = request.Image.Citation.Title,
                    Author = request.Image.Citation.Author,
                    Url = request.Image.Citation.Url,
                    Year = request.Image.Citation.Year
                };
            }
            else
            {
                folklore.Image.Citation.Title = request.Image.Citation.Title;
                folklore.Image.Citation.Author = request.Image.Citation.Author;
                folklore.Image.Citation.Url = request.Image.Citation.Url;
                folklore.Image.Citation.Year = request.Image.Citation.Year;
            }
        }

        // Delete citations (unlink only)
        if (request.Citations.Delete.Count > 0)
        {
            var folkloreCitationsToRemove = folklore.FolkloreCitations
                .Where(fc => request.Citations.Delete.Contains(fc.CiteId))
                .ToList();

            foreach (var folkloreCitation in folkloreCitationsToRemove)
            {
                folklore.FolkloreCitations.Remove(folkloreCitation);
                _db.Set<FolkloreCitation>().Remove(folkloreCitation);
            }
        }

        // Create citations
        if (request.Citations.Create.Count > 0)
        {
            foreach (var citationRequest in request.Citations.Create)
            {
                var citation = new Citation
                {
                    Title = citationRequest.Title,
                    Author = citationRequest.Author,
                    Url = citationRequest.Url,
                    Year = citationRequest.Year
                };

                folklore.FolkloreCitations.Add(new FolkloreCitation
                {
                    FolkloreId = folklore.FolkloreId,
                    Folklore = folklore,
                    Citation = citation
                });
            }
        }

        // Link existing citations
        if (request.Citations.LinkExisting.Count > 0)
        {
            foreach (var citationRequest in request.Citations.LinkExisting)
            {
                var citation = await GetExistingCitationOrThrowAsync(citationRequest.CiteId, ct);

                citation.Title = citationRequest.Title;
                citation.Author = citationRequest.Author;
                citation.Url = citationRequest.Url;
                citation.Year = citationRequest.Year;

                var alreadyLinked = folklore.FolkloreCitations.Any(fc => fc.CiteId == citation.CiteId);
                if (alreadyLinked) continue;

                folklore.FolkloreCitations.Add(new FolkloreCitation
                {
                    FolkloreId = folklore.FolkloreId,
                    Folklore = folklore,
                    Citation = citation,
                    CiteId = citation.CiteId
                });
            }
        }

        // Update citations
        if (request.Citations.Update.Count > 0)
        {
            foreach (var citationRequest in request.Citations.Update)
            {
                var existingFolkloreCitation = folklore.FolkloreCitations
                    .FirstOrDefault(hc => hc.CiteId == citationRequest.CiteId);

                if (existingFolkloreCitation is null)
                    continue;

                existingFolkloreCitation.Citation.Title = citationRequest.Title;
                existingFolkloreCitation.Citation.Author = citationRequest.Author;
                existingFolkloreCitation.Citation.Url = citationRequest.Url;
                existingFolkloreCitation.Citation.Year = citationRequest.Year;
            }
        }

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region CREATE GALLERY IMAGE

    public async Task CreateGalleryImageAsync(
        int shrineId,
        CreateGalleryImageFormRequest request,
        string publicId,
        CancellationToken ct
    )
    {
        var shrine = await _db.Shrines
            .Include(s => s.ShrineGalleries)
            .FirstOrDefaultAsync(s => s.ShrineId == shrineId, ct);

        if (shrine is null)
            throw new NotFoundException("Shrine not found.");

        var image = new Image
        {
            PublicId = publicId,
            ImageUrl = request.ImageUrl,
            Title = request.Title,
            Desc = request.Desc
        };

        if (request.Citation is not null)
        {
            image.Citation = new Citation
            {
                Title = request.Citation.Title,
                Author = request.Citation.Author,
                Url = request.Citation.Url,
                Year = request.Citation.Year
            };
        }

        shrine.ShrineGalleries.Add(new ShrineGallery
        {
            ShrineId = shrine.ShrineId,
            Shrine = shrine,
            Image = image
        });

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region DELETE GALLERY IMAGE

    public async Task DeleteGalleryImageAsync(int imageId, CancellationToken ct)
    {
        var image = await _db.Images
            .Include(i => i.Citation)
            .FirstOrDefaultAsync(i => i.ImgId == imageId, ct);

        if (image is null)
            throw new NotFoundException("Gallery image not found.");

        var shrineGalleryLinks = await _db.ShrineGalleries
            .Where(sg => sg.ImgId == imageId)
            .ToListAsync(ct);

        if (shrineGalleryLinks.Count > 0)
        {
            _db.ShrineGalleries.RemoveRange(shrineGalleryLinks);
        }

        if (image.Citation is not null)
        {
            _db.Citations.Remove(image.Citation);
        }

        _db.Images.Remove(image);

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region UPDATE GALLERY IMAGE

    public async Task UpdateGalleryImageAsync(
        int imageId,
        UpdateGalleryImageFormRequest request,
        CancellationToken ct
    )
    {
        var image = await _db.Images
            .Include(i => i.Citation)
            .FirstOrDefaultAsync(i => i.ImgId == imageId, ct);

        if (image is null)
            throw new NotFoundException("Gallery image not found.");

        image.ImageUrl = request.ImageUrl;
        image.Title = request.Title;
        image.Desc = request.Desc;

        if (request.Citation is null)
        {
            if (image.Citation is not null)
            {
                _db.Citations.Remove(image.Citation);
                image.Citation = null;
                image.CiteId = null;
            }
        }
        else if (image.Citation is null)
        {
            image.Citation = new Citation
            {
                Title = request.Citation.Title,
                Author = request.Citation.Author,
                Url = request.Citation.Url,
                Year = request.Citation.Year
            };
        }
        else
        {
            image.Citation.Title = request.Citation.Title;
            image.Citation.Author = request.Citation.Author;
            image.Citation.Url = request.Citation.Url;
            image.Citation.Year = request.Citation.Year;
        }

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region IMPORT SHRINES

    public async Task ImportShrinesAsync(ImportShrinesRequest request, CancellationToken ct)
    {
        if (request.Previews is null || request.Previews.Count == 0)
            return;

        var shrines = request.Previews.Select(preview => new Shrine
        {
            InputtedId = preview.ImportId.Trim(),
            NameEn = "Unnamed Shrine",
            NameJp = string.IsNullOrWhiteSpace(preview.Name) ? "" : preview.Name.Trim(),
            Lat = (decimal?)preview.Lat,
            Lon = (decimal?)preview.Lon,
            Status = "import"
        }).ToList();

        await _db.Shrines.AddRangeAsync(shrines, ct);
        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region CREATE SHRINE

    public async Task CreateShrineAsync(CreateShrineRequest request, CancellationToken ct)
    {
        if (request.Lat is null || request.Lon is null)
            throw new ValidationException("Shrine coordinates are required.");

        var shrine = new Shrine
        {
            NameEn = request.NameEn,
            NameJp = request.NameJp,
            Lat = (decimal)request.Lat.Value,
            Lon = (decimal)request.Lon.Value,
            Status = "draft"
        };

        _db.Add(shrine);
        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region DELETE SHRINE

    public async Task DeleteShrineAsync(int shrineId, CancellationToken ct)
    {
        var shrine = await _db.Shrines
            .Include(s => s.ShrineHistories)
                .ThenInclude(h => h.HistoryCitations)
            .Include(s => s.ShrineFolklores)
                .ThenInclude(f => f.FolkloreCitations)
            .FirstOrDefaultAsync(s => s.ShrineId == shrineId, ct);

        if (shrine is null) throw new NotFoundException($"Shrine {shrineId} was not found.");

        // Collect citation ids tied to shrine
        var citationIds = new HashSet<int>();

        foreach (var h in shrine.ShrineHistories)
            foreach (var hc in h.HistoryCitations)
                citationIds.Add(hc.CiteId);

        foreach (var f in shrine.ShrineFolklores)
            foreach (var fc in f.FolkloreCitations)
                citationIds.Add(fc.CiteId);

        _db.Shrines.Remove(shrine);
        await _db.SaveChangesAsync(ct);

        if (citationIds.Count == 0) return;

        // Find citations that are no longer referenced anywhere
        var orphanCitationIds = new List<int>();

        foreach (var citeId in citationIds)
        {
            bool stillUsed =
                await _db.HistoryCitations.AnyAsync(x => x.CiteId == citeId, ct) ||
                await _db.FolkloreCitations.AnyAsync(x => x.CiteId == citeId, ct) ||
                await _db.Images.AnyAsync(x => x.CiteId == citeId, ct);

            if (!stillUsed)
                orphanCitationIds.Add(citeId);
        }

        if (orphanCitationIds.Count > 0)
        {
            var citations = await _db.Citations
                .Where(c => orphanCitationIds.Contains(c.CiteId))
                .ToListAsync(ct);

            _db.Citations.RemoveRange(citations);
            await _db.SaveChangesAsync(ct);
        }
    }

    #endregion

    #region UPDATE SHRINE STATUS

    public async Task UpdateShrineStatus(int shrineId, string status, CancellationToken ct)
    {
        var shrine = await _db.Shrines
            .FirstOrDefaultAsync(s => s.ShrineId == shrineId, ct);

        if (shrine is null) throw new NotFoundException($"Shrine {shrineId} was not found.");

        if (status != "import" && status != "draft" && status != "review" && status != "published")
            throw new BadRequestException("Could not update shrine status, status presented is invalid.");

        shrine.Status = status;

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region SUBMIT REVIEW SHRINE

    public async Task SubmitShrineForReview(int shrineId, int userId, CancellationToken ct)
    {
        // Get shrine and validate it exists
        var shrine = await _db.Shrines.FirstOrDefaultAsync(s => s.ShrineId == shrineId, ct);
        if (shrine is null) 
            throw new NotFoundException($"Shrine {shrineId} was not found.");

        // Check if shrine is already pending review
        var hasPendingReview = await _db.ShrineReviews
            .AnyAsync(r => r.ShrineId == shrineId && r.Decision == ReviewDecision.Pending, ct);

        if (hasPendingReview)
            throw new BadRequestException("Shrine already has a pending review.");

        // New ShrineReview
        var review = new ShrineReview
        {
            ShrineId = shrineId,
            SubmittedAt = DateTime.UtcNow,
            SubmittedBy = userId,
            Decision = ReviewDecision.Pending
        };

        _db.ShrineReviews.Add(review);

        shrine.Status = "review";

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region REJECT REVIEW SHRINE

    public async Task RejectShrineForReview(int shrineId, int userId, string message, CancellationToken ct)
    {  
        if (string.IsNullOrWhiteSpace(message))
            throw new BadRequestException("Rejection message is required.");

        // Get shrine and validate it exists
        var shrine = await _db.Shrines.FirstOrDefaultAsync(s => s.ShrineId == shrineId, ct);
        if (shrine is null) 
            throw new NotFoundException($"Shrine {shrineId} was not found.");
        if (shrine.Status != "review")
            throw new BadRequestException("Only shrines in review can be published.");

        // Get review and validate it exists
        var review = await _db.ShrineReviews
            .FirstOrDefaultAsync(r => r.Decision == ReviewDecision.Pending && r.ShrineId == shrineId, ct);
        if (review is null) 
            throw new NotFoundException($"Shrine {shrineId} does not have a pending review.");

        // Update review
        review.ReviewedAt = DateTime.UtcNow;
        review.ReviewedBy = userId;
        review.ReviewerComment = message;
        review.Decision = ReviewDecision.Rejected;

        // Update shrine status
        if (shrine.InputtedId is null) shrine.Status = "draft";
        else shrine.Status = "import";
        
        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region PUBLISH REVIEW SHRINE

    public async Task PublishShrineForReview(int shrineId, int userId, CancellationToken ct)
    {  
        // Get shrine and validate it exists
        var shrine = await _db.Shrines.FirstOrDefaultAsync(s => s.ShrineId == shrineId, ct);
        if (shrine is null) 
            throw new NotFoundException($"Shrine {shrineId} was not found.");
        if (shrine.Status != "review")
            throw new BadRequestException("Only shrines in review can be published.");

        // Get review and validate it exists
        var review = await _db.ShrineReviews
            .FirstOrDefaultAsync(r => r.Decision == ReviewDecision.Pending && r.ShrineId == shrineId, ct);
        if (review is null) 
            throw new NotFoundException($"Shrine {shrineId} does not have a pending review.");

        // Update review
        review.ReviewedAt = DateTime.UtcNow;
        review.ReviewedBy = userId;
        review.Decision = ReviewDecision.Published;

        // Update shrine status
        shrine.Status = "published";
        shrine.PublishedAt = DateTime.UtcNow;
        
        await _db.SaveChangesAsync(ct);
    }

    #endregion
}