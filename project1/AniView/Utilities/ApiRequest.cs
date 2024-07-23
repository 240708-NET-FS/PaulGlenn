using AniView.Utilities;
using Newtonsoft.Json; 
namespace AniView.Utilities; 
public class AnimeApiRequest {

  private static readonly string s_baseURL = "https://api.jikan.moe/v4/anime?q="; 
  private static readonly HttpClient s_client = new() 
  {
    BaseAddress = new Uri(s_baseURL)
  }; 

   async public static Task<AnimeListResponse> GetAnimeList(string name ) {
    string completeURL = s_baseURL + $"{name}&sfw"; 
    using HttpResponseMessage response = await s_client.GetAsync(completeURL); 
  
  try 
    {
      response.EnsureSuccessStatusCode();
      var streamResponse = await response.Content.ReadAsStringAsync();
      // Console.WriteLine(streamResponse);
      File.WriteAllText("./HttpResponse.txt",streamResponse); 

      //  deserialize JSON 
      AnimeListResponse animeList = JsonConvert.DeserializeObject<AnimeListResponse>(streamResponse) ?? new();
      return animeList; 

    } 
    catch (HttpRequestException e) 
    {
    
    
      // log exception to file 
      File.WriteAllText("/Users/nls.pglenn/github/revature/PaulGlenn/project1/AniView/obs/logs/controllerLog.txt",e.Message); 
      return null; 
    }

   }

}