using AniView.Entities;
using AniView.Utilities;

public class ShowPrinter : IPrinter<Show> {
    public static void Print(Show show ) {

        string nameString = $"{show.ShowID}. {show.Name}";
        string lastEpisodeWatchedString = $"Episode {show.LastEpisodeWatched} of {show.Episodes}";
        string dateLastWatchedString = $"Last watched on {show.DateLastWatched.Date}"; 
        string isFavorite = show.Favorite ? "Yes" : "No" ;
        string isAiring = show.isAiring ? "Yes" : "No"; 

        // find max string length 
        int maxLength = nameString.Length; 
        if ( lastEpisodeWatchedString.Length > maxLength ) maxLength  = lastEpisodeWatchedString.Length; 
        if ( dateLastWatchedString.Length > maxLength ) maxLength  = dateLastWatchedString.Length; 
        System.Console.WriteLine(nameString);
        System.Console.WriteLine(dateLastWatchedString);
        System.Console.WriteLine(lastEpisodeWatchedString);
        System.Console.WriteLine($"Still Airing? {isAiring}");
        System.Console.WriteLine($"Favorited? {isFavorite}");
        System.Console.WriteLine(new string('-',maxLength));
    }

    }
