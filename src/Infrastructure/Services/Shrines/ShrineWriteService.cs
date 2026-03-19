using Application.Common.Exceptions;
using Application.Features.Shrines.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Shrines;

public class ShrineWriteService : IShrineWriteService
{
    private readonly AppDbContext _db;

    public ShrineWriteService(AppDbContext db)
    {
        _db = db;
    }

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

        // Delete Tags (remove from shrine, not globaly)
        if (request.Tags.Delete.Count > 0)
        {
            var shrineTagsToRemove = shrine.ShrineTags
                .Where(st => request.Tags.Delete.Contains(st.TagId))
                .ToList();

            foreach (var shrineTag in shrineTagsToRemove)
            {
                shrine.ShrineTags.Remove(shrineTag);
            }
        }

        // Create Tag
        if (request.Tags.Create.Count > 0)
        {
            foreach (var tagRequest in request.Tags.Create)
            {
                var newTag = new Tag
                {
                    TitleEn = tagRequest.TitleEn,
                    TitleJp = tagRequest.TitleJp
                };

                shrine.ShrineTags.Add(new ShrineTag
                {
                    ShrineId = shrine.ShrineId,
                    Shrine = shrine,
                    Tag = newTag
                });
            }
        }

        // Update Tag
        if (request.Tags.Update.Count > 0)
        {
            foreach (var tagRequest in request.Tags.Update)
            {
                var existingShrineTag = shrine.ShrineTags
                    .FirstOrDefault(st => st.TagId == tagRequest.TagId);

                if (existingShrineTag is null)
                    continue;

                existingShrineTag.Tag.TitleEn = tagRequest.TitleEn;
                existingShrineTag.Tag.TitleJp = tagRequest.TitleJp;
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
                ImgSource = request.HeroImage.ImgSource,
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

            shrine.Image.ImgSource = request.HeroImage.ImgSource;
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

        await _db.SaveChangesAsync(ct);
    }

    // =======================================================================
    // KAMI
    // =======================================================================

    // CREATE KAMI
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
                ImgSource = request.Image.ImgSource,
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

        // Create citations if provided
        if (request.Citations is not null && request.Citations.Count > 0)
        {
            foreach (var citationRequest in request.Citations)
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
                    Kami = kami,
                    Citation = citation
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

    // LINK KAMI
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

    // UNLINK KAMI
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

    // UPDATE KAMI
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
                ImgSource = request.Image.ImgSource,
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

            kami.Image.ImgSource = request.Image.ImgSource;
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

        // Delete citations
        if (request.Citations.Delete.Count > 0)
        {
            var kamiCitationsToRemove = kami.KamiCitations
                .Where(kc => request.Citations.Delete.Contains(kc.CiteId))
                .ToList();

            foreach (var kamiCitation in kamiCitationsToRemove)
            {
                kami.KamiCitations.Remove(kamiCitation);
                _db.Citations.Remove(kamiCitation.Citation);
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

    // =======================================================================
    // HISTORY
    // =======================================================================

    // CREATE HISTORY
    public async Task CreateHistoryAsync(int shrineId, CreateHistoryRequest request, CancellationToken ct)
    {
        // Load shrine
        var shrine = await _db.Shrines
            .Include(s => s.ShrineHistories)
            .FirstOrDefaultAsync(s => s.ShrineId == shrineId, ct);

        // Validate shrine exists
        if(shrine is null)
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
        if(request.Image is not null)
        {
            var image = new Image
            {
                ImgSource = request.Image.ImgSource,
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

        // Create citations if provided
        if (request.Citations is not null && request.Citations.Count > 0)
        {
            foreach (var citationRequest in request.Citations)
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
                    History = history,
                    Citation = citation
                });
            }
        }

        // Link history to shrine
        shrine.ShrineHistories.Add(history);

        await _db.SaveChangesAsync(ct);
    }

    // DELETE HISTORY
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

        // Remove history citations + underlying citations
        if (history.HistoryCitations.Count > 0)
        {
            foreach (var historyCitation in history.HistoryCitations.ToList())
            {
                _db.Citations.Remove(historyCitation.Citation);
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

    // UPDATE HISTORY
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
                ImgSource = request.Image.ImgSource,
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

            history.Image.ImgSource = request.Image.ImgSource;
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

        // Delete citations
        if (request.Citations.Delete.Count > 0)
        {
            var historyCitationsToRemove = history.HistoryCitations
                .Where(hc => request.Citations.Delete.Contains(hc.CiteId))
                .ToList();

            foreach (var historyCitation in historyCitationsToRemove)
            {
                history.HistoryCitations.Remove(historyCitation);
                _db.Citations.Remove(historyCitation.Citation);
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

    // =======================================================================
    // FOLKLORE
    // =======================================================================

    // CREATE FOLKLORE
    public async Task CreateFolkloreAsync(int shrineId, CreateFolkloreRequest request, CancellationToken ct)
    {
        // Load shrine
        var shrine = await _db.Shrines
            .Include(s => s.ShrineFolklores)
            .FirstOrDefaultAsync(s => s.ShrineId == shrineId, ct);

        // Validate shrine exists
        if(shrine is null)
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
        if(request.Image is not null)
        {
            var image = new Image
            {
                ImgSource = request.Image.ImgSource,
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

        // Create citations if provided
        if (request.Citations is not null && request.Citations.Count > 0)
        {
            foreach (var citationRequest in request.Citations)
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
                    Folklore = folklore,
                    Citation = citation
                });
            }
        }

        // Link folklore to shrine
        shrine.ShrineFolklores.Add(folklore);

        await _db.SaveChangesAsync(ct);
    }

    // DELETE FOLKLORE
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

        // Remove Folklore citations + underlying citations
        if (folklore.FolkloreCitations.Count > 0)
        {
            foreach (var FolkloreCitation in folklore.FolkloreCitations.ToList())
            {
                _db.Citations.Remove(FolkloreCitation.Citation);
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

    // UPDATE FOLKLORE
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
                ImgSource = request.Image.ImgSource,
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

            folklore.Image.ImgSource = request.Image.ImgSource;
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

        // Delete citations
        if (request.Citations.Delete.Count > 0)
        {
            var folkloreCitationsToRemove = folklore.FolkloreCitations
                .Where(fc => request.Citations.Delete.Contains(fc.CiteId))
                .ToList();

            foreach (var folkloreCitation in folkloreCitationsToRemove)
            {
                folklore.FolkloreCitations.Remove(folkloreCitation);
                _db.Citations.Remove(folkloreCitation.Citation);
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
}