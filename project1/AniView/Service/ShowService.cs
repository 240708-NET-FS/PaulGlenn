using AniView.DAO; 
using AniView.Entities;
using AniView.Utilities;
using Microsoft.EntityFrameworkCore.Storage;


namespace AniView.Service; 

public class ShowService(ShowDAO showDAO)
{ 
    // CRUD
    private readonly ShowDAO _showDAO = showDAO;

    public Show GetById(int Id) {
        return _showDAO.GetById(Id) ; 
    }

    public List<Show> GetAll()
    {
       List<Show> showList = (List<Show>) _showDAO.GetAll() ;
       return showList; 
    }

    public List<Show> GetAllByUserName(string userName) {
        return _showDAO.GetAllByUserName(userName);
    }

    public bool Create(User user, Anime anime)
    {
        // user should provide date last watched and last episode watched 
        // for now let's just have them provide the last episode watched. 
    
        Show? existingShow  = _showDAO.GetByName( user, anime.title_english);
        if(existingShow != null) 
        {
            System.Console.WriteLine("You've already added this show!");
            return false ; 
        }
        System.Console.WriteLine($"What is the last episode you watched (enter 0 for none, out of {anime.episodes}): ");
        string lastEpisodeWatchedString = Console.ReadLine() ?? "0";
        int lastEpisodeWatched = Int32.Parse(lastEpisodeWatchedString);

        // 
        Show show  = new() {Name= anime.title_english, DateLastWatched= DateTime.Now, Episodes=anime.episodes,isAiring = anime.airing, LastEpisodeWatched=lastEpisodeWatched, Favorite=false , UserID = user.UserID  }; 

        _showDAO.Create(show); 
        return true; 
    }

    public void Delete(Show item)
    {
        _showDAO.Delete(item); 
    }

    public void Update(Show item)
    {
        _showDAO.Update(item);
    }

    public bool ChangeFavoriteStatus(Show item) {
        item.Favorite = !item.Favorite; 
        _showDAO.Update(item);
        return item.Favorite; 
    }

    public bool ChangeLastWatched(Show item, int episode) {
          if ( episode >= 0 && episode <= item.Episodes) {
          item.DateLastWatched = DateTime.Now; 
          item.LastEpisodeWatched = episode ; 
          _showDAO.Update(item); 
          return true; 
        }
        return false; 
    }

}