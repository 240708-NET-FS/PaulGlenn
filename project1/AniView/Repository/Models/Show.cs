namespace AniView.Entities; 

public class Show {
    public int ShowID {get;set; }
    public string Name {get; set; }

    public int UserID {get;set; }

    public int Episodes {get; set; }

   
    public int LastEpisodeWatched {get; set ; }
   

    public bool isAiring {get; set; }

    public DateTime DateLastWatched {get; set; }

    public bool Favorite {get;set; }

    public User User {get; set; }


}