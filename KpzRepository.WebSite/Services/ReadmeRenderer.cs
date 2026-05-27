using Markdig;

namespace KpzRepository.WebSite.Services;

/// <summary>
/// Converts the project README markdown file to HTML for display on the home page.
/// </summary>
public sealed class ReadmeRenderer
{
    private readonly MarkdownPipeline _pipeline;
    private readonly IWebHostEnvironment _environment;
    private string? _cachedHtml;

    public ReadmeRenderer(IWebHostEnvironment environment)
    {
        _environment = environment;
        _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();
    }

    /// <summary>
    /// Returns README content as HTML, loading and caching on first access.
    /// </summary>
    public string GetHtml()
    {
        if (_cachedHtml is not null)
        {
            return _cachedHtml;
        }

        var readmePath = ResolveReadmePath();
        if (readmePath is null)
        {
            _cachedHtml = "<p class=\"readme-error\">README.md was not found.</p>";
            return _cachedHtml;
        }

        var markdown = File.ReadAllText(readmePath);
        _cachedHtml = Markdown.ToHtml(markdown, _pipeline);
        return _cachedHtml;
    }

    private string? ResolveReadmePath()
    {
        var candidates = new[]
        {
            Path.Combine(_environment.ContentRootPath, "README.md"),
            Path.Combine(AppContext.BaseDirectory, "README.md"),
            Path.GetFullPath(Path.Combine(_environment.ContentRootPath, "..", "README.md")),
        };

        return candidates.FirstOrDefault(File.Exists);
    }
}
