using AniView.DAO; 
using AniView.Entities;
using dotenv.net; 


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

        // here is where we will handle hashing passwords
        item.Salt = DateTime.Now.ToString(); 
        item.Passphrase = HashPass(item.Passphrase, item.Salt); 
        _userDAO.Create(item); 
    }

    public bool Authenticate(User user) {
        // if user doesn't exist, return false
        User? existingUser  = GetByName(user.UserName);  
        
        if(existingUser == null) return false; 
    
        // hash password for comparison 
    
        string hashed = HashPass(user.Passphrase,existingUser.Salt); 
        

        //return true if passwords match , false if not 
        return existingUser.Passphrase == hashed; 
    }

    public void Delete(User item)
    {
        _userDAO.Delete(item);
    }

    public void Update(User item)
    {
        _userDAO.Update(item);
    }

    private static string HashPass(string str, string salt) {
        DotEnv.Load() ; 

        //string salt = Environment.GetEnvironmentVariable("SALT"); 
        
        string inputString = salt + salt + str + salt + salt; 
        byte[] data = System.Text.Encoding.ASCII.GetBytes(inputString);
        data = System.Security.Cryptography.SHA256.HashData(data);
        string hashedData = System.Text.Encoding.ASCII.GetString(data);
        return hashedData; 
    }

}