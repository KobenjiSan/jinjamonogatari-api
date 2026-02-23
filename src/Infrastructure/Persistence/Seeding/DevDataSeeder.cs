using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Persistence.Seeding;

public static class DevDataSeeder
{
    private static DateTime Utc(string isoZ)
        => DateTime.SpecifyKind(DateTime.Parse(isoZ), DateTimeKind.Utc);

    public static async Task SeedAsync(AppDbContext db)
    {
        // Ensure DB + migrations are applied (fine for dev)
        await db.Database.MigrateAsync();

        // Seed only if empty (choose ONE table as your "already seeded" check)
        // Using Citations because basically everything can depend on it.
        if (await db.Shrines.AnyAsync())
            return;

        // -----------------------
        // 1) Citations (FK target)
        // -----------------------
        db.Citations.AddRange(
            new Citation
            {
                CiteId = 1,
                Title = "Test Reference: Kyoto Shrines Overview",
                Author = "Fixture Author",
                Url = "https://example.com/test/citation-1",
                Year = 2020,
                Notes = "Development-only citation entry."
            },
            new Citation
            {
                CiteId = 2,
                Title = "Test Reference: Image Attribution Pack",
                Author = "Fixture Author",
                Url = "https://example.com/test/citation-2",
                Year = 2021,
                Notes = "Development-only image attribution entry."
            },
            new Citation
            {
                CiteId = 3,
                Title = "Test Reference: random text",
                Author = "Fixture Author",
                Url = "https://example.com/test/citation-2",
                Year = 2022,
                Notes = "Development-only image attribution entry."
            },
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
        // 2) Images (FK target for many)
        // -----------------------
        db.Images.AddRange(
            new Image
            {
                ImgId = 101,
                ImgSource = "https://media.istockphoto.com/id/515824638/photo/fushimi-inari-shrine-of-kyoto.jpg?s=2048x2048&w=is&k=20&c=TwPT62bno8foiF-09zoETsfC4zj6yPRJwnDeMXpZogw=",
                Title = "Shrine 1 Hero (Test)",
                Desc = "Placeholder hero image for Shrine 1.",
                CiteId = 1,
                CreatedAt = Utc("2026-01-27T18:39:30Z"),
                UpdatedAt = Utc("2026-01-27T18:39:30Z")
            },
            new Image
            {
                ImgId = 102,
                ImgSource = "https://www.howto-osaka.com/en/wp/wp-content/uploads/2023/03/koyasan-4.jpg",
                Title = "Shrine 2 Hero (Test)",
                Desc = "Placeholder hero image for Shrine 2.",
                CiteId = 1,
                CreatedAt = Utc("2026-01-27T18:39:40Z"),
                UpdatedAt = Utc("2026-01-27T18:39:40Z")
            },
            new Image
            {
                ImgId = 201,
                ImgSource = "https://japanhousela.com/wp-content/uploads/2025/02/Sengen-sama.jpg",
                Title = "Inari Okami test",
                Desc = "Image of Inari Okami (fixture).",
                CiteId = 2,
                CreatedAt = Utc("2026-01-27T18:39:50Z"),
                UpdatedAt = Utc("2026-01-27T18:39:50Z")
            },
            new Image
            {
                ImgId = 202,
                ImgSource = "https://images.squarespace-cdn.com/content/v1/6683b1f0c2de43611580eee6/1728974165706-O2AL95S7KY0WP49D8TWE/Japan-shrine-qTPCqxCR-LA.jpg",
                Title = "random history test img",
                Desc = "Image of shrine",
                CiteId = 2,
                CreatedAt = Utc("2026-01-27T18:39:50Z"),
                UpdatedAt = Utc("2026-01-27T18:39:50Z")
            }
        );

        await db.SaveChangesAsync();

        // -----------------------
        // 3) Tags
        // -----------------------
        db.Tags.AddRange(
            new Tag { TagId = 1, TitleEn = "Inari", TitleJp = "稲荷", CreatedAt = Utc("2026-01-27T18:39:00Z"), UpdatedAt = Utc("2026-01-27T18:39:00Z") },
            new Tag { TagId = 2, TitleEn = "Tenjin", TitleJp = "天神", CreatedAt = Utc("2026-01-27T18:39:00Z"), UpdatedAt = Utc("2026-01-27T18:39:00Z") },
            new Tag { TagId = 3, TitleEn = "Downtown", TitleJp = "市街地", CreatedAt = Utc("2026-01-27T18:39:00Z"), UpdatedAt = Utc("2026-01-27T18:39:00Z") },
            new Tag { TagId = 4, TitleEn = "Historic", TitleJp = "歴史的", CreatedAt = Utc("2026-01-27T18:39:00Z"), UpdatedAt = Utc("2026-01-27T18:39:00Z") },
            new Tag { TagId = 5, TitleEn = "Popular", TitleJp = "人気", CreatedAt = Utc("2026-01-27T18:39:00Z"), UpdatedAt = Utc("2026-01-27T18:39:00Z") }
        );

        await db.SaveChangesAsync();

        // -----------------------
        // 4) Shrines
        // -----------------------
        db.Shrines.AddRange(
            new Shrine
            {
                ShrineId = 1,
                InputtedId = "test-import-kyoto-001",
                Lat = (decimal?)35.0069,
                Lon = (decimal?)135.7742,
                Slug = "kyo-test-inari-1",
                NameEn = "Kyo Test Inari Shrine",
                NameJp = "京テスト稲荷神社",
                ShrineDesc = "A small test shrine record near central Kyoto, used for development fixtures. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                AddressRaw = "Kyoto, Japan (test address)",
                Prefecture = "Kyoto",
                City = "Kyoto",
                Ward = "Shimogyo",
                Locality = "Downtown",
                PostalCode = "600-0000",
                Country = "Japan",
                PhoneNumber = null,
                Email = null,
                Website = null,
                ImgId = 101,
                Status = "published",
                CreatedAt = Utc("2026-01-27T18:40:00Z"),
                UpdatedAt = Utc("2026-01-27T18:40:00Z"),
                PublishedAt = Utc("2026-01-27T18:40:00Z")
            },
            new Shrine
            {
                ShrineId = 2,
                InputtedId = "test-import-kyoto-002",
                Lat = (decimal?)35.0132,
                Lon = (decimal?)135.7609,
                Slug = "kyo-test-tenmangu-2",
                NameEn = "Kyo Test Tenmangu",
                NameJp = "京テスト天満宮",
                ShrineDesc = "Second fixture shrine within walking distance of Kyoto center; includes tags, kami, and citations.",
                AddressRaw = "Kyoto, Japan (test address)",
                Prefecture = "Kyoto",
                City = "Kyoto",
                Ward = "Nakagyo",
                Locality = "Downtown",
                PostalCode = "604-0000",
                Country = "Japan",
                PhoneNumber = null,
                Email = null,
                Website = null,
                ImgId = 102,
                Status = "review",
                CreatedAt = Utc("2026-01-27T18:41:00Z"),
                UpdatedAt = Utc("2026-01-27T18:41:00Z"),
                PublishedAt = Utc("2026-01-27T18:40:00Z")
            },
            new Shrine
            {
                ShrineId = 3,
                InputtedId = "test-import-kyoto-003",
                Lat = (decimal?)35.0131,
                Lon = (decimal?)135.7675,
                Slug = "kyo-test-tenmangu-3",
                NameEn = "Kyo Test shrine things",
                NameJp = "京テスト天満宮",
                ShrineDesc = "3rd fixture shrine within walking distance of Kyoto center; includes tags, kami, and citations.",
                AddressRaw = "Kyoto, Japan (test address)",
                Prefecture = "Kyoto",
                City = "Kyoto",
                Ward = "Nakagyo",
                Locality = "Downtown",
                PostalCode = "604-0000",
                Country = "Japan",
                PhoneNumber = null,
                Email = null,
                Website = null,
                ImgId = 202,
                Status = "review",
                CreatedAt = Utc("2026-01-27T18:41:00Z"),
                UpdatedAt = Utc("2026-01-27T18:41:00Z"),
                PublishedAt = Utc("2026-01-27T18:40:00Z")
            }
        );

        await db.SaveChangesAsync();

        // -----------------------
        // 5) Shrine <-> Tag links
        // -----------------------
        db.ShrineTags.AddRange(
            new ShrineTag { ShrineId = 1, TagId = 1, CreatedAt = Utc("2026-01-27T18:40:10Z") },
            new ShrineTag { ShrineId = 1, TagId = 3, CreatedAt = Utc("2026-01-27T18:40:10Z") },
            new ShrineTag { ShrineId = 1, TagId = 4, CreatedAt = Utc("2026-01-27T18:40:10Z") },
            new ShrineTag { ShrineId = 1, TagId = 5, CreatedAt = Utc("2026-01-27T18:40:10Z") },

            new ShrineTag { ShrineId = 2, TagId = 2, CreatedAt = Utc("2026-01-27T18:41:10Z") },
            new ShrineTag { ShrineId = 2, TagId = 3, CreatedAt = Utc("2026-01-27T18:41:10Z") },

            new ShrineTag { ShrineId = 3, TagId = 2, CreatedAt = Utc("2026-01-27T18:41:10Z") }
        );

        await db.SaveChangesAsync();

        // -----------------------
        // 6) Kami
        // -----------------------
        db.Kamis.AddRange(
            new Kami
            {
                KamiId = 1,
                NameEn = "Inari Ōkami",
                NameJp = "稲荷大神",
                ImgId = 201,
                Desc = "A commonly referenced kami for Inari-type shrines (fixture text). Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                Status = "published",
                CreatedAt = Utc("2026-01-27T18:38:00Z"),
                UpdatedAt = Utc("2026-01-27T18:38:00Z"),
                PublishedAt = Utc("2026-01-27T18:38:00Z")
            },
            new Kami
            {
                KamiId = 2,
                NameEn = "Sugawara no Michizane",
                NameJp = "菅原道真",
                ImgId = null,
                Desc = "Often associated with Tenmangu shrines (fixture text).",
                Status = "review",
                CreatedAt = Utc("2026-01-27T18:38:30Z"),
                UpdatedAt = Utc("2026-01-27T18:38:30Z"),
                PublishedAt = Utc("2026-01-27T18:40:00Z")
            },
            new Kami
            {
                KamiId = 3,
                NameEn = "Uka-no-Mitama no Kami",
                NameJp = "宇迦之御魂神",
                ImgId = null,
                Desc = "A deity closely associated with agriculture, fertility, and prosperity, frequently linked to Inari worship. Uka-no-Mitama is venerated as a provider of abundant harvests and business success, and in many traditions is understood as one of the core identities behind Inari. Over centuries, local beliefs, syncretic practices, and regional folklore have shaped how this kami is perceived, sometimes merging attributes with other food and grain deities. This longer fixture text is intended to test extended content rendering, scrolling behavior, and layout stability in the CMS and mobile views.",
                Status = "published",
                CreatedAt = Utc("2026-01-27T18:38:50Z"),
                UpdatedAt = Utc("2026-01-27T18:38:50Z"),
                PublishedAt = Utc("2026-01-27T18:38:50Z")
            }
        );

        await db.SaveChangesAsync();

        // -----------------------
        // 7) Shrine <-> Kami links
        // -----------------------
        db.ShrineKamis.AddRange(
            new ShrineKami { ShrineId = 1, KamiId = 1, CreatedAt = Utc("2026-01-27T18:40:20Z") },
            new ShrineKami { ShrineId = 2, KamiId = 2, CreatedAt = Utc("2026-01-27T18:41:20Z") },
            new ShrineKami { ShrineId = 1, KamiId = 3, CreatedAt = Utc("2026-01-27T18:40:25Z") }
        );

        await db.SaveChangesAsync();

        // -----------------------
        // 8) Shrine Gallery (Shrine <-> Image)
        // -----------------------
        db.ShrineGalleries.AddRange(
            new ShrineGallery { ShrineId = 1, ImgId = 201, CreatedAt = Utc("2026-01-27T18:40:30Z") },
            new ShrineGallery { ShrineId = 1, ImgId = 202, CreatedAt = Utc("2026-01-27T18:40:30Z") },
            new ShrineGallery { ShrineId = 1, ImgId = 102, CreatedAt = Utc("2026-01-27T18:40:30Z") },
            new ShrineGallery { ShrineId = 2, ImgId = 201, CreatedAt = Utc("2026-01-27T18:40:30Z") },
            new ShrineGallery { ShrineId = 2, ImgId = 101, CreatedAt = Utc("2026-01-27T18:40:30Z") }
        );

        await db.SaveChangesAsync();

        // -----------------------
        // 9) History
        // -----------------------
        db.Histories.AddRange(
            new History
            {
                HistoryId = 1,
                ShrineId = 1,
                EventDate = DateOnly.Parse("1890-01-01"),
                SortOrder = 1,
                Title = "Founded (Fixture)",
                Information = "Fixture history event used for UI timelines and citation linking.",
                ImgId = null,
                Status = "published",
                CreatedAt = Utc("2026-01-27T18:42:00Z"),
                UpdatedAt = Utc("2026-01-27T18:42:00Z"),
                PublishedAt = Utc("2026-01-27T18:42:00Z")
            },
            new History
            {
                HistoryId = 3,
                ShrineId = 1,
                EventDate = DateOnly.Parse("1923-09-01"),
                SortOrder = 2,
                Title = "Damage During Regional Disaster (Fixture)",
                Information = "Fixture event representing partial damage and later repairs after a regional disaster.",
                ImgId = 102,
                Status = "published",
                CreatedAt = Utc("2026-01-27T19:00:00Z"),
                UpdatedAt = Utc("2026-01-27T19:00:00Z"),
                PublishedAt = Utc("2026-01-27T19:00:00Z")
            },
            new History
            {
                HistoryId = 4,
                ShrineId = 1,
                EventDate = DateOnly.Parse("1978-01-01"),
                SortOrder = 3,
                Title = "Major Renovation (Fixture)",
                Information = "Fixture renovation event used to test longer timelines and multiple citations per shrine.",
                ImgId = 202,
                Status = "review",
                CreatedAt = Utc("2026-01-27T19:10:00Z"),
                UpdatedAt = Utc("2026-01-27T19:10:00Z"),
                PublishedAt = Utc("2026-01-27T18:40:00Z")
            },
            new History
            {
                HistoryId = 2,
                ShrineId = 2,
                EventDate = DateOnly.Parse("1955-01-01"),
                SortOrder = 1,
                Title = "Rebuilt (Fixture)",
                Information = "Fixture event for testing ordering, status, and citations.",
                ImgId = null,
                Status = "review",
                CreatedAt = Utc("2026-01-27T18:42:30Z"),
                UpdatedAt = Utc("2026-01-27T18:42:30Z"),
                PublishedAt = Utc("2026-01-27T18:40:00Z")
            },
            new History
            {
                HistoryId = 5,
                ShrineId = 2,
                EventDate = DateOnly.Parse("1989-01-01"),
                SortOrder = 2,
                Title = "Precinct Expansion (Fixture)",
                Information = "Fixture event representing expansion of shrine grounds and facilities.",
                ImgId = 202,
                Status = "published",
                CreatedAt = Utc("2026-01-27T19:20:00Z"),
                UpdatedAt = Utc("2026-01-27T19:20:00Z"),
                PublishedAt = Utc("2026-01-27T19:20:00Z")
            },
            new History
            {
                HistoryId = 6,
                ShrineId = 2,
                EventDate = DateOnly.Parse("2011-03-11"),
                SortOrder = 3,
                Title = "Community Restoration Efforts (Fixture)",
                Information = "Fixture event noting post-disaster community-led restoration and support activities.",
                ImgId = null,
                Status = "published",
                CreatedAt = Utc("2026-01-27T19:30:00Z"),
                UpdatedAt = Utc("2026-01-27T19:30:00Z"),
                PublishedAt = Utc("2026-01-27T19:30:00Z")
            }
        );

        await db.SaveChangesAsync();

        // -----------------------
        // 10) Folklore
        // -----------------------
        db.Folklores.AddRange(
            new Folklore
            {
                FolkloreId = 1,
                ShrineId = 1,
                Title = "Fox Messenger Tale (Fixture)",
                Information = "A short placeholder folklore story to test long-form content rendering and citations.",
                ImgId = 201,
                Status = "published",
                CreatedAt = Utc("2026-01-27T18:43:00Z"),
                UpdatedAt = Utc("2026-01-27T18:43:00Z"),
                PublishedAt = Utc("2026-01-27T18:43:00Z")
            },
            new Folklore
            {
                FolkloreId = 2,
                ShrineId = 2,
                Title = "The Whispering Cedar",
                Information = "Lorem ipsum dolor sit amet consectetur adipiscing elit. Quisque faucibus ex sapien vitae pellentesque sem placerat. In id cursus mi pretium tellus duis convallis. Tempus leo eu aenean sed diam urna tempor. Pulvinar vivamus fringilla lacus nec metus bibendum egestas. Iaculis massa nisl malesuada lacinia integer nunc posuere. Ut hendrerit semper vel class aptent taciti sociosqu. Ad litora torquent per conubia nostra inceptos himenaeos.Lorem ipsum dolor sit amet consectetur adipiscing elit. Quisque faucibus ex sapien vitae pellentesque sem placerat. In id cursus mi pretium tellus duis convallis. Tempus leo eu aenean sed diam urna tempor. Pulvinar vivamus fringilla lacus nec metus bibendum egestas. Iaculis massa nisl malesuada lacinia integer nunc posuere. Ut hendrerit semper vel class aptent taciti sociosqu. Ad litora torquent per conubia nostra inceptos himenaeos.Lorem ipsum dolor sit amet consectetur adipiscing elit. Quisque faucibus ex sapien vitae pellentesque sem placerat. In id cursus mi pretium tellus duis convallis. Tempus leo eu aenean sed diam urna tempor. Pulvinar vivamus fringilla lacus nec metus bibendum egestas. Iaculis massa nisl malesuada lacinia integer nunc posuere. Ut hendrerit semper vel class aptent taciti sociosqu. Ad litora torquent per conubia nostra inceptos himenaeos.Lorem ipsum dolor sit amet consectetur adipiscing elit. Quisque faucibus ex sapien vitae pellentesque sem placerat. In id cursus mi pretium tellus duis convallis. Tempus leo eu aenean sed diam urna tempor. Pulvinar vivamus fringilla lacus nec metus bibendum egestas. Iaculis massa nisl malesuada lacinia integer nunc posuere. Ut hendrerit semper vel class aptent taciti sociosqu. Ad litora torquent per conubia nostra inceptos himenaeos.",
                ImgId = 201,
                Status = "published",
                CreatedAt = Utc("2026-02-05T17:00:00Z"),
                UpdatedAt = Utc("2026-02-05T17:00:00Z"),
                PublishedAt = Utc("2026-02-05T17:00:00Z")
            },
            new Folklore
            {
                FolkloreId = 3,
                ShrineId = 2,
                Title = "The Lantern of Returning",
                Information = "A paper lantern once lit itself during a storm, guiding a lost child back to the shrine. Since then, visitors leave small lantern charms praying for safe returns.",
                ImgId = null,
                Status = "published",
                CreatedAt = Utc("2026-02-05T17:05:00Z"),
                UpdatedAt = Utc("2026-02-05T17:05:00Z"),
                PublishedAt = Utc("2026-02-05T17:05:00Z")
            },
            new Folklore
            {
                FolkloreId = 4,
                ShrineId = 2,
                Title = "The Fox’s Hidden Path",
                Information = "It is said a white fox appears on misty mornings, walking a path only the sincere can see. Following it supposedly leads to a small stone offering site behind the shrine.",
                ImgId = null,
                Status = "draft",
                CreatedAt = Utc("2026-02-05T17:10:00Z"),
                UpdatedAt = Utc("2026-02-05T17:10:00Z"),
                PublishedAt = Utc("2026-02-05T17:00:00Z")
            }
        );

        await db.SaveChangesAsync();

        // -----------------------
        // 11) History <-> Citation links
        // -----------------------
        db.HistoryCitations.AddRange(
            new HistoryCitation { HistoryId = 1, CiteId = 1, CreatedAt = Utc("2026-01-27T18:42:10Z") },
            new HistoryCitation { HistoryId = 2, CiteId = 1, CreatedAt = Utc("2026-01-27T18:42:40Z") },
            new HistoryCitation { HistoryId = 3, CiteId = 2, CreatedAt = Utc("2026-01-27T19:00:10Z") },
            new HistoryCitation { HistoryId = 4, CiteId = 1, CreatedAt = Utc("2026-01-27T19:10:10Z") },
            new HistoryCitation { HistoryId = 5, CiteId = 2, CreatedAt = Utc("2026-01-27T19:20:10Z") },
            new HistoryCitation { HistoryId = 6, CiteId = 1, CreatedAt = Utc("2026-01-27T19:30:10Z") }
        );

        await db.SaveChangesAsync();

        // -----------------------
        // 12) Folklore <-> Citation links
        // -----------------------
        db.FolkloreCitations.AddRange(
            new FolkloreCitation { FolkloreId = 1, CiteId = 1, CreatedAt = Utc("2026-01-27T18:43:10Z") },

            new FolkloreCitation { FolkloreId = 2, CiteId = 1, CreatedAt = Utc("2026-01-27T18:43:10Z") },
            new FolkloreCitation { FolkloreId = 2, CiteId = 2, CreatedAt = Utc("2026-01-27T18:43:10Z") },
            new FolkloreCitation { FolkloreId = 2, CiteId = 3, CreatedAt = Utc("2026-01-27T18:43:10Z") },

            new FolkloreCitation { FolkloreId = 3, CiteId = 1, CreatedAt = Utc("2026-01-27T18:43:10Z") },
            new FolkloreCitation { FolkloreId = 4, CiteId = 1, CreatedAt = Utc("2026-01-27T18:43:10Z") }
        );

        await db.SaveChangesAsync();

        // -----------------------
        // 13) Kami <-> Citation links
        // -----------------------
        db.KamiCitations.AddRange(
            new KamiCitation { KamiId = 1, CiteId = 1, CreatedAt = Utc("2026-01-27T18:38:10Z") },
            new KamiCitation { KamiId = 2, CiteId = 1, CreatedAt = Utc("2026-01-27T18:38:40Z") },
            new KamiCitation { KamiId = 1, CiteId = 2, CreatedAt = Utc("2026-01-27T18:38:10Z") },
            new KamiCitation { KamiId = 3, CiteId = 1, CreatedAt = Utc("2026-01-27T18:38:10Z") }
        );

        await db.SaveChangesAsync();

        // -----------------------
        // 14) Etiquette Topics
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
                PublishedAt = Utc("2026-02-03T09:00:00Z"),
                CreatedAt = Utc("2026-02-01T12:45:00Z"),
                UpdatedAt = Utc("2026-02-03T09:00:00Z")
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
                PublishedAt = Utc("2026-02-03T09:00:00Z"),
                CreatedAt = Utc("2026-02-01T12:46:00Z"),
                UpdatedAt = Utc("2026-02-03T09:00:00Z")
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
                PublishedAt = Utc("2026-02-03T09:00:00Z"),
                CreatedAt = Utc("2026-02-01T12:47:00Z"),
                UpdatedAt = Utc("2026-02-03T09:00:00Z")
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
                PublishedAt = Utc("2026-02-03T09:00:00Z"),
                CreatedAt = Utc("2026-02-01T12:48:00Z"),
                UpdatedAt = Utc("2026-02-03T09:00:00Z")
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
                PublishedAt = Utc("2026-02-03T09:00:00Z"),
                CreatedAt = Utc("2026-02-01T12:49:00Z"),
                UpdatedAt = Utc("2026-02-03T09:00:00Z")
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
                PublishedAt = Utc("2026-02-03T09:00:00Z"),
                CreatedAt = Utc("2026-02-01T12:50:00Z"),
                UpdatedAt = Utc("2026-02-03T09:00:00Z")
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
                PublishedAt = Utc("2026-02-03T09:00:00Z"),
                CreatedAt = Utc("2026-02-01T12:51:00Z"),
                UpdatedAt = Utc("2026-02-03T09:00:00Z")
            }
        );

