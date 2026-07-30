using System.Text.RegularExpressions;

namespace TsaElectronics.Api.Helpers;

public static partial class SlugHelper
{
    public static string Generate(string value)
    {
        var slug = value.Trim().ToLowerInvariant();
        slug = NonAlphaNumeric().Replace(slug, "-");
        slug = MultipleDashes().Replace(slug, "-");
        return slug.Trim('-');
    }

    [GeneratedRegex(@"[^a-z0-9]+")]
    private static partial Regex NonAlphaNumeric();

    [GeneratedRegex(@"-{2,}")]
    private static partial Regex MultipleDashes();
}
