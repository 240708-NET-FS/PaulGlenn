using AniView.Entities;
using AniView.Controller;
using AniView.DAO;
using AniView.Service;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;

namespace AniView ;
public class AniView
{
    static void Main(string[] args) 
    {


        // get user name 
        string name = "" ; 
        System.Console.WriteLine("Hello! What is your name?");
        name = Console.ReadLine() ?? "" ; 
        while(!Validator.NameIsValid(name)){
            System.Console.WriteLine("That name is invalid. Try again (we will save this with your cats!)");
            System.Console.WriteLine("Enter your name: ");
            name = Console.ReadLine()  ?? "" ; 

        }

        // this concludes the functionality for retrieving one cat. todo next week : add to favorites list which we can store in a database 
        using (var context = new ApplicationDbContext()) 
        {
            // get user 
            UserDAO userDAO = new(context); 
            UserService userService = new(userDAO); 
            User user = userService.GetById(1);

            ShowDAO showDAO = new(context); 
            ShowService showService = new(showDAO); 

            ShowController showController = new(user,showService) ; 

            bool continuity = true; 
            while(continuity) 
            {
                showController.Prompt(); 
                System.Console.WriteLine("Would you like to continue using the app?");
                if(Console.ReadLine().ToLower() != "yes") continuity = false; 
            }
                

        }

        System.Console.WriteLine("Have a great time watching anime! 👋🏾");
        
    }
}
