namespace AniView.Utilities; 
public class InputRetriever {
    public static string GetName() {
      bool isValid = false; 
      string name = "" ; 
      while(!isValid) {
        Console.WriteLine("Enter the name of an anime you want to add to your list: ");
        name = Console.ReadLine() ?? "" ; 
        isValid  = Validator.CheckNameIsValid(name); 
        if(!isValid) System.Console.WriteLine("Invalid username! Can't start with a number. Try again.");
      }
      return name; 
    }
    public static string GetUserName() {
      bool isValid = false; 
      string name = "" ; 
      while(!isValid) {
        System.Console.WriteLine("Enter username:");
        name = Console.ReadLine() ?? "" ; 
        isValid  = Validator.CheckNameIsValid(name); 
        if(!isValid) System.Console.WriteLine("Invalid username! Can't start with a number. Try again.");
      }
      return name; 
    }

    public static int GetChoice() 
    {

    var choiceIsValid = int.TryParse(Console.ReadLine(), out int num);

    while (!choiceIsValid) 
    {
      System.Console.WriteLine("Please select a valid number!");
      choiceIsValid =  int.TryParse(Console.ReadLine(),out num);
    }

    return num; 


    }

    public static string GetPassword(){
      bool isValid = false ; 
      string password=""; 
      while(!isValid) {
        System.Console.WriteLine("Enter a new password: ");
        password = Console.ReadLine();
        isValid= Validator.CheckInputIsNotEmpty(password); 
        if (!isValid) System.Console.WriteLine("You didn't enter anything! Try again. ");
      }
      return password;
    }

    public static string FormatDateTime(DateTime dateTime) 
    {
      string dateTimeString = "" ;
      dateTimeString += dateTime.Year + "-";  
      dateTimeString += dateTime.Month + "-";
      dateTimeString += dateTime.Day + "_";
      dateTimeString += dateTime.Hour + ":";
      dateTimeString += dateTime.Minute + ":";
      dateTimeString += dateTime.Second;
    
      return dateTimeString;  
  }
}