using AniView.Entities; 

namespace AniView.DAO; 

public class UserDAO (ApplicationDbContext context ) {

    private readonly ApplicationDbContext _context  = context; 

    // CRUD

    // Create
    public void Create(User item)
    {
        
    }

    // Read
    public User GetById(int ID)
    {
        User user = _context.Users.FirstOrDefault(u => u.UserID == ID);

        return user;
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