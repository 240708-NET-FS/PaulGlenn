using AniView.DAO; 
using AniView.Entities;


namespace AniView.Service; 

public class ShowService(ShowDAO showDAO)
{ 
    // CRUD
    private readonly ShowDAO _showDAO = showDAO;

    public Show GetById(int Id) {
        return _showDAO.GetById(Id) ; 
    }

    public ICollection<Show> GetAll()
    {
       return _showDAO.GetAll() ;
    }

    public List<Show> GetAllByUserName(string userName) {
        return _showDAO.GetAllByUserName(userName);
    }

    public void Create(User user, AnimeListAnime anime)
    {
        // user should provide date last watched and last episode watched 
        // for now let's just have them provide the last episode watched. 

        System.Console.WriteLine($"What is the last episdoe you watched (enter 0 for none, out of {anime.episodes}): ");
        string lastEpisodeWatchedString = Console.ReadLine() ?? "0";
        int lastEpisodeWatched = Int32.Parse(lastEpisodeWatchedString);

        // 
        Show show  = new Show() {Name= anime.title_english, DateLastWatched= DateTime.Now, Episodes=anime.episodes,isAiring = anime.airing, LastEpisodeWatched=lastEpisodeWatched, Favorite=false , UserID = 1  }; 

        _showDAO.Create(show); 
    }

    public void Delete(Show item)
    {
        _showDAO.Delete(item); 
    }

    public void Update(Show item)
    {
        _showDAO.Update(item);
    }
}