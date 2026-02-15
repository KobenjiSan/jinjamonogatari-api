using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EtiquetteController : ControllerBase
{
    private readonly AppDbContext _db;

    public EtiquetteController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/etiquette
    [HttpGet]
    public async Task<ActionResult> GetTopics()
    {
        var topics = await _db.EtiquetteTopics
            .AsNoTracking()
            .Include(t => t.Steps)
            .Include(t => t.TopicCitations)
                .ThenInclude(tc => tc.Citation)
            .OrderBy(t => t.GuideOrder)
            .Select(t => new
            {
                t.TopicId,
                t.Slug,
                t.TitleLong,
                t.TitleShort,
                t.Summary,
                t.IconKey,
                t.IconSet,
                t.ImageId,
                t.ShowInGlance,
                t.ShowAsHighlight,
                t.GlanceOrder,
                t.GuideOrder,
                t.Status,
                t.PublishedAt,
                t.CreatedAt,
                t.UpdatedAt,
                Steps = t.Steps
                    .OrderBy(s => s.StepOrder)
                    .Select(s => new
                    {
                        s.StepId,
                        s.StepOrder,
                        s.Text,
                        s.ImageId
                    }),
                Citations = t.TopicCitations
                    .Select(tc => new
                    {
                        tc.CiteId,
                        tc.Citation.Title,
                        tc.Citation.Author,
                        tc.Citation.Url,
                        tc.Citation.Year,
                        tc.Citation.Notes
                    })
            })
            .ToListAsync();

        return Ok(topics);
    }

    // GET: api/etiquette/3
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetTopicDetail(int id)
    {
        var topic = await _db.EtiquetteTopics
            .AsNoTracking()
            .Include(t => t.Image)
            .Include(t => t.Steps.OrderBy(s => s.StepOrder))
                .ThenInclude(s => s.Image)
            .Include(t => t.TopicCitations)
                .ThenInclude(tc => tc.Citation)
            .FirstOrDefaultAsync(t => t.TopicId == id);

        if (topic == null)
            return NotFound();

        return Ok(topic);
    }
}
