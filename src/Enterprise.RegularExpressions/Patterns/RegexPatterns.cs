namespace Enterprise.RegularExpressions.Patterns;

public static class RegexPatterns
{
    /// <summary>
    /// Regular expression that matches the inner characters inside an email prefix (before the @ sign).
    /// The first and last characters are omitted.
    /// This requires at least two characters before the @ sign.
    /// </summary>
    public const string EmailAddressPrefixInner = @"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@)";
}