using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Y22.Extensions;

public static class ParsingExtensions
{
    public static (T1, T2, T3, T4) RegexParse<T1, T2, T3, T4>(
        this string input,
        [StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
        where T1 : IParsable<T1>
        where T2 : IParsable<T2>
        where T3 : IParsable<T3>
        where T4 : IParsable<T4>
    {
        var groups = new Regex(pattern).Match(input).Groups.Values.Skip(1).ToList();
        return (
            groups[0].Value.Parse<T1>(),
            groups[1].Value.Parse<T2>(),
            groups[2].Value.Parse<T3>(),
            groups[3].Value.Parse<T4>()
        );
    }
}