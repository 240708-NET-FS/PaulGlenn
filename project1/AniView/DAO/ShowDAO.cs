using AniView.Entities; 

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
        return null; 
    }

    public Show GetById(int ID) 
    {
        return null ; 
    }

    public void Update(Show show)
    {
        
    }

    public void Delete( Show show)
    {

    }

}