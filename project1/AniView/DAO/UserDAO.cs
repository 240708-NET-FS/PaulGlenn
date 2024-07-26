using AniView.Entities; 

namespace AniView.DAO; 

public class UserDAO (ApplicationDbContext context ) : IDAO<User> {

    private readonly ApplicationDbContext _context  = context; 

    // CRUD

    // Create
    public void Create(User item)
    {
      _context.Users.Add(item);
       _context.SaveChanges();  
    }

    // Read
    public User GetById(int ID)
    {
        User user = _context.Users.FirstOrDefault(u => u.UserID == ID);

        return user;
    }

    public User GetByName(string name) {
        User user =  _context.Users.FirstOrDefault(u=>u.UserName == name); 
        return user ;
    }
    public ICollection<User> GetAll()
    {
        return null ; 
    }

    // Update
    public void Update(User newItem)
    {

    }

    // Delete

    public void Delete(User item) {

    }

}