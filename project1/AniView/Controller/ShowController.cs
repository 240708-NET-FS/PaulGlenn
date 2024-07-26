
using AniView.Entities;
using AniView.Service;
using AniView.Utilities;
using AniView.Utilities.Printers; 


namespace AniView.Controller; 
public class ShowController 
{

  private readonly string _userName; 
  private readonly User _User; 
  private DateTime _reqDate = DateTime.Now; 



  private readonly ShowService _showService; 

  private readonly InputRetriever inputRetriever = new() ; 
  private static readonly string _baseURL = "https://api.jikan.moe/v4/anime?q=";  

  private static readonly HttpClient s_client = new() 
  {
    BaseAddress = new Uri(_baseURL)
  }; 

  public ShowController() {
    _userName = "testUser" ; 
  } 
  public ShowController(User user, ShowService service ) 
  {  
    _User = user; 
    _userName = _User.UserName ; 
    _showService = service; 
  }

 private static void SorrySendoff() {
    Console.WriteLine("Sorry we couldn't find what you were looking for! ");
    Console.WriteLine("We hope you'll come again. 🫡");
 }

  public void RunShowApp() {
       bool continuity = true; 
            while(continuity) 
            {
                RunShowAppUI(); 
                System.Console.WriteLine("Would you like to continue using the app?");
                string response = Console.ReadLine() ?? "no"; 
                if(!response.Equals("yes")) continuity = false; 
            }
  }
  public void RunShowAppUI() {
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
        DisplayAllShows(); 
        break; 
      default: 
        return; 
    } 

  }

 

  public bool PromptNewAnime() 
  {
    Console.WriteLine("Enter the name of an anime you want to add to your list: ");

    string name =  inputRetriever.GetName();

    if (!Validator.CheckInputIsNotEmpty(name)) {

      Console.WriteLine("You didn't enter anything! 😭");
      Console.WriteLine("Try again. \n");
      return false ;
    } else {
      
      AddNewShow(name).GetAwaiter().GetResult(); 
      return true; 
    }
  }



  async public Task<Anime[]> GetShowList(string name) 
  {
    Console.WriteLine($" Name: {name} \n");
    Console.WriteLine($" Retreiving Anime ... ");
    // record precise time API call was made 
    _reqDate = DateTime.Now; 
    AnimeListResponse animeListResponse = await AnimeApiRequest.GetAnimeList(name);
    return animeListResponse.data; 

  }

// handles user input for adding a new show 
  async public Task AddNewShow(string name) 
  {

  
    Anime[] animeList=  await GetShowList(name);
    if ( animeList.Length > 0 ) 
    {
      Console.WriteLine("Found the following anime matches under that search: ");
      AnimeListPrinter.Print(animeList);
    } else {
      // nothing was found for them. Just exit after an apology 
      SorrySendoff() ; 
    }
    
    Console.WriteLine("Would you like to add one of these to your watchlist? (yes/no)");

    string wantsToAdd = Console.ReadLine() ?? "" ; 
    switch (wantsToAdd.ToLower()) 
    {
      case "yes": 
        // now have them select anime from list 
        Console.WriteLine("Which one would you like to track? Enter the number");
        int choice = inputRetriever.GetChoice() - 1 ;//array indexing  

        Anime anime = animeList[choice];
        Console.WriteLine($"Your choice: {anime.ToString()}");

        // next: add this to DB 
        bool isAdded = _showService.Create(_User,anime);

        if (isAdded) System.Console.WriteLine("Added!");
        else System.Console.WriteLine("You have already added this show!");
        break; 
      default: 
        SorrySendoff(); 
        break; 


    }

  }

  // display shows 

  public void DisplayAllShows() 
  {
    Console.WriteLine("Here is your current list of shows: ");
    List<Show> shows = (List<Show>) _showService.GetAllByUserName(_userName); 
    switch(shows.Count) {
      case 0: 
        Console.WriteLine("You have no shows added!");
        break; 
      default: 
        Console.WriteLine("Here is your current list of shows: ");
        for(int i = 0; i < shows.Count; i++ ) 
        {
         ShowPrinter.Print(shows.ElementAt(i));
        }

        // prompt to make additional changes
        Console.WriteLine("Would you like to make changes? (yes/no)");
        string answer = Console.ReadLine() ?? "no" ; 
        if(String.Equals(answer.ToLower(), "yes")) EditShow(); 
        break; 

    }
  
  }

  public void EditShow() 
  {
    // will prompt the user to make show edits. able to call Delete() and Update() 
    // the user will enter the ID of the anime they want to edit / delete 
    Console.WriteLine("Enter the number (from above) of the show you would like to edit from the list above.");
    int showChoice  = inputRetriever.GetChoice(); 

    // we will get show by ID 
    Show show = _showService.GetById(showChoice); 
    Console.WriteLine("Your selection: ");
    ShowPrinter.Print(show); 

    // prompt user to choose what they want to do 
    Console.WriteLine("What would you like to do with this show?");
    Console.WriteLine("1. Edit Last Episode Watched"); 
    Console.WriteLine("2. Mark/unmark show as a favorite");
    Console.WriteLine("3. Delete Show");

    int editChoice = inputRetriever.GetChoice() ; 
    switch(editChoice) {
      case 1: 
     
        System.Console.WriteLine($"Enter a number from 0(unwatched) to {show.Episodes}: ");
        int episode =  inputRetriever.GetChoice() ; 
        bool success = _showService.ChangeLastWatched(show,episode); 
        if (success) Console.WriteLine($"Show {show.Name} had its last episode watched set to {episode} out of {show.Episodes} 🫡");
        else System.Console.WriteLine("Entered episode is outside the available range");
        break; 
      case 2: 
        // mark as favorite 
        bool favStatus = _showService.ChangeFavoriteStatus(show); 
        string favStatusChange = favStatus ? "marked" : "unmarked"; 
        Console.WriteLine($"Show {show.Name} has been {favStatusChange} as a favorite🫡");

        break; 
      case 3: 
        // delete show 
        DeleteShow(show);
        Console.WriteLine($"Show {show.Name} has been removed from your list 🫡");
        break; 
      default: 
        System.Console.WriteLine("No valid change selected!");
        break;

    }

  }

  public void DeleteShow(Show show) {
      _showService.Delete(show); 
  }

}
  