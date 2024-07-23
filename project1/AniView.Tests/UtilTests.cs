using System.ComponentModel.DataAnnotations;
using AniView.Utilities; 
namespace AniView.Tests;

public class AniViewTests_UtilTestsShould
{

   
    [Fact]
    public void NameStartingWithNumber_ShouldBeInvalid()
    {
        // Arrange 
        string testName = "9Paul"; 
        
        // Act 
        bool isTestNameValid = Utilities.Validator.CheckNameIsValid(testName);

        // Assert 
        Assert.False(isTestNameValid); 
    }

    [Fact]
    public void ProperNameShouldBeValid() {
        string testName = "RealName"; 
        bool isTestNameValid = Utilities.Validator.CheckNameIsValid(testName); 
        Assert.True(isTestNameValid);
    }
}