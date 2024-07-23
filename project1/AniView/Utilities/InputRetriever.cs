namespace AniView.Utilities; 
public class InputRetriever {
    public virtual string GetName() {
        return Console.ReadLine() ?? ""; 
    }

    public virtual int GetChoice() 
    {

    var choiceIsValid = int.TryParse(Console.ReadLine(), out int num);

    while (!choiceIsValid) 
    {
      System.Console.WriteLine("Please select a valid number!");
      choiceIsValid =  int.TryParse(Console.ReadLine(),out num);
    }

    return num; 


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