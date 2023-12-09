using Enterprise.RegularExpressions.Evaluators;
using Enterprise.RegularExpressions.Patterns;
using System.Text.RegularExpressions;
using static Enterprise.Constants.CharacterConstants;
using static System.StringComparison;

namespace Enterprise.RegularExpressions.Services;

public static class EmailAddressMaskingService
{
    public static string MaskEmailAddress(string emailAddress)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
            return emailAddress;

        int indexOf = emailAddress.IndexOf(AtSymbol, Ordinal);

        if (indexOf == -1)
            return emailAddress;
            
        string maskCandidate = emailAddress[..indexOf];

        if (maskCandidate.Length <= 2)
        {
            // too short to mask...
            // TODO: should we throw an error, or just mask the whole thing?
            return emailAddress;
        }

        string input = $"{maskCandidate}{AtSymbol}";
        string pattern = RegexPatterns.EmailAddressPrefixInner;
        MatchEvaluator matchEvaluator = MatchEvaluators.ReplaceWithAsterisk;

        string masked = Regex.Replace(input, pattern, matchEvaluator);

        string maskedEmailAddress = emailAddress.Replace(input, masked);

        return maskedEmailAddress;
    }
}