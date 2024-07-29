using System.Reflection.Metadata.Ecma335;
using AniView.Entities; 
using AniView.Service; 
using AniView.Utilities;

namespace AniView.Controller;

public class UserController(UserService service)
{
    private readonly UserService _userService = service;
    private string _name = "" ; 
    private User? User;

    public User? GetUser() {
        return User; 
    }

    public User RunUserUI() {

        bool isLoggedIn = false; 

        while (!isLoggedIn)
        {
            
            _name = InputRetriever.GetUserName();

            User = GetExistingUser(); 
            if ( User == null ) 
            {
                isLoggedIn = AddNewUser() ; 
            }
            else 
            {
                isLoggedIn = LoginUser(); 
            }
        }
        User=GetExistingUser();

        return User; 
        

    }

    private User GetExistingUser() {
        return  _userService.GetByName(_name);
    }

    private bool LoginUser() {

        bool exitLoginLoop = false; 
        while(!exitLoginLoop)
        {
            System.Console.WriteLine("Enter password: ");
            string password = Console.ReadLine() ?? "" ; 
            User unauthUser=  new() {UserName=_name, Passphrase=password};
            if(_userService.Authenticate(unauthUser)) return true;
            else {
                Console.WriteLine("That username or password was incorrect. Enter 1 to try again or anything else to sign up instead. ");
                int choice = InputRetriever.GetChoice();
                if ( choice != 1) exitLoginLoop= true ; 

            }
        }
        return false; 
        
       
    }
    private bool AddNewUser(){
        string password = InputRetriever.GetPassword(); 
        _userService.Create(new(){UserName=_name,Passphrase=password}); 
       return true; 

    }
    
    
}