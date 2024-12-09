using System.Text.RegularExpressions;

public interface IRegexParsable<TSelf> where TSelf : IRegexParsable<TSelf>?
{
    static abstract TSelf Parse(Match match);
}