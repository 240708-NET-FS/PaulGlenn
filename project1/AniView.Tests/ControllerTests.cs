using System.ComponentModel.DataAnnotations;
using AniView.Controller;
using AniView.Entities;
using AniView.Utilities;
namespace AniView.Tests;

public class AniViewTests_ControllerShould
{
    [Fact]
    public async void ControllerShouldRetrieveAListOfAnime()
    {
        // Arrange: mock text input name with Console.SetIn
        string mockName = "My Hero Academia"; 
        Console.SetIn(new StringReader(mockName));

        ShowController showController = new() ; 
        Anime[] animeList = await showController.GetShowList(mockName); 
       
        // Assert that the anime list has populated with something  
        Assert.IsType<Anime[]>(animeList);
        Assert.NotEmpty(animeList);
        
    }
}