        await db.SaveChangesAsync();

        // -----------------------
        // 15) Etiquette Steps
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
        // 16) Etiquette Topic <-> Citation links
        // -----------------------
        db.EtiquetteCitations.AddRange(
            new EtiquetteTopicCitation { TopicId = 1, CiteId = 5, CreatedAt = Utc("2026-02-03T09:00:00Z") },
            new EtiquetteTopicCitation { TopicId = 2, CiteId = 4, CreatedAt = Utc("2026-02-03T09:00:00Z") },
            new EtiquetteTopicCitation { TopicId = 3, CiteId = 6, CreatedAt = Utc("2026-02-03T09:00:00Z") },
            new EtiquetteTopicCitation { TopicId = 3, CiteId = 5, CreatedAt = Utc("2026-02-03T09:00:00Z") },
            new EtiquetteTopicCitation { TopicId = 4, CiteId = 4, CreatedAt = Utc("2026-02-03T09:00:00Z") },
            new EtiquetteTopicCitation { TopicId = 4, CiteId = 5, CreatedAt = Utc("2026-02-03T09:00:00Z") },
            new EtiquetteTopicCitation { TopicId = 5, CiteId = 7, CreatedAt = Utc("2026-02-03T09:00:00Z") },
            new EtiquetteTopicCitation { TopicId = 6, CiteId = 6, CreatedAt = Utc("2026-02-03T09:00:00Z") },
            new EtiquetteTopicCitation { TopicId = 7, CiteId = 8, CreatedAt = Utc("2026-02-03T09:00:00Z") }
        );

