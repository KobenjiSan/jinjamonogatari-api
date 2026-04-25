using Api.Controllers.Etiquette;
using Application.Common.Exceptions;
using Application.Features.Etiquette.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Etiquette;

public class EtiquetteWriteService : IEtiquetteWriteService
{
    private readonly AppDbContext _db;

    public EtiquetteWriteService(AppDbContext db)
    {
        _db = db;
    }

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

    #region Create Etiquette

    public async Task CreateEtiquetteAsync(CreateEtiquetteRequest request, CancellationToken ct)
    {
        var r = request.Basic;

        // Validate unique slug
        var slugExists = await _db.EtiquetteTopics
            .AnyAsync(t => t.Slug == r.Slug, ct);
        if (slugExists)
            throw new BadRequestException("Topic slug already exists.");
    
        if (string.IsNullOrWhiteSpace(r.Slug))
            throw new BadRequestException("Topic Slug is a required field");

        // Create new topic
        var topic = new EtiquetteTopic
        {
            Slug = r.Slug,
            TitleLong = r.TitleLong,
            Summary = r.Summary,
            ShowInGlance = r.ShowInGlance,
            ShowAsHighlight = r.ShowAsHighlight,
            GuideOrder = r.GuideOrder,
            Status = "published"
        };

        // Create new citations
        if (request.Citations.Create.Count > 0)
        {
            foreach (var citationRequest in request.Citations.Create)
            {
                var citation = BuildCitation(citationRequest);

                topic.TopicCitations.Add(new EtiquetteTopicCitation
                {
                    Topic = topic,
                    Citation = citation
                });
            }
        }

        // Save Topic
        _db.EtiquetteTopics.Add(topic);

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region Create Step

    public async Task CreateStepAsync(int topicId, CreateStepRequest request, string? publicId, CancellationToken ct)
    {
        // Load topics
        var topic = await _db.EtiquetteTopics
            .FirstOrDefaultAsync(t => t.TopicId == topicId, ct);

        // Validate topic exists
        if (topic is null)
            throw new NotFoundException("Topic not found.");

        // Create new step
        var r = request.Basic;
        var step = new EtiquetteStep
        {
            Text = r.Text,
            StepOrder = r.StepOrder,
            Topic = topic,
        };

        // Create image if provided
        if (request.Image is not null)
        {
            var image = new Image
            {
                PublicId = publicId,
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

            step.Image = image;
        }

        _db.EtiquetteSteps.Add(step);
        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region Delete Etiquette

    public async Task DeleteEtiquetteAsync(int topicId, CancellationToken ct)
    {
        var topic = await _db.EtiquetteTopics
            .Include(t => t.TopicCitations)
                .ThenInclude(c => c.Citation)
            .Include(t => t.Steps)
                .ThenInclude(s => s.Image!)
                    .ThenInclude(i => i.Citation)
            .FirstOrDefaultAsync(t => t.TopicId == topicId, ct);

        if (topic is null) throw new NotFoundException("Topic not found.");

        // Remove topic citation
        if (topic.TopicCitations.Count > 0)
        {
            foreach (var topicCitation in topic.TopicCitations.ToList())
            {
                if (topicCitation.Citation is not null)
                {
                    _db.Citations.Remove(topicCitation.Citation);
                }

                _db.Set<EtiquetteTopicCitation>().Remove(topicCitation);
            }
        }

        // Remove image data from steps
        foreach (var step in topic.Steps.ToList())
        {
            // Remove image citations if present
            if (step.Image?.Citation is not null)
            {
                _db.Citations.Remove(step.Image.Citation);
            }

            // Remove image if present
            if (step.Image is not null)
            {
                _db.Images.Remove(step.Image);
            }
        }

        // Remove topic
        _db.EtiquetteTopics.Remove(topic);
        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region Delete Glance

    public async Task DeleteGlanceAsync(int topicId, CancellationToken ct)
    {
        var topic = await _db.EtiquetteTopics
            .FirstOrDefaultAsync(t => t.TopicId == topicId, ct);

        if (topic is null) throw new NotFoundException("Topic not found.");

        topic.ShowInGlance = false;

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region Delete Step

    public async Task DeleteStepAsync(int stepId, CancellationToken ct)
    {
        var step = await _db.EtiquetteSteps
            .Include(s => s.Image!)
                .ThenInclude(i => i.Citation)
            .FirstOrDefaultAsync(t => t.StepId == stepId, ct);

        if (step is null) throw new NotFoundException("Step not found.");

        // Remove image citations if present
        if (step.Image?.Citation is not null)
        {
            _db.Citations.Remove(step.Image.Citation);
        }

        // Remove image if present
        if (step.Image is not null)
        {
            _db.Images.Remove(step.Image);
        }

        // Remove step
        _db.EtiquetteSteps.Remove(step);
        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region Update Etiquette

    public async Task UpdateEtiquetteAsync(int topicId, UpdateEtiquetteRequest request, CancellationToken ct)
    {
        var topic = await _db.EtiquetteTopics
            .Include(t => t.TopicCitations)
                .ThenInclude(c => c.Citation)
            .FirstOrDefaultAsync(t => t.TopicId == topicId, ct);

        if (topic is null) throw new NotFoundException("Topic not found.");

        var r = request.Basic;
        if (string.IsNullOrWhiteSpace(r.Slug))
            throw new BadRequestException("Topic Slug is a required field");

        topic.Slug = r.Slug;
        topic.TitleLong = r.TitleLong;
        topic.Summary = r.Summary;
        topic.ShowInGlance = r.ShowInGlance;
        topic.ShowAsHighlight = r.ShowAsHighlight;
        topic.GuideOrder = r.GuideOrder;

        // Delete citations
        if (request.Citations.Delete.Count > 0)
        {
            var citationsToRemove = topic.TopicCitations
                .Where(fc => request.Citations.Delete.Contains(fc.CiteId))
                .ToList();

            foreach (var topicCitation in citationsToRemove)
            {
                if (topicCitation.Citation is not null)
                {
                    _db.Citations.Remove(topicCitation.Citation);
                }

                topic.TopicCitations.Remove(topicCitation);
                _db.Set<EtiquetteTopicCitation>().Remove(topicCitation);
            }
        }

        // Create citations
        if (request.Citations.Create.Count > 0)
        {
            foreach (var citationRequest in request.Citations.Create)
            {
                var citation = BuildCitation(citationRequest);

                topic.TopicCitations.Add(new EtiquetteTopicCitation
                {
                    TopicId = topic.TopicId,
                    Topic = topic,
                    Citation = citation
                });
            }
        }

        // Update citations
        if (request.Citations.Update.Count > 0)
        {
            foreach (var citationRequest in request.Citations.Update)
            {
                var existingCitation = topic.TopicCitations
                    .FirstOrDefault(hc => hc.CiteId == citationRequest.CiteId);

                if (existingCitation is null)
                    continue;

                existingCitation.Citation.Title = citationRequest.Title;
                existingCitation.Citation.Author = citationRequest.Author;
                existingCitation.Citation.Url = citationRequest.Url;
                existingCitation.Citation.Year = citationRequest.Year;
            }
        }

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region Update Glance

    public async Task UpdateGlanceAsync(int topicId, UpdateGlanceRequest request, CancellationToken ct)
    {
        var topic = await _db.EtiquetteTopics
            .FirstOrDefaultAsync(t => t.TopicId == topicId, ct);

        if (topic is null) throw new NotFoundException("Topic not found.");

        topic.TitleShort = request.TitleShort;
        topic.IconKey = request.IconKey;
        topic.IconSet = request.IconSet;
        topic.GlanceOrder = request.GlanceOrder;
        topic.ShowInGlance = true;

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region Update Step

    public async Task UpdateStepAsync(int stepId, UpdateStepRequest request, string? publicId, CancellationToken ct)
    {
        var step = await _db.EtiquetteSteps
            .Include(s => s.Image!)
                .ThenInclude(i => i.Citation)
            .FirstOrDefaultAsync(t => t.StepId == stepId, ct);

        if (step is null) throw new NotFoundException("Step not found.");

        // Delete image
        if (request.Image.Action == "delete" && step.Image is not null)
        {
            if (step.Image.Citation is not null)
                _db.Citations.Remove(step.Image.Citation);

            _db.Images.Remove(step.Image);
            step.Image = null;
            step.ImageId = null;
        }

        // Create image
        if (request.Image.Action == "create")
        {
            if (step.Image is not null)
                throw new BadRequestException("Step already has an image.");

            var image = new Image
            {
                PublicId = publicId,
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

            step.Image = image;
        }

        // Update image
        if (request.Image.Action == "update")
        {
            if (step.Image is null)
                throw new NotFoundException("Step image not found.");

            if (!string.IsNullOrWhiteSpace(publicId))
            {
                step.Image.PublicId = publicId;
            }
            step.Image.ImageUrl = request.Image.ImageUrl;
            step.Image.Title = request.Image.Title;
            step.Image.Desc = request.Image.Desc;

            if (request.Image.Citation is null)
            {
                if (step.Image.Citation is not null)
                    _db.Citations.Remove(step.Image.Citation);

                step.Image.Citation = null;
                step.Image.CiteId = null;
            }
            else if (step.Image.Citation is null)
            {
                step.Image.Citation = new Citation
                {
                    Title = request.Image.Citation.Title,
                    Author = request.Image.Citation.Author,
                    Url = request.Image.Citation.Url,
                    Year = request.Image.Citation.Year
                };
            }
            else
            {
                step.Image.Citation.Title = request.Image.Citation.Title;
                step.Image.Citation.Author = request.Image.Citation.Author;
                step.Image.Citation.Url = request.Image.Citation.Url;
                step.Image.Citation.Year = request.Image.Citation.Year;
            }
        }

        await _db.SaveChangesAsync(ct);
    }

    #endregion
}
