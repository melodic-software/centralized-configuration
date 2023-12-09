using System.Text.RegularExpressions;
using static Enterprise.Constants.CharacterConstants;

namespace Enterprise.RegularExpressions.Evaluators;

public static class MatchEvaluators
{
    public static MatchEvaluator ReplaceWithAsterisk = m => new string (Asterisk, m.Length);
}