        await db.SaveChangesAsync();

        // -----------------------
        // 17) Reset sequences (Postgres)
        // -----------------------
        await db.Database.ExecuteSqlRawAsync("""
            SELECT setval(pg_get_serial_sequence('citations','cite_id'), (SELECT COALESCE(MAX(cite_id), 1) FROM citations));
            SELECT setval(pg_get_serial_sequence('images','img_id'), (SELECT COALESCE(MAX(img_id), 1) FROM images));

            SELECT setval(pg_get_serial_sequence('tags','tag_id'), (SELECT COALESCE(MAX(tag_id), 1) FROM tags));
            SELECT setval(pg_get_serial_sequence('shrines','shrine_id'), (SELECT COALESCE(MAX(shrine_id), 1) FROM shrines));
            SELECT setval(pg_get_serial_sequence('kami','kami_id'), (SELECT COALESCE(MAX(kami_id), 1) FROM kami));

            SELECT setval(pg_get_serial_sequence('history','history_id'), (SELECT COALESCE(MAX(history_id), 1) FROM history));
            SELECT setval(pg_get_serial_sequence('folklore','folklore_id'), (SELECT COALESCE(MAX(folklore_id), 1) FROM folklore));

            SELECT setval(pg_get_serial_sequence('etiquette_topics','topic_id'), (SELECT COALESCE(MAX(topic_id), 1) FROM etiquette_topics));
            SELECT setval(pg_get_serial_sequence('etiquette_steps','step_id'), (SELECT COALESCE(MAX(step_id), 1) FROM etiquette_steps));
        """);
    }
}