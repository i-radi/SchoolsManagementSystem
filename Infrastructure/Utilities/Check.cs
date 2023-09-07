using System.Text.RegularExpressions;

namespace Infrastructure.Utilities;

public static class Check
{
    public static bool IsEmail(string input)
    {
        string pattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
        Regex regex = new Regex(pattern);
        Match match = regex.Match(input);

        return match.Success;
    }

    public static bool InLength(string input)
    {
        string pattern = @"^(?=.{3,50}$)[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z]+(?:\s[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z\d_-]+)?$|[\x20]*$";
        Regex regex = new Regex(pattern);
        Match match = regex.Match(input);

        return match.Success;
    }
}
