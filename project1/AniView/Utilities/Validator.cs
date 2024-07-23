using System.Text.RegularExpressions; 
namespace AniView.Utilities;

public partial class Validator {
    Validator() {} 

    public static bool CheckInputIsNotEmpty(string str) {
        if (str == "" ) return false ; 
        return true ;
    }


    // NameIsValid: if 
    // 1. string is not empty and 
    // 2. does not start with a number 
    public static bool CheckNameIsValid(string str) {
        if ( !CheckInputIsNotEmpty(str)) return false; 

        Match match = MyRegex().Match(str);
        if( match.Length > 0) return false; 
        return true; 
    }


    [GeneratedRegex(@"^[0-9]")]
    private static partial Regex MyRegex();
}