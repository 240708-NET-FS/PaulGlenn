using System.Text.RegularExpressions; 
class Validator {
    Validator() {} 

    public static bool InputIsNotEmpty(string str) {
        if (str == "" ) return false ; 
        return true ;
    }


    // NameIsValid: if 
    // 1. string is not empty and 
    // 2. does not start with a number 
    public static bool NameIsValid(string str) {
        if ( !InputIsNotEmpty(str)) return false; 

        Match match = Regex.Match(str,@"^[0-9]");
        if( match.Length > 0) return false; 
        return true; 
    }
}