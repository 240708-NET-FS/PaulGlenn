using System.Net.Http.Headers;
using CatApp.Entities;

class CatAPIRequest {
  // tagString will be used for API request 
  private string _tagString = ""  ; 
  // tagArray may be used for storage of the characteristics in the database 
  private string[] _tagArray = []; 

  private string _reqName = "" ; 
  private DateTime _reqDate = DateTime.Now; 

  private string _prompt = "" ;

  private static HttpClient s_client = new() 
  {
    BaseAddress = new Uri("https://cataas.com/cat/"), 
  }; 

  public CatAPIRequest(string name ) 
  {  
    this._reqName = name; 
    BuildPrompt() ; 
  }

 

  private void BuildPrompt() {
    string capitalizedName = string.Concat(this._reqName[0].ToString().ToUpper(), this._reqName.AsSpan(1));
    this._prompt += $"Hello {capitalizedName}! Welcome to the cat app!\n"; 
    this._prompt += "Enter the type of cat you want to see.\n"; 
    this._prompt += "Options include: color , emotion, cuteness.\n"; 
    this._prompt += "For multiple options, separate with a comma.\n"; 
    this._prompt += "ex. input 'orange, cute' => a picture of an orange cute 🐈\n"; 
    this._prompt += "Now, enter your cat characteristics: " ;

  }

  public bool Prompt() 
  {
    Console.WriteLine(this._prompt);

    string characteristics =  Console.ReadLine() ?? "";

    if (characteristics == "") {
      Console.WriteLine("You didn't enter anything! 😭");
      Console.WriteLine("Try again. \n");
      return false ;
    } else {
      this.SetTags(characteristics); 
      return true; 
    }
  }
 
  public void SetTags(string tagString) 
  {
      this._tagString = tagString; 
      this._tagArray = tagString.Split(',');
  }

  private string FormatDateTime() {
    string dateTimeString = "" ;
    dateTimeString += this._reqDate.Year + "-";  
    dateTimeString += this._reqDate.Month + "-";
    dateTimeString += this._reqDate.Day + "_";
    dateTimeString += this._reqDate.Hour + ":";
    dateTimeString += this._reqDate.Minute + ":";
    dateTimeString += this._reqDate.Second;
   
   return dateTimeString;  
  }

  // GetCat retrieves cat with _tags from CatAPI 
  async public Task GetCat() 
  {
    
    System.Console.WriteLine($" Tags: {this._tagString} \n");
    System.Console.WriteLine($" Retreiving Cat ... ");
    // record precise time API call was made 
    this._reqDate = DateTime.Now; 
    string message = "You're an  awesome person. You can do anything you want! "

    using HttpResponseMessage response = await s_client.GetAsync($"{_tagString}/says/{message}"); 

    try {
      response.EnsureSuccessStatusCode();
      var streamResponse = await response.Content.ReadAsStreamAsync(); 
   
      string fout = $"./cats/{_reqName}_{String.Join("-",this._tagArray)}_cat_{FormatDateTime()}.jpg"; 
      Stream output = File.OpenWrite(fout); 
      streamResponse.CopyTo(output); 
      Console.WriteLine("Success! Your cat photo is located at: " + fout);


      // persist to database 
      using (var context = new ApplicationDbContext()) {
        Cat cat = new Cat { Tags=this._tagString, DateRequested=this._reqDate,Favorite=false,UserID=1 } ; 
        context.Cats.Add(cat); 
        context.SaveChanges();
      }
    
    } catch (HttpRequestException e) {
      System.Console.WriteLine("Sorry, that didn't work! Try another type of cat 😅");
      
      
      // log exception to file 
      File.WriteAllText("./logs/log.txt",e.Message); 
    }
  



  }
}