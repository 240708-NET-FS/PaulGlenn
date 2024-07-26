namespace AniView.Utilities.Printers; 
public class AnimeListPrinter : IPrinter<Anime[]> {

    public static void Print(Anime[] animeList) {
            for ( int i = 0; i < animeList.Length; i++ ) 
          {
              Console.WriteLine($"{i+1}.  {animeList[i].ToString()}" );
          }
    }
}