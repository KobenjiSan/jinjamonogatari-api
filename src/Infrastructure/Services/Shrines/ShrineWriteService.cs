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
}