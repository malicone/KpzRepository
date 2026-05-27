namespace KpzRepository.WebSite.Components;

/// <summary>
/// Builds the site copyright line shown in the page footer.
/// </summary>
public static class SiteCopyright
{
    private const int StartYear = 2026;

    /// <summary>
    /// When the current year equals <see cref="StartYear"/>, only that year is shown;
    /// otherwise the range is formatted as "start - current".
    /// </summary>
    public static string GetText(int? currentYear = null)
    {
        var year = currentYear ?? DateTime.UtcNow.Year;
        var yearPart = year <= StartYear
            ? year.ToString()
            : $"{StartYear} - {year}";

        return $"\u00a9 {yearPart} - KpzRepository";
    }
}
