using Moq; 
using Microsoft.EntityFrameworkCore;
using AniView.Entities;
using AniView.Service;
using AniView.DAO;
using AniView.Utilities;

namespace AniView.Tests.ServiceTests; 

public class ShowServiceTests {
    // starting arrangement for all tests 

    private static User user1 = new()  {UserID=1,UserName="test",Passphrase="test"};
    private static readonly List<Show> showList = [
        new() {Name="myAnime1",UserID=1,Episodes=20,LastEpisodeWatched=1,Favorite=false,DateLastWatched=DateTime.Now},
        new() {Name="myAnime2",UserID=1,Episodes=30,LastEpisodeWatched=1,Favorite=false,DateLastWatched=DateTime.Now},
        new() {Name="myAnime3",UserID=1,Episodes=40,LastEpisodeWatched=1,Favorite=false,DateLastWatched=DateTime.Now},
    ]; 
    private static readonly IQueryable<Show> data = showList.AsQueryable() ; 

    // Create Testing 
    [Fact]
    public void AddNewShow_Should_CallAddOnTheDbSetandSave() {
        Mock<DbSet<Show>> mockSet = new();
        Mock<ApplicationDbContext> mockContext = new();
        mockSet.As<IQueryable<Show>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<Show>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<Show>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<Show>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

        mockContext.Setup(m=>m.Shows).Returns(mockSet.Object);
        ShowDAO mDAO = new(mockContext.Object);
        ShowService mService = new(mDAO); 
        

      
        Anime anime= new("url","myAnime","myAnime",false,1) ; 
        Console.SetIn(new StringReader("1")); 

        // Act 
        mService.Create(user1, anime); 

        mockSet.Verify(m=>m.Add(It.IsAny<Show>()),Times.Once());
        mockContext.Verify(m=>m.SaveChanges(),Times.Once());
    }

    // Read- GetAll() testing 
    [Fact] 
    public void GetAllShows_Should_ReturnShowList() {
        // Arrange 
        Mock<DbSet<Show>> mockSet = new();
        Mock<ApplicationDbContext> mockContext = new();
        mockSet.As<IQueryable<Show>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<Show>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<Show>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<Show>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
        mockContext.Setup(m=>m.Shows).Returns(mockSet.Object);
        ShowDAO mDAO = new(mockContext.Object);
        ShowService mService = new(mDAO); 

        // Act 
        List<Show> testList = mService.GetAll(); 


        // Assert Basic properties
        //Assert.IsType<List<Show>>(testList);
        Assert.NotEmpty(testList);
        Assert.Equal(3,showList.Count);
        
        // assert that showlist and testlist contain the same shows
        Assert.Equal(showList.ElementAt(0).Name,testList.ElementAt(0).Name);
        Assert.Equal(showList.ElementAt(1).Name,testList.ElementAt(1).Name);
        Assert.Equal(showList.ElementAt(2).Name,testList.ElementAt(2).Name);

    }

    //  Read- GetByID  testing 
    [Fact]
    public void GetById_Should_RetrieveTheCorrectlyNamedShow() {
        // arrange: setup mock db context, service, and DAO 
        Mock<DbSet<Show>> mockSet = new();
        Mock<ApplicationDbContext> mockContext = new();
        mockSet.As<IQueryable<Show>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<Show>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<Show>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<Show>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

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
        Mock<DbSet<Show>> mockSet = new();
        Mock<ApplicationDbContext> mockContext = new();
        mockSet.As<IQueryable<Show>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<Show>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<Show>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<Show>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

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
      

        // check that save has been called and that record has updated 
        mockContext.Verify(m=>m.SaveChanges(),Times.Once());
        Show updatedShow = mDAO.GetById(testShow.ShowID); 
        // Assert.NotNull(updatedShow);
        Assert.Equal(newLastEpisode, updatedShow.LastEpisodeWatched);
 
    }

    // Delete testing 
    [Fact]
    public void DeleteShow_Should_CallDeleteOnTheDbSet_AndSave() {

        Mock<DbSet<Show>> mockSet = new();
        Mock<ApplicationDbContext> mockContext = new();
        mockSet.As<IQueryable<Show>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<Show>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<Show>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<Show>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

        mockContext.Setup(m=>m.Shows).Returns(mockSet.Object);
        ShowDAO mDAO = new(mockContext.Object);
        ShowService mService = new(mDAO); 
        mService.Delete(showList.ElementAt(0));
        mockSet.Verify(m=>m.Remove(It.IsAny<Show>()),Times.Once()); 
        mockContext.Verify(m=>m.SaveChanges(), Times.Once());
        

    }
}