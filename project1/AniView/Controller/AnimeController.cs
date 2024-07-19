using System.Net.Http.Headers;
using AniView.Entities;
using AniView.Service;
using Newtonsoft.Json; 

namespace AniView.Controller; 
public class AnimeController {
  // tagString will be used for API request 
  private string name = ""  ; 
  // tagArray may be used for storage of the characteristics in the database 


  private string _user; 
  private User _User; 
  private DateTime _reqDate = DateTime.Now; 

  private string _prompt = "" ;

  private readonly ShowService _showService; 


  private static string _baseURL = "https://api.jikan.moe/v4/anime?q=";  

  private static HttpClient s_client = new() 
  {
    BaseAddress = new Uri(_baseURL)
  }; 

  public AnimeController(User user, ShowService service ) 
  {  
    _User = user; 
    _user = _User.UserName ; 
    _showService = service; 

    BuildPrompt() ; 
  }

 

  private void BuildPrompt() {
    string capitalizedName = string.Concat(_user[0].ToString().ToUpper(), _user.AsSpan(1));
    _prompt += $"Hello {capitalizedName}! Welcome to AniView, the anime watch tracker app!\n";  
    _prompt += "Enter the name of an anime you want to add to your list: " ;

  }

  public bool Prompt() 
  {
    Console.WriteLine(_prompt);

    string name =  Console.ReadLine() ?? "";

    if (name == "") {
      Console.WriteLine("You didn't enter anything! 😭");
      Console.WriteLine("Try again. \n");
      return false ;
    } else {
      this.name =  name; 
      return true; 
    }
  }

  private string FormatDateTime() {
    string dateTimeString = "" ;
    dateTimeString += _reqDate.Year + "-";  
    dateTimeString += _reqDate.Month + "-";
    dateTimeString += _reqDate.Day + "_";
    dateTimeString += _reqDate.Hour + ":";
    dateTimeString += _reqDate.Minute + ":";
    dateTimeString += _reqDate.Second;
   
   return dateTimeString;  
  }

  // GetCat retrieves cat with _tags from CatAPI 
  async public Task GetAnime() 
  {
    
    System.Console.WriteLine($" Name: {name} \n");
    System.Console.WriteLine($" Retreiving Anime ... ");
    // record precise time API call was made 
    _reqDate = DateTime.Now; 

    string completeURL = _baseURL + $"{name}&sfw"; 
    using HttpResponseMessage response = await s_client.GetAsync(completeURL); 

    try 
    {
        response.EnsureSuccessStatusCode();
        var streamResponse = await response.Content.ReadAsStringAsync();
        // System.Console.WriteLine(streamResponse);
        File.WriteAllText("./obs/traces/HttpResponse.txt",streamResponse); 

        System.Console.WriteLine("Found the following anime matches under that search: ");
        //  deserialize JSON 
        AnimeList animeList = JsonConvert.DeserializeObject<AnimeList>(streamResponse);
        for ( int i = 0; i < animeList.data.Length; i++ ) 
        {
            System.Console.WriteLine($"{i+1}.  {animeList.data[i].ToString()}" );
        }


        // now have them select anime from list 
        System.Console.WriteLine("Which one would you like to track? Enter the number");
        int choice = Int32.Parse(Console.ReadLine()) -1 ; 

        AnimeListAnime anime = animeList.data[choice];
        System.Console.WriteLine($"Your choice: {anime.ToString()}");

        // next: add this to DB 
        _showService.Create(_User,anime);

    } catch (HttpRequestException e) {
      System.Console.WriteLine("Couldn't find anything! Try another anime name 😅");
      
      
      // log exception to file 
      File.WriteAllText("./obs/logs/controllerLog.txt",e.Message); 
    }
  



  }
}