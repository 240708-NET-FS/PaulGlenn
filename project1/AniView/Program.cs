using AniView.Entities;
using AniView.Controller;
using AniView.DAO;
using AniView.Service;
using AniView.Utilities; 
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;

namespace AniView ;
public class AniView
{
    static void Main(string[] args) 
    {

        // get user name 
      
        // System.Console.WriteLine("Hello! What is your name?");
        // string name = Console.ReadLine() ?? "" ; 
        // while(!Validator.CheckNameIsValid(name)){
        //     System.Console.WriteLine("That name is invalid. Try again (we will save this with your cats!)");
        //     System.Console.WriteLine("Enter your name: ");
        //     name = Console.ReadLine()  ?? "" ; 

        // }

        // this concludes the functionality for retrieving one cat. todo next week : add to favorites list which we can store in a database 
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
