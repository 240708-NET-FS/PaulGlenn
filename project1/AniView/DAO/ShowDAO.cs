using AniView.Entities;
using Microsoft.EntityFrameworkCore;

namespace AniView.DAO; 
public class ShowDAO(ApplicationDbContext context) : IDAO<Show> {
    private ApplicationDbContext _context = context;

    public void Create(Show item) 
    {
        _context.Shows.Add(item); 
        _context.SaveChanges();
    }

    public ICollection<Show> GetAll() 
    {
        List<Show> shows = [.. _context.Shows]; 
        return shows; 
    }

    public Show GetById(int ID) 
    {
        return _context.Shows.FirstOrDefault(s=>s.ShowID == ID) ; 
    }

    public Show GetByName(User user, string showName) {
        return _context.Shows.FirstOrDefault(s=>(s.Name == showName && s.UserID == user.UserID));
    }

    public List<Show> GetAllByUserName(string userName) {
        List<Show> shows = [.. _context.Shows.Where(s=>s.User.UserName == userName)]; 
        return shows ;
    }

    public void Update(Show show)
    {

        Show originalShow = GetById(show.ShowID); 
        originalShow.LastEpisodeWatched = show.LastEpisodeWatched; 
        originalShow.DateLastWatched = show.DateLastWatched; 
        originalShow.Favorite = show.Favorite; 

        _context.Shows.Update(originalShow); 
        _context.SaveChanges();

    }

    public void Delete(Show show)
    {
        _context.Shows.Remove(show); 
        _context.SaveChanges() ;
    }

}