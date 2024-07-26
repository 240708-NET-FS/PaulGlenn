using AniView.DAO; 
using AniView.Entities;


namespace AniView.Service; 

public class UserService (UserDAO userDAO) : IService<User> {

    private readonly UserDAO _userDAO = userDAO ; 
      // CRUD
    public User GetById(int ID)
    {
        return _userDAO.GetById(ID); 
    }
    
    public User GetByName(string name) 
    {
        return _userDAO.GetByName(name);
    }

    public ICollection<User> GetAll()
    {
        return _userDAO.GetAll(); 
    }

    public void Create(User item)
    {
        // check that username and password --- ? 
        // check that username doesn't already exist 
        
        _userDAO.Create(item); 
    }

    public void Delete(User item)
    {
        _userDAO.Delete(item);
    }

    public void Update(User item)
    {
        _userDAO.Update(item);
    }

}