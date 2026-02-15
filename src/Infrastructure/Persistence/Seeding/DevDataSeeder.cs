using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Persistence.Seeding;

public static class DevDataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        // Ensure DB + migrations are applied (fine for dev)
        await db.Database.MigrateAsync();

        // Seed only if empty (choose ONE table as your "already seeded" check)
        if (await db.EtiquetteTopics.AnyAsync())
            return;

        // -----------------------
        // 1) Citations (FK target)
        // -----------------------
        db.Citations.AddRange(
            new Citation
            {
                CiteId = 4,
                Title = "Jinja Honchō (Association of Shinto Shrines) — Shinto Basics / Worship at Shrines",
                Author = "Jinja Honchō",
                Url = "https://www.jinjahoncho.or.jp/en/",
                Year = 2020,
                Notes = "General overview of shrine worship etiquette (varies by shrine)."
            },
            new Citation
            {
                CiteId = 5,
                Title = "Meiji Jingu — How to Pray / Shrine Manners",
                Author = "Meiji Jingu",
                Url = "https://www.meijijingu.or.jp/en/",
                Year = 2021,
                Notes = "Practical visitor-facing etiquette for approach, purification, and prayer."
            },
            new Citation
            {
                CiteId = 6,
                Title = "Kanda Myojin — Visiting / How to Pray",
                Author = "Kanda Myojin Shrine",
                Url = "https://www.kandamyoujin.or.jp/",
                Year = 2022,
                Notes = "Visitor guidance; includes temizuya usage and prayer steps (may be Japanese-only sections)."
            },
            new Citation
            {
                CiteId = 7,
                Title = "Bunkachō (Agency for Cultural Affairs) — Shrines and Cultural Properties (overview)",
                Author = "Agency for Cultural Affairs, Japan",
                Url = "https://www.bunka.go.jp/english/",
                Year = 2019,
                Notes = "Background context on shrines as cultural sites; not a step-by-step etiquette guide."
            },
            new Citation
            {
                CiteId = 8,
                Title = "Kyoto City Official Travel Guide — Shrine & Temple Manners (visitor summary)",
                Author = "Kyoto City / Kyoto Travel",
                Url = "https://kyoto.travel/en/",
                Year = 2023,
                Notes = "Tourism-oriented etiquette summary; good for common-sense do/don’t guidance."
            }
        );

        await db.SaveChangesAsync();

        // -----------------------
        // 2) Images
        // -----------------------
        db.Images.AddRange(
            new Image
            {
                ImgId = 102,
                ImgSource = "https://www.howto-osaka.com/en/wp/wp-content/uploads/2023/03/koyasan-4.jpg",
                Title = "Shrine 2 Hero (Test)",
                Desc = "Placeholder hero image for Shrine 2.",
                CiteId = null, // your fixture had cite_id=1, but we are not seeding cite_id=1 in etiquette-only mode
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-01-27T18:39:40Z"), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-01-27T18:39:40Z"), DateTimeKind.Utc)
            },
            new Image
            {
                ImgId = 201,
                ImgSource = "https://japanhousela.com/wp-content/uploads/2025/02/Sengen-sama.jpg",
                Title = "Inari Okami test",
                Desc = "Fixture image used for step illustrations.",
                CiteId = null, // fixture had cite_id=2; omitted here since etiquette module seeds cite_id 4–8
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-01-27T18:39:50Z"), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-01-27T18:39:50Z"), DateTimeKind.Utc)
            },
            new Image
            {
                ImgId = 202,
                ImgSource = "https://images.squarespace-cdn.com/content/v1/6683b1f0c2de43611580eee6/1728974165706-O2AL95S7KY0WP49D8TWE/Japan-shrine-qTPCqxCR-LA.jpg",
                Title = "random history test img",
                Desc = "Fixture image used for step illustrations.",
                CiteId = null,
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-01-27T18:39:50Z"), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-01-27T18:39:50Z"), DateTimeKind.Utc)
            }
        );

        await db.SaveChangesAsync();

        // -----------------------
        // 3) Etiquette Topics
        // -----------------------
        db.EtiquetteTopics.AddRange(
            new EtiquetteTopic
            {
                TopicId = 1,
                Slug = "approaching-torii",
                TitleLong = "Approaching the Torii",
                TitleShort = "Bow Torii",
                Summary = "How to enter respectfully: pause, bow lightly, and pass through calmly.",
                IconKey = "torii-gate",
                IconSet = "fa5",
                ShowInGlance = true,
                ShowAsHighlight = false,
                GlanceOrder = 1,
                GuideOrder = 1,
                Status = "published",
                PublishedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc),
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-01T12:45:00Z"), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc)
            },
            new EtiquetteTopic
            {
                TopicId = 2,
                Slug = "walk-the-side",
                TitleLong = "Walk Along the Side (Sandō)",
                TitleShort = "Walk Side",
                Summary = "On the approach path, avoid the center line when possible and keep a relaxed, quiet pace.",
                IconKey = "walking",
                IconSet = "fa5",
                ShowInGlance = true,
                ShowAsHighlight = false,
                GlanceOrder = 4,
                GuideOrder = 2,
                Status = "published",
                PublishedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc),
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-01T12:46:00Z"), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc)
            },
            new EtiquetteTopic
            {
                TopicId = 3,
                Slug = "purification-temizuya",
                TitleLong = "Purification (Temizuya)",
                TitleShort = "Purification",
                Summary = "Perform a simple hand-and-mouth rinse ritual before approaching the main hall (if the temizuya is available).",
                IconKey = "hands-wash",
                IconSet = "fa5",
                ShowInGlance = true,
                ShowAsHighlight = true,
                GlanceOrder = 2,
                GuideOrder = 3,
                Status = "published",
                PublishedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc),
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-01T12:47:00Z"), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc)
            },
            new EtiquetteTopic
            {
                TopicId = 4,
                Slug = "prayer-2-2-1",
                TitleLong = "Pray at the Haiden (2–2–1)",
                TitleShort = "Pray",
                Summary = "A common prayer style is: offering → bell (if present) → two bows → two claps → one bow.",
                IconKey = "praying-hands",
                IconSet = "fa5",
                ShowInGlance = true,
                ShowAsHighlight = false,
                GlanceOrder = 3,
                GuideOrder = 4,
                Status = "published",
                PublishedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc),
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-01T12:48:00Z"), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc)
            },
            new EtiquetteTopic
            {
                TopicId = 5,
                Slug = "mindfulness-behavior",
                TitleLong = "Mindfulness & Behavior",
                TitleShort = "Mindful",
                Summary = "Keep your voice low, don’t block pathways, and treat the space like a place of worship—not a theme park.",
                IconKey = "brain",
                IconSet = "fa5",
                ShowInGlance = true,
                ShowAsHighlight = false,
                GlanceOrder = 5,
                GuideOrder = 5,
                Status = "published",
                PublishedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc),
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-01T12:49:00Z"), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc)
            },
            new EtiquetteTopic
            {
                TopicId = 6,
                Slug = "photos-and-restricted-areas",
                TitleLong = "Photos & Restricted Areas",
                TitleShort = "Photos",
                Summary = "Photography rules vary. Follow signs, avoid photographing inside buildings unless allowed, and don’t disrupt rituals.",
                IconKey = "camera",
                IconSet = "fa5",
                ShowInGlance = true,
                ShowAsHighlight = false,
                GlanceOrder = 6,
                GuideOrder = 6,
                Status = "published",
                PublishedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc),
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-01T12:50:00Z"), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc)
            },
            new EtiquetteTopic
            {
                TopicId = 7,
                Slug = "ema-omamori-omikuji",
                TitleLong = "Ema, Omamori, and Omikuji",
                TitleShort = "Shrine Luck",
                Summary = "How to participate respectfully in common shrine activities: charms, fortunes, and wish plaques.",
                IconKey = "scroll",
                IconSet = "fa5",
                ShowInGlance = false,
                ShowAsHighlight = false,
                GlanceOrder = null,
                GuideOrder = 7,
                Status = "published",
                PublishedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc),
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-01T12:51:00Z"), DateTimeKind.Utc),
                UpdatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc)
            }
        );

        await db.SaveChangesAsync();

        // -----------------------
        // 4) Etiquette Steps
        // -----------------------
        db.EtiquetteSteps.AddRange(
            new EtiquetteStep { StepId = 3001, TopicId = 3, StepOrder = 1, Text = "At the temizuya, take the ladle with your right hand and pour water over your left hand.", ImageId = 201 },
            new EtiquetteStep { StepId = 3002, TopicId = 3, StepOrder = 2, Text = "Switch: hold the ladle in your left hand and rinse your right hand.", ImageId = 201 },
            new EtiquetteStep { StepId = 3003, TopicId = 3, StepOrder = 3, Text = "Switch back to your right hand. Pour a little water into your left palm and rinse your mouth lightly (do not drink from the ladle).", ImageId = 201 },
            new EtiquetteStep { StepId = 3004, TopicId = 3, StepOrder = 4, Text = "Rinse your left hand again. Then tilt the ladle vertically so remaining water runs down the handle (a simple ‘clean finish’).", ImageId = 201 },
            new EtiquetteStep { StepId = 3005, TopicId = 3, StepOrder = 5, Text = "Return the ladle neatly. If the temizuya is closed or dry, simply proceed respectfully—many shrines modify practices at times.", ImageId = 201 },

            new EtiquetteStep { StepId = 4001, TopicId = 4, StepOrder = 1, Text = "Approach the haiden calmly. Stand behind the offering box line and wait your turn if there’s a queue.", ImageId = 202 },
            new EtiquetteStep { StepId = 4002, TopicId = 4, StepOrder = 2, Text = "Place a small offering into the saisen-bako (amount is personal—there’s no fixed rule).", ImageId = 202 },
            new EtiquetteStep { StepId = 4003, TopicId = 4, StepOrder = 3, Text = "If there is a bell, ring it gently once to signal your presence (don’t repeatedly ring it like an alarm).", ImageId = 202 },
            new EtiquetteStep { StepId = 4004, TopicId = 4, StepOrder = 4, Text = "Perform the common prayer form: bow twice, clap twice, then bow once.", ImageId = 202 },
            new EtiquetteStep { StepId = 4005, TopicId = 4, StepOrder = 5, Text = "During the quiet moment after clapping, you can offer a short prayer: gratitude first, then your request (keep it simple).", ImageId = 202 },
            new EtiquetteStep { StepId = 4006, TopicId = 4, StepOrder = 6, Text = "Step away to the side so the next person can approach. Don’t stand directly in front of the hall to review photos/messages.", ImageId = 202 },

            new EtiquetteStep { StepId = 5001, TopicId = 5, StepOrder = 1, Text = "Use an ‘inside voice’ throughout the grounds. Shrines are living religious spaces, not just tourist photo spots.", ImageId = null },
            new EtiquetteStep { StepId = 5002, TopicId = 5, StepOrder = 2, Text = "Don’t eat or smoke while walking around the main worship areas. If there are designated rest areas, use those.", ImageId = null },
            new EtiquetteStep { StepId = 5003, TopicId = 5, StepOrder = 3, Text = "Avoid touching sacred ropes, offerings, or ritual objects unless you’re clearly invited (some items are for priests only).", ImageId = null },
            new EtiquetteStep { StepId = 5004, TopicId = 5, StepOrder = 4, Text = "If a ceremony is happening, watch from a respectful distance and do not walk through the middle of the ritual space.", ImageId = null },

            new EtiquetteStep { StepId = 7001, TopicId = 7, StepOrder = 1, Text = "Omamori (charms) are typically carried with you (bag, wallet) rather than opened. Treat them as sacred items.", ImageId = null },
            new EtiquetteStep { StepId = 7002, TopicId = 7, StepOrder = 2, Text = "Ema (wish plaques): write a short wish or gratitude, then hang it in the designated ema area (not on random trees).", ImageId = 102 },
            new EtiquetteStep { StepId = 7003, TopicId = 7, StepOrder = 3, Text = "Omikuji (fortunes): read it, then either keep it or tie it at the shrine where indicated (ask or look for the tying area).", ImageId = 102 },
            new EtiquetteStep { StepId = 7004, TopicId = 7, StepOrder = 4, Text = "End-of-year/return etiquette: many people return old omamori to the shrine for appropriate disposal (often at a collection box).", ImageId = null }
        );

        await db.SaveChangesAsync();

        // -----------------------
        // 5) Topic <-> Citation links
        // -----------------------
        db.EtiquetteCitations.AddRange(
            new EtiquetteTopicCitation { TopicId = 1, CiteId = 5, CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc) },
            new EtiquetteTopicCitation { TopicId = 2, CiteId = 4, CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc) },
            new EtiquetteTopicCitation { TopicId = 3, CiteId = 6, CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc) },
            new EtiquetteTopicCitation { TopicId = 3, CiteId = 5, CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc) },
            new EtiquetteTopicCitation { TopicId = 4, CiteId = 4, CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc) },
            new EtiquetteTopicCitation { TopicId = 4, CiteId = 5, CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc) },
            new EtiquetteTopicCitation { TopicId = 5, CiteId = 7, CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc) },
            new EtiquetteTopicCitation { TopicId = 6, CiteId = 6, CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc) },
            new EtiquetteTopicCitation { TopicId = 7, CiteId = 8, CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2026-02-03T09:00:00Z"), DateTimeKind.Utc) }
        );

        await db.SaveChangesAsync();

        await db.Database.ExecuteSqlRawAsync("""
            SELECT setval(pg_get_serial_sequence('citations','cite_id'), (SELECT COALESCE(MAX(cite_id), 1) FROM citations));
            SELECT setval(pg_get_serial_sequence('images','img_id'), (SELECT COALESCE(MAX(img_id), 1) FROM images));
            SELECT setval(pg_get_serial_sequence('etiquette_topics','topic_id'), (SELECT COALESCE(MAX(topic_id), 1) FROM etiquette_topics));
            SELECT setval(pg_get_serial_sequence('etiquette_steps','step_id'), (SELECT COALESCE(MAX(step_id), 1) FROM etiquette_steps));
        """);
    }
}
