using System.Net.Http.Headers;
using AniView.Entities;
using AniView.Service;
using Newtonsoft.Json; 

namespace AniView.Controller; 
public class ShowController 
{

  private string _userName; 
  private User _User; 
  private DateTime _reqDate = DateTime.Now; 



  private readonly ShowService _showService; 


  private static string _baseURL = "https://api.jikan.moe/v4/anime?q=";  

  private static HttpClient s_client = new() 
  {
    BaseAddress = new Uri(_baseURL)
  }; 

  public ShowController(User user, ShowService service ) 
  {  
    _User = user; 
    _userName = _User.UserName ; 
    _showService = service; 
  }

 public void SorrySendoff() {
    Console.WriteLine("Sorry we couldn't find what you were looking for! ");
    Console.WriteLine("We hope you'll come again. 🫡");
 }

  public void Prompt() {
    string capitalizedName = string.Concat(_userName[0].ToString().ToUpper(), _userName.AsSpan(1));
    string prompt = ""; 
    prompt += $"Hello {capitalizedName}! Welcome to AniView, the anime watch tracker app!\n";  
    prompt += "What would you like to do? \n " ;
    prompt += "1. Add a new show \n " ; 
    prompt += "2. Display all shows  (and possibly make changes) \n " ; 


    Console.WriteLine(prompt);
    string choice = Console.ReadLine() ?? "";
    switch(choice) 
    {
      case "1": 
        PromptNewAnime(); 
        break; 
      case "2": 
        DisplayShows(); 
        break; 
      default: 
        return; 
    } 

  }

 

  public bool PromptNewAnime() 
  {
    Console.WriteLine("Enter the name of an anime you want to add to your list: ");

    string name =  Console.ReadLine() ?? "";

    if (!Validator.InputIsNotEmpty(name)) {

      Console.WriteLine("You didn't enter anything! 😭");
      Console.WriteLine("Try again. \n");
      return false ;
    } else {
      
      AddNewShow(name).GetAwaiter().GetResult(); 
      return true; 
    }
  }


  // private string FormatDateTime() {
  //   string dateTimeString = "" ;
  //   dateTimeString += _reqDate.Year + "-";  
  //   dateTimeString += _reqDate.Month + "-";
  //   dateTimeString += _reqDate.Day + "_";
  //   dateTimeString += _reqDate.Hour + ":";
  //   dateTimeString += _reqDate.Minute + ":";
  //   dateTimeString += _reqDate.Second;
   
  //  return dateTimeString;  
  // }

  // GetCat retrieves cat with _tags from CatAPI 
  async public Task AddNewShow(string name) 
  {
    Console.WriteLine($" Name: {name} \n");
    Console.WriteLine($" Retreiving Anime ... ");
    // record precise time API call was made 
    _reqDate = DateTime.Now; 

    string completeURL = _baseURL + $"{name}&sfw"; 
    using HttpResponseMessage response = await s_client.GetAsync(completeURL); 

    try 
    {
        response.EnsureSuccessStatusCode();
        var streamResponse = await response.Content.ReadAsStringAsync();
        // Console.WriteLine(streamResponse);
        File.WriteAllText("./obs/traces/HttpResponse.txt",streamResponse); 

        //  deserialize JSON 
        AnimeList animeList = JsonConvert.DeserializeObject<AnimeList>(streamResponse);
        if ( animeList.data.Length > 0 ) {
          Console.WriteLine("Found the following anime matches under that search: ");
          for ( int i = 0; i < animeList.data.Length; i++ ) 
          {
              Console.WriteLine($"{i+1}.  {animeList.data[i].ToString()}" );
          }

        } else {
          // nothing was found for them. Just exit after an apology 
          SorrySendoff() ; 
          return; 
        }
        

        Console.WriteLine("Would you like to add one of these? (yes/no)");

        string add = Console.ReadLine() ?? "" ; 
        switch (add.ToLower()) {
          case "yes": 
           // now have them select anime from list 
            Console.WriteLine("Which one would you like to track? Enter the number");
            int choice = GetShowChoice() - 1 ;//array indexing  

            AnimeListAnime anime = animeList.data[choice];
            Console.WriteLine($"Your choice: {anime.ToString()}");

            // next: add this to DB 
            _showService.Create(_User,anime);
            break; 
          default: 
            SorrySendoff(); 
            break; 
    

        }

    } catch (HttpRequestException e) {
     SorrySendoff(); 
      
      
      // log exception to file 
      File.WriteAllText("./obs/logs/controllerLog.txt",e.Message); 
    }




  }


  // display shows 

  public void DisplayShows() 
  {
    Console.WriteLine("Here is your current list of shows: ");
    List<Show> shows = (List<Show>) _showService.GetAllByUserName(_userName); 
    bool editMode = false ; 
    switch(shows.Count) {
      case 0: 
        Console.WriteLine("You have no shows added!");
        break; 
      default: 
        Console.WriteLine("Here is your current list of shows: ");
        for(int i = 0; i < shows.Count; i++ ) 
        {
          Printer.PrintShow(shows.ElementAt(i));
        }

        // prompt to make additional changes
        Console.WriteLine("Would you like to make changes? (yes/no)");
        if(Console.ReadLine().ToLower() == "yes") EditShows(); 
        break; 

    }
  
  }

  private static int GetShowChoice()
  {
      return Int32.Parse(Console.ReadLine());
  }

  public void EditShows() 
  {
    // will prompt the user to make show edits. able to call Delete() and Update() 
    // the user will enter the ID of the anime they want to edit / delete 
    Console.WriteLine("Enter the number of the show you would like to edit from the list above.");
    int showChoice  = GetShowChoice(); 

    // we will get show by ID 
    Show show = _showService.GetById(showChoice); 
    Console.WriteLine("Your selection: ");
    Printer.PrintShow(show); 

    // prompt user to choose what they want to do 
    Console.WriteLine("What would you like to do with this show?");
    Console.WriteLine("1. Edit Last Episode Watched"); 
    Console.WriteLine("2. Mark/unmark show as a favorite");
    Console.WriteLine("3. Delete Show");

    int editChoice = GetShowChoice() ; 
    switch(editChoice) {
      case 1: 
        // edit last episode watched 
        System.Console.WriteLine($"Enter a number between 0(for unwatched) and {show.Episodes}: ");
        int episodeChoice=  GetShowChoice() ; 
        if ( episodeChoice <= show.Episodes) {
          show.DateLastWatched = DateTime.Now; 
          show.LastEpisodeWatched = episodeChoice ; 
          _showService.Update(show); 
        }
        Console.WriteLine($"Show {show.Name} had its last episode watched set to {show.LastEpisodeWatched} out of {show.Episodes} 🫡");
        break; 
      case 2: 
        // mark as favorite 
        show.Favorite = !show.Favorite; 
        _showService.Update(show);
        string favStatus = show.Favorite ? "marked" : "unmarked"; 
        Console.WriteLine($"Show {show.Name} has been {favStatus} as a favorite🫡");

        break; 
      case 3: 
        // delete show 
        _showService.Delete(show); 
        Console.WriteLine($"Show {show.Name} has been removed from your list 🫡");
        break; 
      default: 
        break;

    }



  }

}
  