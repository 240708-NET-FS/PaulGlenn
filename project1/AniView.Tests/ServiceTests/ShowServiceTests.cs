using Moq; 
using Microsoft.EntityFrameworkCore;
using AniView.Entities;
using AniView.Service;
using AniView.DAO;
using AniView.Utilities;

namespace AniView.Tests.ServiceTests; 

public class ShowServiceTests {
    // starting arrangement for all tests 
    private static readonly List<Show> showList = [
        new() {Name="myAnime1",UserID=1,Episodes=1,LastEpisodeWatched=1,Favorite=false,DateLastWatched=DateTime.Now},
        new() {Name="myAnime2",UserID=1,Episodes=1,LastEpisodeWatched=1,Favorite=false,DateLastWatched=DateTime.Now},
        new() {Name="myAnime3",UserID=1,Episodes=1,LastEpisodeWatched=1,Favorite=false,DateLastWatched=DateTime.Now},
    ]; 
    static readonly IQueryable<Show> data = showList.AsQueryable() ; 

    // Create Testing 
    [Fact]
    public void AddNewShow_Should_CallAddOnTheDbSetandSave() {
        var mockSet = new Mock<DbSet<Show>>();
        var mockContext = new Mock<ApplicationDbContext>();
        mockSet.As<IQueryable<Show>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<Show>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<Show>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<Show>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

        mockContext.Setup(m=>m.Shows).Returns(mockSet.Object);
        var mDAO = new ShowDAO(mockContext.Object); 
        var mService = new ShowService(mDAO); 
        

        User user = new() {UserID=1,UserName="test",Passphrase="test"};
        Anime anime= new("url","myAnime","myAnime",false,1) ; 
        Console.SetIn(new StringReader("1")); 

        // Act 
        mService.Create(user, anime); 

        mockSet.Verify(m=>m.Add(It.IsAny<Show>()),Times.Once());
        mockContext.Verify(m=>m.SaveChanges(),Times.Once());
    }

    // Read- GetAll() testing 
    [Fact] 
    public void GetAllShows_Should_ReturnShowList() {
        // Arrange 
        var mockSet = new Mock<DbSet<Show>>();
        var mockContext = new Mock<ApplicationDbContext>();
        mockSet.As<IQueryable<Show>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<Show>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<Show>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<Show>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
        mockContext.Setup(m=>m.Shows).Returns(mockSet.Object);
        var mDAO = new ShowDAO(mockContext.Object); 
        var mService = new ShowService(mDAO); 

        // Act 
        List<Show> testList = mService.GetAll(); 


        // Assert Basic properties
        Assert.IsType<List<Show>>(testList);
        Assert.NotEmpty(testList);
        Assert.Equal(3,showList.Count);
        
        // assert that showlist and testlist contain the same shows
        Assert.Equal(showList.ElementAt(0).Name,testList.ElementAt(0).Name);
        Assert.Equal(showList.ElementAt(1).Name,testList.ElementAt(1).Name);
        Assert.Equal(showList.ElementAt(2).Name,testList.ElementAt(2).Name);

    }

    // TODO: Read- GetByID and GetByName testing 

    // TODO: Update testing 
    
    // Delete testing 
    [Fact]
    public void DeleteShow_Should_CallDeleteOnTheDbSet_AndSave() {

        var mockSet = new Mock<DbSet<Show>>();
        var mockContext = new Mock<ApplicationDbContext>();
        mockSet.As<IQueryable<Show>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<Show>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<Show>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<Show>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

        mockContext.Setup(m=>m.Shows).Returns(mockSet.Object);
        var mDAO = new ShowDAO(mockContext.Object); 
        var mService = new ShowService(mDAO); 
        mService.Delete(showList.ElementAt(0));
        mockSet.Verify(m=>m.Remove(It.IsAny<Show>()),Times.Once()); 
        mockContext.Verify(m=>m.SaveChanges(), Times.Once());
        

    }
}