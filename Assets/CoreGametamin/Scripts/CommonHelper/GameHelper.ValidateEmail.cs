using System.Text.RegularExpressions;

namespace Gametamin.Core
{
    public static partial class GameHelper
    {
        const string MatchEmailPattern = @"(\w[-._\w]*\w@\w[-._\w]*\w\.\w{2,3})";
        public static bool ValidateEmail(this string email)
        {
            if (email != null)
                return Regex.IsMatch(email, MatchEmailPattern);
            else
                return false;
        }
    }
}
