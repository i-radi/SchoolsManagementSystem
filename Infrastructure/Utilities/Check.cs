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
}
