using System.Reflection.Metadata.Ecma335;
using AniView.Entities; 
using AniView.Service; 
using AniView.Utilities;

namespace AniView.Controller;

public class UserController(UserService service)
{
    private readonly UserService _userService = service;
    private string _name ; 
    private User User;

    public User GetUser() {
        return User; 
    }

    public void RunUserUI() {
        System.Console.WriteLine("Hello! What is your name?");
        string name = Console.ReadLine() ?? "" ; 
        while(!Validator.CheckNameIsValid(name)){
            System.Console.WriteLine("That name is invalid. Try again (we will save this with your cats!)");
            System.Console.WriteLine("Enter your name: ");
            name = Console.ReadLine()  ?? "" ; 
        }
        _name = name ; 

        User = GetExistingUser(); 

        User ??= AddNewUser(); 

        // if user == null 
        // add new user 


    }
    private User GetExistingUser() {
        return  _userService.GetByName(_name);
    }

    private User AddNewUser(){
        //System.Console.WriteLine("What password would you like to prote");
        _userService.Create(new(){UserName=_name,Passphrase="password"}); 
       return GetExistingUser();

    }
    
    
}