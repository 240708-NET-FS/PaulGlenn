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

    public ICollection<User> GetAll()
    {
        return null ; 
    }

    public void Create(User item)
    {

    }

    public void Delete(User item)
    {

    }

    public void Update(User item)
    {

    }

}