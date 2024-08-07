using Moq; 
using AniView.Entities;
using AniView.Service;
using AniView.DAO;
using AniView.Utilities;
using AniView.Tests.Mocks; 

namespace AniView.Tests.ServiceTests; 

public class ShowServiceTests {
    // starting arrangement for all tests 

    private static readonly User user1 = new()  {UserID=1,UserName="test",Passphrase="test",Salt=DateTime.Now.ToString()};
    private static readonly List<Show> animeList = [
        new() {Name="myAnime1",UserID=1,Episodes=20,LastEpisodeWatched=1,Favorite=false,DateLastWatched=DateTime.Now},
        new() {Name="myAnime2",UserID=1,Episodes=30,LastEpisodeWatched=1,Favorite=false,DateLastWatched=DateTime.Now},
        new() {Name="myAnime3",UserID=1,Episodes=40,LastEpisodeWatched=1,Favorite=false,DateLastWatched=DateTime.Now},
    ]; 
    private static readonly IQueryable<Show> data = animeList.AsQueryable() ; 

    // Create Testing 
    [Fact]
    public void AddNewShow_Should_CallAddOnTheDbSetandSave() {
        MockDbSet<Show> mockSet = new(animeList);
        Mock<ApplicationDbContext> mockContext = new();


        mockContext.Setup(m=>m.Shows).Returns(mockSet.Object);
        ShowDAO mDAO = new(mockContext.Object);
        ShowService mService = new(mDAO); 
        

      
        Anime anime= new("url","myAnime","myAnime",false, 1) ;
        Console.SetIn(new StringReader("1")); 

        // Act 
        bool createSuccess = mService.Create(user1, anime); 

        // Assert that it has saved 
        Assert.True(createSuccess);
        mockSet.Verify(m=>m.Add(It.IsAny<Show>()),Times.Once());
        mockContext.Verify(m=>m.SaveChanges(),Times.Once());
        // Show savedShow = mDAO.GetByName(user1,"myAnime");
        // Assert.NotNull(savedShow); 
        List<Show> newShowList= (List<Show>)  mDAO.GetAll();
        System.Console.WriteLine("show list length after add: " + newShowList.Count);
       //Assert.Equal(4, newShowList.Count); 
    }

    // Read- GetAll() testing 
    [Fact] 
    public void GetAllShows_Should_ReturnShowList() {
        // Arrange 
     
        MockDbSet<Show> mockSet = new(animeList);

        Mock<ApplicationDbContext> mockContext = new();
     
        mockContext.Setup(m=>m.Shows).Returns(mockSet.Object);
        ShowDAO mDAO = new(mockContext.Object);
        ShowService mService = new(mDAO); 

        // Act 
        List<Show> testList = mService.GetAll(); 


        // Assert Basic properties
        //Assert.IsType<List<Show>>(testList);
        Assert.NotEmpty(testList);
        Assert.Equal(3,animeList.Count);
        
        // assert that showlist and testlist contain the same shows
        Assert.Equal(animeList.ElementAt(0).Name,testList.ElementAt(0).Name);
        Assert.Equal(animeList.ElementAt(1).Name,testList.ElementAt(1).Name);
        Assert.Equal(animeList.ElementAt(2).Name,testList.ElementAt(2).Name);

    }

    //  Read- GetByID  testing 
    [Fact]
    public void GetById_Should_RetrieveTheCorrectlyNamedShow() {
        // arrange: setup mock db context, service, and DAO 
        MockDbSet<Show> mockSet = new(animeList);
        Mock<ApplicationDbContext> mockContext = new();
       

        mockContext.Setup(m=>m.Shows).Returns(mockSet.Object);
        ShowDAO mDAO = new(mockContext.Object);
        ShowService mService = new(mDAO); 

        // Act: call on the DAO for the first anime to test against the one retreived by its ID from the service 
        Show firstShow = mDAO.GetByName(user1,"myAnime1");
        Show foundShow = mService.GetById(firstShow.ShowID); 
        
        // assert names match 
        Assert.NotNull(foundShow);
        Assert.Equal(firstShow.Name, foundShow.Name);
        

    }
    //  Read- GetAllByUserName  testing 
   
    // TODO: Update testing 

    [Fact]
    public void UpdateShow_Should_UpdateEpisodeAndDateLastWatched () {
        // arrange :
        MockDbSet<Show> mockSet = new(animeList);
        Mock<ApplicationDbContext> mockContext = new();
     
        mockContext.Setup(m=>m.Shows).Returns(mockSet.Object);
        ShowDAO mDAO = new(mockContext.Object);
        ShowService mService = new(mDAO); 
        
        // act -  get show 1 and update last episode watched 
        // in order to not rely on service functionality, we call getByID directly 
        Show testShow  = mDAO.GetByName(user1,"myAnime1"); 
        //Assert.NotNull(testShow);
        // update show 
        int newLastEpisode =15; 
        testShow.LastEpisodeWatched = newLastEpisode; 
        mService.Update(testShow);
      

        // assert : check that the show episode as retreived from the DAO reflects the change 
     
        Show updatedShow = mDAO.GetById(testShow.ShowID); 
        Assert.Equal(newLastEpisode, updatedShow.LastEpisodeWatched);
 
    }

    // Delete testing 
    [Fact]
    public void DeleteShow_Should_CallDeleteOnTheDbSet_AndSave() {
        // arrange 
        MockDbSet<Show> mockSet = new(animeList);
        Mock<ApplicationDbContext> mockContext = new();
  

        mockContext.Setup(m=>m.Shows).Returns(mockSet.Object);
        ShowDAO mDAO = new(mockContext.Object);
        ShowService mService = new(mDAO);
        List<Show> showList= (List<Show>) mDAO.GetAll(); 

        // act 
        mService.Delete(showList.ElementAt(0));

        // assert  
        List<Show> newShowList = (List<Show>) mDAO.GetAll() ; 

        Assert.Equal(1, showList.Count - newShowList.Count);

    }
}