using AniView.Entities;
using AniView.Controller;
using AniView.DAO;
using AniView.Service;


namespace AniView ;
public class AniView
{
    static void Main(string[] args) 
    {

        using (var context = new ApplicationDbContext()) 
        {
            // get user 
            UserDAO userDAO = new(context); 
            UserService userService = new(userDAO); 
            UserController userController = new UserController(userService); 
            userController.RunUserUI(); 
            User user = userController.GetUser();

            // run show app with user 
        
            ShowDAO showDAO = new(context); 
            ShowService showService = new(showDAO); 

            ShowController showController = new(user,showService) ; 

            showController.RunShowApp();
                

        }

        System.Console.WriteLine("Have a great time watching anime! 👋🏾");
        
    }
}
