﻿//using AniViewApp.Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;

namespace AniView {
  class AniView
  {
     static void Main(string[] args) {
      string name = "" ; 
      System.Console.WriteLine("Hello! What is your name?");
      name = Console.ReadLine() ?? ""  ; 

    while(!Validator.NameIsValid(name)){
      System.Console.WriteLine("That name is invalid. Try again (we will save this with your cats!)");
      System.Console.WriteLine("Enter your name: ");
      name = Console.ReadLine()  ?? "" ; 

    }

    //add name to DB -- will move later to UserDAO
    //  using (var context = new ApplicationDbContext()) {
        
    //    // bool existingUser = !context.Users.Where(u =>( u.UserName == name)).ToList().IsNullOrEmpty();

    //     if ( existingUser == true  ) {
    //        // User newUser = new User{UserName=name, Passphrase="password",UserID=1};
    //         context.Users.Add(newUser); 
    //     } else {
    //       System.Console.WriteLine("Username is taken! Enter a new username. ");
    //     }
      
    //   }
    


      AnimeController animeRequest = new(name) ; 
      bool promptSuccess = false; 
      while(!promptSuccess ) {
        promptSuccess = animeRequest.Prompt(); 
      }
      animeRequest.GetAnime().GetAwaiter().GetResult(); 

      // this concludes the functionality for retrieving one cat. todo next week : add to favorites list which we can store in a database 

      
    }
  }
}